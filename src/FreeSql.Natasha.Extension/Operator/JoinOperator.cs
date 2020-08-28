using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace FreeSql.Natasha.Extension
{
    public static class JoinOperator<TEntity, TReturn> where TEntity : class
    {

        public static ImmutableDictionary<long,string> JoinExpressionMapping;
        static JoinOperator()
        {
            JoinExpressionMapping = ImmutableDictionary.Create<long, string>();
        }
        public static IEnumerable<TReturn> ToList<TempReturn>(ISelect<TEntity> select, Expression<Func<TEntity, TempReturn>> expression)
        {

            var code = expression.GetHashCode();
            HashSet<string> props = new HashSet<string>(typeof(TReturn).GetProperties().Select(item => item.Name));

            //查询表达式树是否为之前处理过的
            if (!JoinExpressionMapping.ContainsKey(code))
            {
                //给匿名类创建一个代理类
                StringBuilder script = new StringBuilder();
                // 获取构造函数参数
                var arguments = ((NewExpression)expression.Body).Arguments;
                //获取匿名类成员
                var members = ((NewExpression)expression.Body).Members;
                var joinTypeFlagMapping = new Dictionary<Type, JoinTypeFlag>();
                for (int i = 0; i < arguments.Count; i++)
                {
                    // 方法类型参数
                    if (arguments[i].NodeType == ExpressionType.Call)
                    {
                        var methodExpression = (MethodCallExpression)arguments[i];
                        var methodDeclaringType = methodExpression.Method.DeclaringType;
                        if (methodDeclaringType.IsGenericType && methodDeclaringType.GetGenericTypeDefinition().Name.Contains("Join"))
                        {

                            var joinType = methodDeclaringType.GetGenericArguments()[0];
                            if (methodExpression.Arguments[0].NodeType == ExpressionType.Quote)
                            {
                                var quoteExpression = (UnaryExpression)methodExpression.Arguments[0];
                                if (quoteExpression.Operand.NodeType == ExpressionType.Lambda)
                                {
                                    var lambdaExpression = (LambdaExpression)quoteExpression.Operand;
                                    if (lambdaExpression.Body.NodeType == ExpressionType.MemberAccess)
                                    {
                                        var memberExpression = (MemberExpression)lambdaExpression.Body;
                                        var definitionType = methodDeclaringType.GetGenericTypeDefinition();
                                        if (definitionType == typeof(InnerJoin<>))
                                        {
                                            joinTypeFlagMapping[joinType] = JoinTypeFlag.Inner;
                                            script.Append(InnerJoin.GetJoinScript(joinType));
                                        }
                                        else if (definitionType == typeof(LeftJoin<>))
                                        {
                                            joinTypeFlagMapping[joinType] = JoinTypeFlag.Left;
                                            script.Append(LeftJoin.GetJoinScript(joinType));
                                        }
                                        else if (definitionType == typeof(RightJoin<>))
                                        {
                                            joinTypeFlagMapping[joinType] = JoinTypeFlag.Right;
                                            script.Append(RightJoin.GetJoinScript(joinType));
                                        }
                                        props.Remove(members[i].Name);
                                        script.Append($".\"{memberExpression.Member.Name}\" AS \"{members[i].Name}\",");
                                    }

                                }
                            }
                        }

                    }
                    else if (arguments[i].NodeType == ExpressionType.MemberAccess)
                    {

                        var memberExpression = (MemberExpression)arguments[i];
                        if (members[i].Name == memberExpression.Member.Name)
                        {
                            props.Remove(memberExpression.Member.Name);
                            script.Append($"a.\"{memberExpression.Member.Name}\",");
                        }
                        else
                        {
                            props.Remove(members[i].Name);
                            script.Append($"a.\"{memberExpression.Member.Name}\" AS \"{members[i].Name}\",");
                        }

                    }

                }
                if (script.Length > 1)
                {
                    foreach (var item in props)
                    {
                        if (!TableInfomation<TEntity>.BlockSelectFields.Contains(item))
                        {
                            script.Append($"a.\"{item}\",");
                        }
                       
                    }
                    script.Length -= 1;
                    JoinExpressionMapping = JoinExpressionMapping.Add(code, script.ToString());

                    var builder = new StringBuilder();
                    foreach (var item in joinTypeFlagMapping)
                    {
                        var joinFieldCache = OrmNavigate<TEntity>.JoinScriptMapping[item.Key];
                        string joinAlias = string.Empty;
                        switch (item.Value)
                        {
                            case JoinTypeFlag.Left:
                                joinAlias = LeftJoin.GetJoinScript(item.Key);
                                builder.Append($"obj.LeftJoin(\"");
                                break;
                            case JoinTypeFlag.Inner:
                                joinAlias = InnerJoin.GetJoinScript(item.Key);
                                builder.Append($"obj.InnerJoin(\"");
                                break;
                            case JoinTypeFlag.Right:
                                joinAlias = RightJoin.GetJoinScript(item.Key);
                                builder.Append($"obj.RightJoin(\"");
                                break;
                            default:
                                break;
                        }
                        var joinFieldScript = $"\"{item.Key.Name}\" AS {joinAlias} ON a.\"{joinFieldCache.src}\" = {joinAlias}.\"{joinFieldCache.dst}\"";
                        builder.Append(joinFieldScript.Replace("\"", "\\\""));
                        builder.AppendLine("\");");
                    }

                    JoinFiller<TEntity, TReturn>.Add(code, NDelegate
                        .DefaultDomain()
                        .Action<ISelect<TEntity>>(builder.ToString()));

                }

            }
            var fieldScript = JoinExpressionMapping[code];
            ////调用 TReturn 的处理函数
            JoinFiller<TEntity, TReturn>.HandlerSelect(code, select);
            //返回执行结果
            return select.ToList<TReturn>(fieldScript);

        }
    }

    public static class JoinOperator<TEntity> where TEntity : class
    {

        public static ImmutableDictionary<long,string> JoinExpressionMapping;
        static JoinOperator()
        {
            JoinExpressionMapping = ImmutableDictionary.Create<long, string>();
        }

        public static IEnumerable<object> ToList<TReturn>(ISelect<TEntity> select, Expression<Func<TEntity, TReturn>> expression)
        {

            var code = expression.GetHashCode();

            //查询表达式树是否为之前处理过的
            if (!JoinExpressionMapping.ContainsKey(code))
            {
                //给匿名类创建一个代理类
                var nclass = NClass.DefaultDomain().Public();
                StringBuilder script = new StringBuilder();
                // 获取构造函数参数
                var arguments = ((NewExpression)expression.Body).Arguments;
                //获取匿名类成员
                var members = ((NewExpression)expression.Body).Members;
                var joinTypeFlagMapping = new Dictionary<Type, JoinTypeFlag>();
                for (int i = 0; i < arguments.Count; i++)
                {
                    // 方法类型参数
                    if (arguments[i].NodeType == ExpressionType.Call)
                    {
                        var methodExpression = (MethodCallExpression)arguments[i];
                        var methodDeclaringType = methodExpression.Method.DeclaringType;
                        if (methodDeclaringType.IsGenericType && methodDeclaringType.GetGenericTypeDefinition().Name.Contains("Join"))
                        {

                            var joinType = methodDeclaringType.GetGenericArguments()[0];
                            if (methodExpression.Arguments[0].NodeType == ExpressionType.Quote)
                            {
                                var quoteExpression = (UnaryExpression)methodExpression.Arguments[0];
                                if (quoteExpression.Operand.NodeType == ExpressionType.Lambda)
                                {
                                    var lambdaExpression = (LambdaExpression)quoteExpression.Operand;
                                    if (lambdaExpression.Body.NodeType == ExpressionType.MemberAccess)
                                    {
                                        var memberExpression = (MemberExpression)lambdaExpression.Body;
                                        nclass.Property(item => item
                                        .Public()
                                        .Type(((PropertyInfo)memberExpression.Member).PropertyType)
                                        .Name(members[i].Name));

                                        var definitionType = methodDeclaringType.GetGenericTypeDefinition();
                                        if (definitionType == typeof(InnerJoin<>))
                                        {
                                            joinTypeFlagMapping[joinType] = JoinTypeFlag.Inner;
                                            script.Append(InnerJoin.GetJoinScript(joinType));
                                        }
                                        else if (definitionType == typeof(LeftJoin<>))
                                        {
                                            joinTypeFlagMapping[joinType] = JoinTypeFlag.Left;
                                            script.Append(LeftJoin.GetJoinScript(joinType));
                                        }
                                        else if (definitionType == typeof(RightJoin<>))
                                        {
                                            joinTypeFlagMapping[joinType] = JoinTypeFlag.Right;
                                            script.Append(RightJoin.GetJoinScript(joinType));
                                        }
                                        script.Append($".\"{memberExpression.Member.Name}\" AS \"{members[i].Name}\",");
                                    }

                                }
                            }
                        }

                    }
                    else if (arguments[i].NodeType == ExpressionType.MemberAccess)
                    {
                       
                        var memberExpression = (MemberExpression)arguments[i];
                        if (members[i].Name == memberExpression.Member.Name)
                        {
                            nclass.Property(item => item
                            .Public()
                            .Type(((PropertyInfo)memberExpression.Member).PropertyType)
                            .Name(memberExpression.Member.Name));
                            script.Append($"a.\"{memberExpression.Member.Name}\",");
                        }
                        else
                        {
                            nclass.Property(item => item
                            .Public()
                            .Type(((PropertyInfo)memberExpression.Member).PropertyType)
                            .Name(members[i].Name));
                            script.Append($"a.\"{memberExpression.Member.Name}\" AS \"{members[i].Name}\",");
                        }

                    }
                    //JoinObjectCache<ISelect<T>, TReturn>.GetObjects = NDelegate.RandomDomain().Func<ISelect<T>, object>(builder.ToString());

                }
                if (script.Length > 1)
                {

                    script.Length -= 1;
                    var joinScript = script.ToString();
                    JoinExpressionMapping = JoinExpressionMapping.Add(code, joinScript);

                    var proxyClass = nclass.GetType();
                    //返回强类型的集合结果
                    ProxyCaller<TEntity, TReturn>.Add(code, NDelegate
                        .DefaultDomain(item => item.LogSyntaxError())
                        .Func<ISelect<TEntity>, IEnumerable<object>>($"return arg.ToList<{proxyClass.GetDevelopName()}>(\"{joinScript.Replace("\"","\\\"")}\");"));

                    var builder = new StringBuilder();
                    foreach (var item in joinTypeFlagMapping)
                    {
                        var joinFieldCache = OrmNavigate<TEntity>.JoinScriptMapping[item.Key];
                        string joinAlias = string.Empty;
                        switch (item.Value)
                        {
                            case JoinTypeFlag.Left:
                                joinAlias = LeftJoin.GetJoinScript(item.Key);
                                builder.Append($"obj.LeftJoin(\"");
                                break;
                            case JoinTypeFlag.Inner:
                                joinAlias = InnerJoin.GetJoinScript(item.Key);
                                builder.Append($"obj.InnerJoin(\"");
                                break;
                            case JoinTypeFlag.Right:
                                joinAlias = RightJoin.GetJoinScript(item.Key);
                                builder.Append($"obj.RightJoin(\"");
                                break;
                            default:
                                break;
                        }
                        var joinFieldScript = $"\"{item.Key.Name}\" AS {joinAlias} ON a.\"{joinFieldCache.src}\" = {joinAlias}.\"{joinFieldCache.dst}\"";
                        builder.Append(joinFieldScript.Replace("\"", "\\\""));
                        builder.AppendLine("\");");
                    }

                    
                    JoinFiller<TEntity, TReturn>.Add(code,NDelegate
                        .DefaultDomain()
                        .Action<ISelect<TEntity>>(builder.ToString()));

                }

            }
            //调用 TReturn 的处理函数
            JoinFiller<TEntity, TReturn>.HandlerSelect(code, select);
            //返回执行结果
            //return select.ToList<TReturn>(JoinExpressionMapping[code]);
            return ProxyCaller<TEntity, TReturn>.ToList(code, select);

        }
        
    }

    /// <summary>
    /// 存放与返回值相关的外联查询处理函数
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public static class JoinFiller<TEntity, TReturn> where TEntity : class
    {
        private static ImmutableDictionary<long, Action<ISelect<TEntity>>> JoinActionMapping;
        static JoinFiller()
        {
            JoinActionMapping = ImmutableDictionary.Create<long, Action<ISelect<TEntity>>>();
        }
        public static void HandlerSelect(long code, ISelect<TEntity> select)
        {
            JoinActionMapping[code](select);
        }

        public static void Add(long code, Action<ISelect<TEntity>> func)
        {
            JoinActionMapping = JoinActionMapping.Add(code, func);
        }

    }


    /// <summary>
    /// 存放与 TReturn 相关的动态执行委托
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public static class ProxyCaller<TEntity, TReturn> where TEntity : class
    {
        private static ImmutableDictionary<long, Func<ISelect<TEntity>, IEnumerable<object>>> ProxyActionMapping;
        static ProxyCaller()
        {
            ProxyActionMapping = ImmutableDictionary.Create<long, Func<ISelect<TEntity>, IEnumerable<object>>>();
        }
        public static IEnumerable<object> ToList(long code, ISelect<TEntity> select)
        {
            return ProxyActionMapping[code](select);
        }

        public static void Add(long code , Func<ISelect<TEntity>, IEnumerable<object>> func)
        {
            ProxyActionMapping = ProxyActionMapping.Add(code, func);
        }
    }


}
