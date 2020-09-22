using FreeSql;
using System;
using System.Text;

namespace Aries
{

    public static class QueryOperator<TEntity, TQueryModel> where TEntity : class where TQueryModel : QueryModel, new()
    {
        
        public static readonly Action<ISelect<TEntity>, TQueryModel> SelectWhereHandler;
        public static readonly Action<IUpdate<TEntity>, TQueryModel> UpdateWhereHandler;
        public static readonly Action<IDelete<TEntity>, TQueryModel> DeleteWhereHandler;
        static QueryOperator()
        {

   //         var stringBuilder = new StringBuilder();
   //         var props = typeof(TQueryModel).GetProperties();

            
   //         foreach (var item in props)
   //         {

   //             if (PropertiesCache<TEntity>.PropMembers.Contains(item.Name))
   //             {

   //                 if (!PropertiesCache<TEntity>.BlockWhereFields.Contains(item.Name))
   //                 {
   //                     if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
   //                     {
   //                         stringBuilder.AppendLine($"if(arg2.{item.Name}!=null){{");
   //                         stringBuilder.AppendLine($"arg1.Where(obj=>obj.{item.Name}==arg2.{item.Name});");
   //                         stringBuilder.AppendLine("}");
   //                     }
   //                     else if (item.PropertyType == typeof(string))
   //                     {
   //                         stringBuilder.AppendLine($"if(arg2.{item.Name}!=default){{");
   //                         stringBuilder.AppendLine($"arg1.Where(obj=>obj.{item.Name}.Contains(arg2.{item.Name}));");
   //                         stringBuilder.AppendLine("}");
   //                     }
   //                 }
                    
   //             }
                
   //         }
   //         var result = stringBuilder.ToString();

   //         DeleteWhereHandler += NDelegate
   //.DefaultDomain()
   //.Action<IDelete<TEntity>, TQueryModel>(result);

   //         UpdateWhereHandler += NDelegate
   // .DefaultDomain()
   // .Action<IUpdate<TEntity>, TQueryModel>(result);

   //         SelectWhereHandler += NDelegate
   // .DefaultDomain()
   // .Action<ISelect<TEntity>, TQueryModel>(result);


            if (typeof(QueryModel).IsAssignableFrom(typeof(TQueryModel)))
            {

                Action<ISelect<TEntity>, TQueryModel> selectAction = (select, queryModel) =>
                {

                    if (queryModel.Orders != null)
                    {

                        if (queryModel.Size != 0)
                        {
                            select.Page(queryModel.Page, queryModel.Size);
                        }
                        var orderBuilder = new StringBuilder(queryModel.Orders.Length * 8);
                        var blockWhereList = PropertiesCache<TEntity>.GetBlockWhereFields();
                        foreach (var item in queryModel.Orders)
                        {

                            if (!blockWhereList.Contains(item.FieldName))
                            {

                                if (PropertiesCache<TEntity>.PropMembers.Contains(item.FieldName))
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

                            }
                            
                        }
                        if (orderBuilder.Length > 0)
                        {

                            orderBuilder.Length -= 1;
                            select.OrderBy(orderBuilder.ToString());

                        }
                    }
                };
                SelectWhereHandler += selectAction;

            }



        }



    }

}
