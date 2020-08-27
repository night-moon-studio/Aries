using System.Collections.Generic;

namespace FreeSql.Natasha.Extension
{
    public static class UpdateExtension
    {

        public static IUpdate<TEntity> UpdateWithHttpModel<TEntity>(this IFreeSql freeSql, ICollection<string> collection, TEntity entity) where TEntity : class
        {
            var update = freeSql.Update<TEntity>();
            HttpContextUpdateOperator<TEntity>.UpdateWhereHandler(update, collection, entity);
            return update;
        }


        /// <summary>
        /// 更新全部
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="update"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IUpdate<TEntity> UpdateAll<TEntity>(this IFreeSql freeSql, TEntity entity) where TEntity : class
        {
            var update = freeSql.Update<TEntity>();
            if (TableInfomation<TEntity>.PrimaryKey == default)
            {
                update.SetDto(entity);
            }
            else
            {
                update.SetSource(entity);
            }
            return update;
        }


        /// <summary>
        /// 增量更新
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool UpdateIncrement<TEntity>(this IFreeSql freeSql, TEntity entity) where TEntity : class
        {
            var repo = freeSql.GetRepository<TEntity>();
            TableInfomation<TEntity>.FillPrimary(repo,entity).First();
            return repo.Update(entity) != 0;

        }
    }
}
