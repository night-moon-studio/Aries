using FreeSql.Natasha.Extension;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace Project
{

    public class IdQueryModel : QueryModel 
    { 
        public long Id { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            var freesql = new FreeSql.FreeSqlBuilder()
                        .UseConnectionString(FreeSql.DataType.PostgreSQL, "Host=127.0.0.1;Port=5432;Username=postgres;Password=123456; Database=test;Pooling=true;Minimum Pool Size=1")
                        .Build();

            //初始化扫描
            TableInfomation.Initialize(freesql,typeof(Test),typeof(Test2),typeof(Test3));

            //配置 Join 关系
            OrmNavigate<Test>.Join<Test2>(item => item.Domain, item => item.Id);
            OrmNavigate<Test>.Join<Test3>(item => item.Type, item => item.Id);

            //前端准备查询条件
            QueryModel queryModel = new QueryModel();
            queryModel.Size = 2;
            queryModel.Orders = new OrderModel[] { new OrderModel() { FieldName = "Id", IsDesc = true } };
            queryModel.Fuzzy = new FuzzyModel[] { new FuzzyModel { FieldName = "Name", FuzzyValue = "44" } };

            //外联查询
            var result = freesql.Select<Test>().QueryWithModel(queryModel,out long total).ToJoinList(item => new
            {
                item.Id,
                item.Name,
                DomainName = InnerJoin<Test2>.MapFrom(item => item.Name),
                TypeName = InnerJoin<Test3>.MapFrom(item => item.TypeName)
            });
            Console.WriteLine(total);


            var result1 = freesql.Select<Test>().ToJoinList(item => new
            {
                item.Id,
                DomainName = RightJoin<Test2>.MapFrom(item => item.Name),
                TypeName = RightJoin<Test3>.MapFrom(item => item.TypeName)
            });
            Console.ReadKey();
        }

        public static TReturn ToList<TReturn>(Expression<Func<Student, TReturn>> expression)
        {
            return default;
        }

        //public static IEnumerable<TReturn> Test<TReturn>(Expression<Func<Student, TReturn>> expression)
        //{
        //    //if (!SelectFieldsMappping.ContainsKey(typeof(TReturn)))
        //    //{
        //    //Link._joinResultCache[typeof(TReturn)] = new Dictionary<string, string>();
        //    // 获取构造函数参数
        //    var arguments = ((NewExpression)expression.Body).Arguments;
        //    //获取匿名类成员
        //    var members = ((NewExpression)expression.Body).Members;


        //    StringBuilder builder = new StringBuilder();
        //    for (int i = 0; i < arguments.Count; i++)
        //    {
        //        // 方法类型参数
        //        if (arguments[i].NodeType == ExpressionType.Call)
        //        {
        //            var methodExpression = (MethodCallExpression)arguments[i];
        //            var type = methodExpression.Method.DeclaringType;
        //            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(InnerJoinHelper<>))
        //            {
        //                var joinType = type.GetGenericArguments()[0];
        //                if (methodExpression.Arguments[0].NodeType == ExpressionType.Quote)
        //                {
        //                    var quoteExpression = (UnaryExpression)methodExpression.Arguments[0];
        //                    if (quoteExpression.Operand.NodeType == ExpressionType.Lambda)
        //                    {
        //                        var lambdaExpression = (LambdaExpression)quoteExpression.Operand;
        //                        if (lambdaExpression.Body.NodeType == ExpressionType.MemberAccess)
        //                        {
        //                            var memberExpression = (MemberExpression)lambdaExpression.Body;
        //                            var result = $"{JoinHelper.GetJoinScript(joinType)}.\"{memberExpression.Member.Name}\" as \"{members[i].Name}\",";
        //                        }

        //                    }
        //                }
        //            }
                    
        //            //item.Name = Method(xxx);
        //            //获取 Method(xxx); 的结果
        //            //var value = NDelegate.DefaultDomain().Func<string>($"return {((MethodCallExpression)bodys[i]).Method.ReflectedType.GetDevelopName()}.{bodys[i]};")();
        //            ///builder.Append($"{members[i].Name} = \"{value.Replace("\"", "\\\"")}\",");
        //        }

        //    }

        //    //去掉“，”
        //    if (builder.Length > 1)
        //    {
        //        builder.Length -= 1;
        //    }

        //    // 闭合
        //    builder.Insert(0, "return arg.ToList(item=>new {");
        //    builder.Append("});");

        //    //适用静态类缓存起来
        //    //JoinObjectCache<ISelect<T>, TReturn>.GetObjects = NDelegate.RandomDomain().Func<ISelect<T>, object>(builder.ToString());

        //    // }
        //    //TypeName = typeof(TReturn).Name;
        //    return default;
        //}
    }


    public class DomainFlags
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class Test
    {
        public long Id { get; set; }
        public short Domain { get; set; }
        public short Type { get; set; }
        public string Name { get; set; }
    }
    public class Test2
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class Test3
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
    }
    public class Student
    {
        public long Id { get; set; }
        public long Tid { get; set; }
        public long Sid { get; set; }
    }

    public class Teaher
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
