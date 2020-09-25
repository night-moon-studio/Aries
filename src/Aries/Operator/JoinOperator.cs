using FreeSql;
using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Aries
{
    //public static class JoinOperator<TEntity, TReturn> where TEntity : class
    //{

    //    public static ImmutableDictionary<long, string> JoinExpressionMapping;
    //    static JoinOperator()
    //    {
    //        JoinExpressionMapping = ImmutableDictionary.Create<long, string>();
    //    }
    //}

    public static class JoinOperator<TEntity> where TEntity : class
    {

        public static ImmutableDictionary<long,string> JoinExpressionMapping;
        static JoinOperator()
        {
            JoinExpressionMapping = ImmutableDictionary.Create<long, string>();
        }

        public static IEnumerable<TReturn> ToList<TReturn>(ISelect<TEntity> select, Expression<Func<TEntity, TReturn>> expression)
        {

            var code = expression.GetHashCode();

            //查询表达式树是否为之前处理过的
            if (JoinAction<TEntity, TReturn>.Action==null)
            {

                Dictionary<string, string> tempCache = new Dictionary<string, string>();
                //给匿名类创建一个代理类
                StringBuilder fieldsScript = new StringBuilder();
                StringBuilder joinScript = new StringBuilder();
                // 获取构造函数参数
                var arguments = ((NewExpression)expression.Body).Arguments;
                //获取匿名类成员
                var members = ((NewExpression)expression.Body).Members;

                
                for (int i = 0; i < arguments.Count; i++)
                {
                    // 方法类型参数
                    if (arguments[i].NodeType == ExpressionType.Call)
                    {
                       
                        string targetMemberName = default;
                        //var member = item.ToString().Split('.')[1];
                        var methodExp = (MethodCallExpression)arguments[i];
                        if (methodExp.Arguments[0].NodeType == ExpressionType.Quote)
                        {
                            var unaryExp = (UnaryExpression)(methodExp.Arguments[0]);
                            if (unaryExp.NodeType == ExpressionType.Quote)
                            {
                                var lbdExp = (LambdaExpression)(unaryExp.Operand);
                                var memberExp = (MemberExpression)(lbdExp.Body);
                                targetMemberName = memberExp.Member.Name;

                            }
                        }
                        if (!tempCache.ContainsKey(arguments[i].ToString()))
                        {
                            if (methodExp.Object.NodeType == ExpressionType.Call)
                            {
                                var callerExp = (MethodCallExpression)(methodExp.Object);
                                var joinTableName = callerExp.Method.GetGenericArguments()[0].Name;
                                var joinType = callerExp.Method.Name;
                                if (callerExp.Arguments[0].NodeType == ExpressionType.MemberAccess)
                                {
                                    var memberExp = (MemberExpression)(callerExp.Arguments[0]);
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
                                    tempCache[arguments[i].ToString()] = joinAliasScript;
                                    joinScript.Append($"\"{joinTableName}\" AS {joinAliasScript} ON a.\"{sourceMemberName}\" == {joinAliasScript}.\"{targetMemberName}\"".Replace("\"", "\\\""));
                                    joinScript.AppendLine("\");");

                                }
                            }
                        }
                        fieldsScript.Append($"{tempCache[arguments[i].ToString()]}.\"{targetMemberName}\" AS  \"{members[i].Name}\",");

                    }
                    else if (arguments[i].NodeType == ExpressionType.MemberAccess)
                    {
                       
                        var memberExpression = (MemberExpression)arguments[i];
                        if (members[i].Name == memberExpression.Member.Name)
                        {

                            fieldsScript.Append($"a.\"{memberExpression.Member.Name}\",");
                        }
                        else
                        {
                            fieldsScript.Append($"a.\"{memberExpression.Member.Name}\" AS \"{members[i].Name}\",");
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
            var sql = select.ToSql(JoinAction<TEntity, TReturn>.FieldsScript);
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




    public class InnerJoin<OutEntity>
    {
        public TReturn MapFrom<TReturn>(Expression<Func<OutEntity, TReturn>> expression)
        {
            return default(TReturn);
        }
    }
    public class LeftJoin<OutEntity>
    {
        public TReturn MapFrom<TReturn>(Expression<Func<OutEntity, TReturn>> expression)
        {
            return default(TReturn);
        }
    }
    public class RightJoin<OutEntity>
    {
        public TReturn MapFrom<TReturn>(Expression<Func<OutEntity, TReturn>> expression)
        {
            return default(TReturn);
        }
    }
}
