using System.Collections.Generic;

namespace FreeSql.Natasha.Extension
{
    public static class QueryExtension
    {

        public static ISelect<TEntity> GetInitedSelect<TEntity>(this IFreeSql freeSql) where TEntity : class
        {
            var select = freeSql.Select<TEntity>();
            PropertiesCache<TEntity>.SelectInitFunc?.Invoke(select);
            return select;
        }



        public static ISelect<TEntity> QueryWithHttpEntity<TEntity>(this ISelect<TEntity> select, IEnumerable<string> collection, TEntity entity) where TEntity : class
        {
            HttpContextQueryOperator<TEntity>.SelectWhereHandler(select, PropertiesCache<TEntity>.GetWhereFields(collection), entity);
            return select;
        }
        public static IUpdate<TEntity> QueryWithHttpEntity<TEntity>(this IUpdate<TEntity> update, IEnumerable<string> collection, TEntity entity) where TEntity : class
        {
            HttpContextQueryOperator<TEntity>.UpdateWhereHandler(update, PropertiesCache<TEntity>.GetWhereFields(collection), entity);
            return update;
        }
        public static IDelete<TEntity> QueryWithHttpEntity<TEntity>(this IDelete<TEntity> delete, IEnumerable<string> collection, TEntity entity) where TEntity : class
        {
            HttpContextQueryOperator<TEntity>.DeleteWhereHandler(delete, PropertiesCache<TEntity>.GetWhereFields(collection), entity);
            return delete;
        }




        public static ISelect<TEntity> QueryWithModel<TEntity, TQueryModel>(this ISelect<TEntity> select, TQueryModel queryModel) where TEntity : class where TQueryModel : QueryModel, new()
        {
            QueryOperator<TEntity, TQueryModel>.SelectWhereHandler(select, queryModel);
            if (queryModel.Fuzzy != null)
            {
                foreach (var model in queryModel.Fuzzy)
                {
                    select.FuzzyQuery(model);
                }
            }
            return select;
        }
        public static ISelect<TEntity> QueryWithModel<TEntity,TQueryModel>(this ISelect<TEntity> select, TQueryModel queryModel,out long total) where TEntity : class where TQueryModel : QueryModel, new()
        {
            QueryOperator<TEntity, TQueryModel>.SelectWhereHandler(select, queryModel);
            if (queryModel.Fuzzy!=null)
            {
                foreach (var model in queryModel.Fuzzy)
                {
                    select.FuzzyQuery(model);
                }
            }
            select.Count(out total);
            return select;
        }
        public static IUpdate<TEntity> QueryWithModel<TEntity, TQueryModel>(this IUpdate<TEntity> update, TQueryModel queryModel) where TEntity : class where TQueryModel : QueryModel, new()
        {
            QueryOperator<TEntity, TQueryModel>.UpdateWhereHandler(update, queryModel);
            return update;
        }
        public static IDelete<TEntity> QueryWithModel<TEntity, TQueryModel>(this IDelete<TEntity> delete, TQueryModel queryModel) where TEntity : class where TQueryModel : QueryModel, new()
        {
            QueryOperator<TEntity, TQueryModel>.DeleteWhereHandler(delete, queryModel);
            return delete;
        }

    }
}
