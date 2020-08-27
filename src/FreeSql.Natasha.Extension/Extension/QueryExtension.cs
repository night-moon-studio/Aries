using System.Collections.Generic;

namespace FreeSql.Natasha.Extension
{
    public static class QueryExtension
    {

        public static ISelect<TEntity> QueryWithHttpEntity<TEntity>(this ISelect<TEntity> select, ICollection<string> collection, TEntity entity) where TEntity : class
        {
            HttpContextQueryOperator<TEntity>.SelectWhereHandler(select, collection, entity);
            return select;
        }
        public static IUpdate<TEntity> QueryWithHttpEntity<TEntity>(this IUpdate<TEntity> update, ICollection<string> collection, TEntity entity) where TEntity : class
        {
            HttpContextQueryOperator<TEntity>.UpdateWhereHandler(update, collection, entity);
            return update;
        }
        public static IDelete<TEntity> QueryWithHttpEntity<TEntity>(this IDelete<TEntity> delete, ICollection<string> collection, TEntity entity) where TEntity : class
        {
            HttpContextQueryOperator<TEntity>.DeleteWhereHandler(delete, collection, entity);
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
