using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSql.Natasha.Extension
{
    public static class HttpContextQueryOperator<TEntity> where TEntity : class
    {
        public static readonly Action<ISelect<TEntity>, IEnumerable<string>, TEntity> SelectWhereHandler;
        public static readonly Action<IUpdate<TEntity>, IEnumerable<string>, TEntity> UpdateWhereHandler;
        public static readonly Action<IDelete<TEntity>, IEnumerable<string>, TEntity> DeleteWhereHandler;
        static HttpContextQueryOperator()
        {

            var stringBuilder = new StringBuilder();
            var props = typeof(TEntity).GetProperties().Select(item=>item.Name);
            stringBuilder.AppendLine("foreach(var field in arg2){");
            foreach (var item in props)
            {

                if (!PropertiesCache<TEntity>.BlockWhereFields.Contains(item))
                {
                    stringBuilder.AppendLine($"if(field == \"{item}\"){{  arg1.Where(obj=>obj.{item}==arg3.{item});  }}");
                    stringBuilder.Append("else ");
                }
                
            }
            stringBuilder.Length -= 5;
            stringBuilder.Append("}");
            var result = stringBuilder.ToString();
            DeleteWhereHandler += NDelegate
   .DefaultDomain()
   .Action<IDelete<TEntity>, IEnumerable<string>, TEntity>(result);

            UpdateWhereHandler += NDelegate
    .DefaultDomain()
    .Action<IUpdate<TEntity>, IEnumerable<string>, TEntity>(result);

            SelectWhereHandler += NDelegate
    .DefaultDomain()
    .Action<ISelect<TEntity>, IEnumerable<string>, TEntity>(result);


        }
    }
}
