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

        private static DynamicDictionaryBase<string,string> JoinExpressionMapping;
        private static readonly ConcurrentDictionary<string, string> _joinDict;
        static JoinOperator()
        {
            _joinDict = new ConcurrentDictionary<string, string>();
            JoinExpressionMapping = _joinDict.PrecisioTree();
        }
        public static IEnumerable<TReturn> ToList<TReturn>(ISelect<TEntity> select, Expression<Func<TEntity, TReturn>> expression)
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

                    if (memberExpression.Expression.NodeType == ExpressionType.Convert ||
                        memberExpression.Expression.NodeType == ExpressionType.Call)
                    {
                        var callExp = ((MethodCallExpression)memberExpression.Expression);
                        var exp = callExp.Arguments[0];
                        var method = callExp.Method;
                        var type = method.DeclaringType;
                        var targetType = method.DeclaringType.GetGenericArguments()[1];
                        if (type.Name.StartsWith("AriesInnerJoin"))
                        {
                            ////var action = NDelegate
                            ////    .DefaultDomain()
                            ////    .Action<ISelect<TEntity>,Expression>
                        }
                        else if (type.Name.StartsWith("AriesLeftJoin"))
                        {

                        }
                        else if (type.Name.StartsWith("AriesRightJoin"))
                        {

                        }

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
            return default;
        }
        #region MyRegion
        /*
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
                            var aliasName = "";
                            if (JoinExpressionMapping[scriptKey] == default)
                            {
                                string targetMemberName = default;
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
                                                var tempValues = memberExp.ToString().Split('.');
                                                aliasName = tempValues[0];
                                                targetMemberName = tempValues[1];
                                                _joinDict[scriptKey] = aliasName;
                                                JoinExpressionMapping.Dispose();
                                                JoinExpressionMapping = _joinDict.PrecisioTree(DynamicCache.DyanamicCacheDirection.KeyToValue);
                                            }
                                        }

                                    }
                                }


                                if (methodExp.NodeType == ExpressionType.Call)
                                {

                                    var joinTableType = methodExp.Method.GetGenericArguments()[0].GetDevelopName();
                                    string condition = default;
                                    if (methodExp.Arguments.Count > 2)
                                    {
                                        var exp = methodExp.Arguments[2];
                                        if (exp.NodeType == ExpressionType.Quote)
                                        {
                                            condition = "&&" + ((LambdaExpression)(((UnaryExpression)exp).Operand)).Body.ToString();
                                        }
                                    }
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
                                            joinScript.Append($"obj.InnerJoin<{joinTableType}>((a,{aliasName})=>a.{sourceMemberName} == {aliasName}.{targetMemberName}{condition});");
                                        }
                                        else if (joinType == "AriesLeftJoin")
                                        {
                                            joinScript.Append($"obj.LeftJoin<{joinTableType}>((a,{aliasName})=>a.{sourceMemberName} == {aliasName}.{targetMemberName}{condition});");
                                        }
                                        else if (joinType == "AriesRightJoin")
                                        {
                                            joinScript.Append($"obj.RightJoin<{joinTableType}>((a,{aliasName})=>a.{sourceMemberName} == {aliasName}.{targetMemberName}{condition});");
                                        }

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

        }*/
        #endregion


    }



}
