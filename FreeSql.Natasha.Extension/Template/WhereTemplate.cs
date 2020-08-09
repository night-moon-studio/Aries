using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace FreeSql.PgSql.Natasha.Extension.Template
{
    public class WhereTemplate
    {
        public void UseModel<TQueryModel>()
        {

        }
    }


    public static class QueryModelAnalysis<TEntity, TQueryModel> where TEntity : class where TQueryModel : class, new()
    {
        public static readonly ImmutableHashSet<string> PropMembers;
        public static readonly Action<ISelect<TEntity>, TQueryModel> SelectWhereHandler;
        public static readonly Action<IUpdate<TEntity>, TQueryModel> UpdateWhereHandler;
        public static readonly Action<IDelete<TEntity>, TQueryModel> IDeleteWhereHandler;
        static QueryModelAnalysis()
        {

            //IUpdate<TEntity> a;
            //ISelect<TEntity> s;
            //s.Where(item=>item.Contains)
            //s.OrderByDescending
            //a.wh
            var stringBuilder = new StringBuilder();
            var props = typeof(TQueryModel).GetProperties();
            PropMembers = ImmutableHashSet.CreateRange(props.Select(item => item.Name));
            if (typeof(PageOrderModel).IsAssignableFrom(typeof(TQueryModel))) ;
            {
                Action<ISelect<TEntity>, TQueryModel> selectAction = (select, queryModel) =>
                {

                    var model = (PageOrderModel)(object)queryModel;
                    if (model.Orders != null)
                    {

                        if (model.Size!=0)
                        {
                            select.Page(model.Page,model.Size);
                        }
                        var orderBuilder = new StringBuilder(model.Orders.Length * 8);
                        foreach (var item in model.Orders)
                        {

                            if (PropMembers.Contains(item.FieldName))
                            {

                                orderBuilder.Append($"a.\"{item.FieldName}\" ");
                                if (item.IsDesc)
                                {
                                    orderBuilder.Append("DESC,");
                                }
                                else
                                {
                                    orderBuilder.Append("ASC,");
                                }

                            }
                            if (orderBuilder.Length > 0)
                            {

                                orderBuilder.Length -= 1;

                            }
                            select.OrderBy(orderBuilder.ToString());

                        }
                    }
                };
                SelectWhereHandler += selectAction;
            }

            var orderProp = typeof(TQueryModel).GetProperty("Orders");
            foreach (var item in props)
            {

                if (PropMembers.Contains(item.Name))
                {

                    if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        stringBuilder.AppendLine($"if(arg2.{item.Name}!=null){{");
                        stringBuilder.AppendLine($"arg1.Where(obj=>obj.{item.Name}==arg2.{item.Name})");
                        stringBuilder.AppendLine("}");
                    }
                    else if (item.PropertyType == typeof(string)) 
                    {
                        stringBuilder.AppendLine($"if(arg2.{item.Name}!=default){{");
                        stringBuilder.AppendLine($"arg1.Where(obj=>obj.{item.Name}.Contains(arg2.{item.Name}));");
                        stringBuilder.AppendLine("}");
                    }

                }
                
            }

            SelectWhereHandler += NDelegate
                .DefaultDomain()
                .


        }

    }


    public class OrderModel
    {

        public string FieldName { get; set; }

        public bool IsDesc { get; set; }

    }


    public class PageModel
    {

        public int Page { get; set; }

        public int Size { get; set; }

        public bool? Total { get; set; }

    }


    public class PageOrderModel : PageModel
    {

        public OrderModel[] Orders { get; set; }

    }


}
