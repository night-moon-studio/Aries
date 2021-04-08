using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Aries
{
    public static class QueryExtension
    {

        /// <summary>
        /// 通过主键获取实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static TEntity GetByPrimaryKey<TEntity>(this IFreeSql freeSql, TEntity entity) where TEntity : class
        {
            return freeSql.Select<TEntity>().WhereByPrimaryKeyFromEntity(entity).First();
        }
        public static TEntity GetByPrimaryKey<TEntity, TPrimaryKey>(this IFreeSql freeSql, TPrimaryKey key) where TEntity : class
        {
            return freeSql.Select<TEntity>().WhereByPrimaryKey(key).First();
        }


        public static Expression<Func<TEntity,bool>> GetWhereExpression<TEntity>(IEnumerable<string> collection, TEntity entity) where TEntity : class
        {
            Expression<Func<TEntity, bool>> exp = default;
            foreach (var item in collection)
            {
                var temp = HttpContextQueryOperator<TEntity>.WhereHandler(item, entity);
                if (temp != default)
                {
                    exp = exp == default? temp : exp.And(temp);
                }
            }
            return exp;
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
        public static ISelect<TEntity> WhereWithEntity<TEntity>(this ISelect<TEntity> select, IEnumerable<string> collection, TEntity entity) where TEntity : class
        {
            return select.Where(GetWhereExpression(collection,entity));
        }
        public static IUpdate<TEntity> WhereWithEntity<TEntity>(this IUpdate<TEntity> update, IEnumerable<string> collection, TEntity entity) where TEntity : class
        {
            return update.Where(GetWhereExpression(collection, entity));
        }
        public static IDelete<TEntity> WhereWithEntity<TEntity>(this IDelete<TEntity> delete, IEnumerable<string> collection, TEntity entity) where TEntity : class
        {
            return delete.Where(GetWhereExpression(collection, entity));
        }


        public static Expression<Func<TEntity, bool>> GetFuzzyExpression<TEntity, TQueryModel>(TQueryModel queryModel) where TEntity : class where TQueryModel : QueryModel, new()
        {

            var models = queryModel.Fuzzy;
            Expression<Func<TEntity, bool>> exp = FuzzyQueryOperator<TEntity>.GetFuzzyExpression(models[0]);
            for (int i = 1; i < queryModel.Fuzzy.Length; i++)
            {

                var temp = FuzzyQueryOperator<TEntity>.GetFuzzyExpression(models[i]);
                if (temp!=default)
                {
                    if (models[i].IsOr)
                    {
                        exp = exp.Or(temp);
                    }
                    else
                    {
                        exp = exp.And(temp);
                    }
                }

            }
            return exp;
        }


        /// <summary>
        /// 针对IUpdate的查询模型
        /// 会受到 PropertiesCache<TEntity>.BlockWhereFields 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TQueryModel"></typeparam>
        /// <param name="select">操作句柄</param>
        /// <param name="queryModel">查询模型</param>
        /// <returns></returns>
        public static ISelect<TEntity> WhereWithModel<TEntity, TQueryModel>(this ISelect<TEntity> select, TQueryModel queryModel) where TEntity : class where TQueryModel : QueryModel, new()
        {
            QueryOperator<TEntity, TQueryModel>.SelectWhereHandler(select, queryModel);
            if (queryModel.Fuzzy != null && queryModel.Fuzzy.Length != 0)
            {
                return select.Where(GetFuzzyExpression<TEntity, TQueryModel>(queryModel));
            }
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
        public static IUpdate<TEntity> WhereWithModel<TEntity, TQueryModel>(this IUpdate<TEntity> update, TQueryModel queryModel) where TEntity : class where TQueryModel : QueryModel, new()
        {
            if (queryModel.Fuzzy != null && queryModel.Fuzzy.Length != 0)
            {
                return update.Where(GetFuzzyExpression<TEntity, TQueryModel>(queryModel));
            }
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
        public static IDelete<TEntity> WhereWithModel<TEntity, TQueryModel>(this IDelete<TEntity> delete, TQueryModel queryModel) where TEntity : class where TQueryModel : QueryModel, new()
        {
            //QueryOperator<TEntity, TQueryModel>.DeleteWhereHandler(delete, queryModel);
            if (queryModel.Fuzzy != null && queryModel.Fuzzy.Length != 0)
            {
                return delete.Where(GetFuzzyExpression<TEntity, TQueryModel>(queryModel));
            }
            return delete;
        }

    }
}
