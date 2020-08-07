using System;
using System.Collections.Generic;
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


    public static class QueryModelAnalysis<TEntity,TQueryModel> where TEntity : class
    { 

        public static readonly Action<ISelect<TEntity>, TQueryModel> SelectWhereHandler;
        public static readonly Action<IUpdate<TEntity>, TQueryModel> UpdateWhereHandler;
        public static readonly Action<IInsert<TEntity>, TQueryModel> InsertWhereHandler;
        public static readonly Action<IDelete<TEntity>, TQueryModel> IDeleteWhereHandler;
        static QueryModelAnalysis()
        {
            var props = typeof(TQueryModel).GetProperties().Where(item=>item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>));


        }

    }


    public class OrderModel
    {

        public string FieldName { get; set; }

        public bool IsDesc { get; set; }

    }


    public class PageModel 
    {

        public int? Page { get; set; }

        public int? Size { get; set; }

        public bool? Total { get; set; }

    }


    public class PageOrderModel : PageModel
    { 

        public OrderModel[] Orders { get; set; }

    }


}
