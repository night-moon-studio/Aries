using FreeSql.PgSql.Natasha.Extension.Join.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = "b";
            Test(item => new { 
                Name = item.Id, 
                item.Sid, 
                Age = 10, 
                Test = a, 
                Out = JoinHelper<Student>.Field(item => item.Id) });
            //var a = new JoinOperator1<Student>();
            //a.LefJoin<Teaher>(item => item.Tid, "Id").ContactResult(item => new { Name = item.Name });

            //School 默认使用外联字段 _SchoolJoinId;
            //Navigate<Student>.InnerJoin<School>(stu => stu.SchoolName,"cid");

            ////class 默认使用外联字段 _classJoinId;
            //Navigate<Student>.LeftJoin<Class>(stu => stu.ClassEntity);
            //Navigate.LeftJoin<Student>(stu => stu.ClassEntity.From<Class>("cid"));
            //ClassEntity : public class MyClass{ public string Name{get;set;} public string Committer{get;set;}}

            //var a = ToList(item => new { Name = item.Tid });
        }

        public static TReturn ToList<TReturn>(Expression<Func<Student, TReturn>> expression)
        {
            return default;
        }

        public static IEnumerable<TReturn> Test<TReturn>(Expression<Func<Student, TReturn>> expression)
        {
            //if (!SelectFieldsMappping.ContainsKey(typeof(TReturn)))
            //{
            //Link._joinResultCache[typeof(TReturn)] = new Dictionary<string, string>();
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
                                    var result = $"{JoinHelper.GetJoinScript(joinType)}.\"{memberExpression.Member.Name}\" as \"{members[i].Name}\",";
                                }

                            }
                        }
                    }
                    
                    //item.Name = Method(xxx);
                    //获取 Method(xxx); 的结果
                    //var value = NDelegate.DefaultDomain().Func<string>($"return {((MethodCallExpression)bodys[i]).Method.ReflectedType.GetDevelopName()}.{bodys[i]};")();
                    ///builder.Append($"{members[i].Name} = \"{value.Replace("\"", "\\\"")}\",");
                }

            }

            //去掉“，”
            if (builder.Length > 1)
            {
                builder.Length -= 1;
            }

            // 闭合
            builder.Insert(0, "return arg.ToList(item=>new {");
            builder.Append("});");

            //适用静态类缓存起来
            //JoinObjectCache<ISelect<T>, TReturn>.GetObjects = NDelegate.RandomDomain().Func<ISelect<T>, object>(builder.ToString());

            // }
            //TypeName = typeof(TReturn).Name;
            return default;
        }
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
