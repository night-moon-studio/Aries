using FreeSql.PgSql.Natasha.Extension.Join.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FreeSql.Natasha.Extension
{
    public class JoinResultTemplate<TEntity>
    {
        public static ImmutableDictionary<Type, string> SelectFieldsMappping;
        static JoinResultTemplate()
        {
            SelectFieldsMappping = ImmutableDictionary.Create<Type, string>();
            var props = typeof(TEntity).GetProperties().Select(item => item.Name);
            var builder = new StringBuilder();
            foreach (var item in props)
            {
                builder.Append($"a.{item},");
            }
            if (builder.Length>0)
            {
                builder.Length -= 1;
            }
        }
        public void Test()
        {
            
        }


        public IEnumerable<TReturn> ToList<TReturn>(Expression<Func<TEntity, TReturn>> expression) where TReturn : TEntity
        {


            if (!SelectFieldsMappping.ContainsKey(typeof(TReturn)))
            {

                StringBuilder script = new StringBuilder();
                // 获取构造函数参数
                var arguments = ((NewExpression)expression.Body).Arguments;
                //获取匿名类成员
                var members = ((NewExpression)expression.Body).Members;


                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < arguments.Count; i++)
                {
                    // 方法类型参数
                    if (arguments[i].NodeType == ExpressionType.Call)
                    {
                        var methodExpression = (MethodCallExpression)arguments[i];
                        var type = methodExpression.Method.DeclaringType;
                        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(JoinHelper<>))
                        {
                            var joinType = type.GetGenericArguments()[0];
                            if (methodExpression.Arguments[0].NodeType == ExpressionType.Quote)
                            {
                                var quoteExpression = (UnaryExpression)methodExpression.Arguments[0];
                                if (quoteExpression.Operand.NodeType == ExpressionType.Lambda)
                                {
                                    var lambdaExpression = (LambdaExpression)quoteExpression.Operand;
                                    if (lambdaExpression.Body.NodeType == ExpressionType.MemberAccess)
                                    {
                                        var memberExpression = (MemberExpression)lambdaExpression.Body;
                                        script.Append($"{JoinHelper.GetJoinScript(joinType)}.\"{memberExpression.Member.Name}\" as \"{members[i].Name}\",");
                                    }

                                }
                            }
                        }

                    }
                    //JoinObjectCache<ISelect<T>, TReturn>.GetObjects = NDelegate.RandomDomain().Func<ISelect<T>, object>(builder.ToString());

                }
                if (script.Length > 1)
                {
                    script.Length -= 1;
                    SelectFieldsMappping.Add(typeof(TReturn), script.ToString());
                }

            }
            return default;
        }


    }
}
