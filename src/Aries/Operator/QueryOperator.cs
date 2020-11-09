using FreeSql;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace Aries
{

    public static class QueryOperator<TEntity, TQueryModel> where TEntity : class where TQueryModel : QueryModel, new()
    {

        public static readonly Action<ISelect<TEntity>, TQueryModel> SelectWhereHandler;

        static QueryOperator()
        {

            if (typeof(QueryModel).IsAssignableFrom(typeof(TQueryModel)))
            {

                Action<ISelect<TEntity>, TQueryModel> selectAction = (select, queryModel) =>
                {

                    if (queryModel.Orders != null)
                    {

                        var orderBuilder = new StringBuilder(queryModel.Orders.Length * 8);
                        var blockWhereList = PropertiesCache<TEntity>.GetBlockWhereFields();
                        foreach (var item in queryModel.Orders)
                        {

                            if (!blockWhereList.Contains(item.FieldName) && PropertiesCache<TEntity>.PropMembers.Contains(item.FieldName))
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
