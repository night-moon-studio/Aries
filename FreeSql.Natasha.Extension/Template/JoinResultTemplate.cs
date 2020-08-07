using Natasha.CSharp;
using Natasha.Reverser;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace FreeSql.Natasha.Extension
{
    public class JoinTemplate<TEntity> : PrimaryTemplate<TEntity> where TEntity : class
    {
        public static ImmutableHashSet<long> JoinExpressionMapping;
        static JoinTemplate()
        {
            JoinExpressionMapping = ImmutableHashSet.Create<long>();
            var props = typeof(TEntity).GetProperties().Select(item => item.Name);
            var builder = new StringBuilder();
            foreach (var item in props)
            {
                builder.Append($"a.\"{item}\",");
            }
            if (builder.Length > 0)
            {
                builder.Length -= 1;
            }

        }
        public JoinTemplate(IFreeSql freeSql) : base(freeSql)
        {

        }
        protected ISelect<TEntity> SelectHandler;
        public IEnumerable<object> ToList<TReturn>(Expression<Func<TEntity, TReturn>> expression)
        {

            var code = expression.GetHashCode();
            if (SelectHandler == null)
            {
                SelectHandler = SqlHandler.Select<TEntity>();
            }
            var type = typeof(TReturn);
            if (!JoinExpressionMapping.Contains(code))
            {
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
                                        .Name(memberExpression.Member.Name));

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
                        nclass.Property(item => item
                        .Public()
                        .Type(((PropertyInfo)memberExpression.Member).PropertyType)
                        .Name(memberExpression.Member.Name));
                        script.Append($"a.\"{memberExpression.Member.Name}\",");
                    }
                    //JoinObjectCache<ISelect<T>, TReturn>.GetObjects = NDelegate.RandomDomain().Func<ISelect<T>, object>(builder.ToString());

                }
                if (script.Length > 1)
                {

                    script.Length -= 1;
                    var joinScript = script.ToString();
                    JoinExpressionMapping = JoinExpressionMapping.Add(code);

                    var tempClass = nclass.GetType();
                    ProxyCaller<TEntity, TReturn>.Add(code, NDelegate
                        .DefaultDomain(item => item.LogSyntaxError())
                        .Func<ISelect<TEntity>, IEnumerable<object>>($"return arg.ToList<{tempClass.GetDevelopName()}>(\"{joinScript.Replace("\"","\\\"")}\");"));

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

                    //$"\"{typeof(TJoinEntity).Name}\" AS {InnerJoinHelper<TJoinEntity>.JoinAliasName} ON a.\"{srcFieldName}\" = {InnerJoinHelper<TJoinEntity>.JoinAliasName}.\"{destFieldName}\"")
                    JoinFiller<TEntity, TReturn>.Add(code,NDelegate
                        .DefaultDomain()
                        .Action<ISelect<TEntity>>(builder.ToString()));

                }

            }

            JoinFiller<TEntity, TReturn>.HandlerSelect(code,SelectHandler);
            return ProxyCaller<TEntity, TReturn>.ToList(code, SelectHandler);

        }


    }
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


    public class Test2 
    {
        public long Id { get; set; }
        public short DomainFlags { get; set; }
        public string DomainName { get; set; }
    }

}
