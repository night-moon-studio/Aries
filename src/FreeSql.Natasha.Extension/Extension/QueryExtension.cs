using System.Collections.Generic;

namespace FreeSql.Natasha.Extension
{
    public static class QueryExtension
    {

        /// <summary>
        /// 获取初始化后的查询句柄
        /// 会受到  PropertiesCache<TEntity>.SelectInitFunc 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql"></param>
        /// <returns></returns>
        public static ISelect<TEntity> GetInitedSelect<TEntity>(this IFreeSql freeSql) where TEntity : class
        {
            var select = freeSql.Select<TEntity>();
            PropertiesCache<TEntity>.SelectInitFunc?.Invoke(select);
            return select;
        }


        /// <summary>
        /// 根据集合指定的字段，和实体中的信息进行参数化查询处理
        /// 会受到 PropertiesCache<TEntity>.BlockWhereFields 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="select">操作句柄</param>
        /// <param name="collection">指定要查询的集合</param>
        /// <param name="entity">携带信息的实体</param>
        /// <returns></returns>
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


        /// <summary>
        /// 针对ISelect的查询模型
        /// 会受到 PropertiesCache<TEntity>.BlockWhereFields 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TQueryModel"></typeparam>
        /// <param name="select">操作句柄</param>
        /// <param name="queryModel">查询模型</param>
        /// <returns></returns>
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

        /// <summary>
        /// 针对IUpdate的查询模型
        /// 会受到 PropertiesCache<TEntity>.BlockWhereFields 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TQueryModel"></typeparam>
        /// <param name="update">操作句柄</param>
        /// <param name="queryModel">查询模型</param>
        /// <returns></returns>
        public static IUpdate<TEntity> QueryWithModel<TEntity, TQueryModel>(this IUpdate<TEntity> update, TQueryModel queryModel) where TEntity : class where TQueryModel : QueryModel, new()
        {
            QueryOperator<TEntity, TQueryModel>.UpdateWhereHandler(update, queryModel);
            return update;
        }
        /// <summary>
        /// 针对IDelete的查询模型
        /// 会受到 PropertiesCache<TEntity>.BlockWhereFields 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TQueryModel"></typeparam>
        /// <param name="delete">操作句柄</param>
        /// <param name="queryModel">查询模型</param>
        public static IDelete<TEntity> QueryWithModel<TEntity, TQueryModel>(this IDelete<TEntity> delete, TQueryModel queryModel) where TEntity : class where TQueryModel : QueryModel, new()
        {
            QueryOperator<TEntity, TQueryModel>.DeleteWhereHandler(delete, queryModel);
            return delete;
        }

    }
}
