using FreeSql;
using Natasha.CSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Aries
{

    public static class JoinOperator<TEntity> where TEntity : class
    {

        public static readonly ConcurrentDictionary<string, string> JoinExpressionMapping;
        static JoinOperator()
        {
            JoinExpressionMapping = new ConcurrentDictionary<string, string>();
        }

        public static IEnumerable<TReturn> ToList<TReturn>(ISelect<TEntity> select, Expression<Func<TEntity, TReturn>> expression)
        {

            //查询表达式树是否为之前处理过的
            if (JoinAction<TEntity, TReturn>.Action == null)
            {

                //给匿名类创建一个代理类
                StringBuilder fieldsScript = new StringBuilder();
                StringBuilder joinScript = new StringBuilder();
                // 获取构造函数参数
                var arguments = ((NewExpression)expression.Body).Arguments;
                //获取匿名类成员
                var members = ((NewExpression)expression.Body).Members;


                for (int i = 0; i < arguments.Count; i++)
                {

                    if (arguments[i].NodeType == ExpressionType.MemberAccess)
                    {

                        var memberExpression = (MemberExpression)arguments[i];

                        var memberName = memberExpression.Member.Name;

                        // 方法类型参数
                        if (memberExpression.Expression.NodeType == ExpressionType.Convert ||
                        memberExpression.Expression.NodeType == ExpressionType.Call)
                        {

                            var scriptKey = memberExpression.Expression.ToString();
                            if (!JoinExpressionMapping.ContainsKey(scriptKey))
                            {
                                string targetMemberName = default;
                                //var member = item.ToString().Split('.')[1];
                                var methodExp = (MethodCallExpression)memberExpression.Expression;
                                if (methodExp.Arguments[1].NodeType == ExpressionType.Quote)
                                {
                                    var unaryExp = (UnaryExpression)(methodExp.Arguments[1]);
                                    if (unaryExp.NodeType == ExpressionType.Quote)
                                    {

                                        var lbdExp = (LambdaExpression)(unaryExp.Operand);
                                        if (lbdExp.Body.NodeType == ExpressionType.Convert)
                                        {
                                            var bodyExp = (UnaryExpression)lbdExp.Body;
                                            if (bodyExp.Operand.NodeType == ExpressionType.MemberAccess)
                                            {
                                                var memberExp = (MemberExpression)(bodyExp.Operand);
                                                targetMemberName = memberExp.Member.Name;
                                            }
                                        }

                                    }
                                }


                                if (methodExp.NodeType == ExpressionType.Call)
                                {
                                    //var callerExp = (MethodCallExpression)(methodExp.Object);
                                    var joinTableType = methodExp.Method.GetGenericArguments()[0];
                                    var joinTableName = TableInfomation.GetRealTableName(joinTableType);
                                    var joinType = methodExp.Method.Name;

                                    MemberExpression memberExp = default;
                                    if (methodExp.Arguments[0].NodeType == ExpressionType.Convert)
                                    {
                                        memberExp = (MemberExpression)((UnaryExpression)(methodExp.Arguments[0])).Operand;
                                    }
                                    else if (methodExp.Arguments[0].NodeType == ExpressionType.MemberAccess)
                                    {
                                        memberExp = (MemberExpression)(methodExp.Arguments[0]);
                                    }
                                    if (memberExp != default)
                                    {
                                        var sourceMemberName = memberExp.Member.Name;

                                        if (joinType == "AriesInnerJoin")
                                        {
                                            joinScript.Append("obj.InnerJoin(\"");
                                        }
                                        else if (joinType == "AriesLeftJoin")
                                        {
                                            joinScript.Append("obj.LeftJoin(\"");
                                        }
                                        else if (joinType == "AriesRightJoin")
                                        {
                                            joinScript.Append("obj.RightJoin(\"");
                                        }

                                        var joinAliasScript = $"{joinTableName}_{joinType}_{sourceMemberName}";
                                        JoinExpressionMapping[scriptKey] = joinAliasScript;
                                        joinScript.Append($"\"{joinTableName}\" AS {joinAliasScript} ON a.\"{sourceMemberName}\" = {joinAliasScript}.\"{targetMemberName}\"".Replace("\"", "\\\""));
                                        joinScript.AppendLine("\");");

                                    }
                                }
                            }
                            fieldsScript.Append($"{JoinExpressionMapping[scriptKey]}.\"{memberName}\" AS \"{members[i].Name}\",");

                        }
                        else
                        {
                            if (members[i].Name == memberName)
                            {

                                fieldsScript.Append($"a.\"{memberName}\",");
                            }
                            else
                            {
                                fieldsScript.Append($"a.\"{memberName}\" AS \"{members[i].Name}\",");
                            }
                        }



                    }
                }
                if (fieldsScript.Length > 1)
                {

                    fieldsScript.Length -= 1;
                    JoinAction<TEntity, TReturn>.FieldsScript = fieldsScript.ToString();
                    JoinAction<TEntity, TReturn>.Action = NDelegate
                        .DefaultDomain()
                        .Action<ISelect<TEntity>>(joinScript.ToString());

                }

            }
            //调用 TReturn 的处理函数
            JoinAction<TEntity, TReturn>.Action(select);
            //返回执行结果
            return select.ToList<TReturn>(JoinAction<TEntity, TReturn>.FieldsScript);
            //return ProxyCaller<TEntity, TReturn>.ToList(code, select);

        }

    }

    /// <summary>
    /// 存放与返回值相关的外联查询处理函数
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public static class JoinAction<TEntity, TReturn> where TEntity : class
    {
        public static Action<ISelect<TEntity>> Action;
        public static string FieldsScript;
    }

}
