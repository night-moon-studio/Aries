using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeSql.Natasha.Extension
{
    public static class UpdateExtension
    {

        /// <summary>
        /// 按需更新，不自带条件，需要补充查询条件
        /// 会受到 PropertiesCache<TEntity>.UpdateInitFunc 以及 PropertiesCache<TEntity>.AllowUpdateColumns
        /// 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql">freesql实例</param>
        /// <param name="collection">需要更新的字段</param>
        /// <param name="entity">更新的实体</param>
        /// <returns></returns>
        public static IUpdate<TEntity> UpdateWithHttpModel<TEntity>(this IFreeSql freeSql, IEnumerable<string> collection, TEntity entity) where TEntity : class
        {
            PropertiesCache<TEntity>.UpdateInitFunc?.Invoke(entity);
            var update = freeSql.Update<TEntity>();
            HttpContextUpdateOperator<TEntity>.UpdateFieldsHandler(update, PropertiesCache<TEntity>.GetUpdateFields(collection), entity);
            return update;
        }


        /// <summary>
        /// 更新全部
        /// 会受到 PropertiesCache<TEntity>.UpdateInitFunc 以及 PropertiesCache<TEntity>.AllowUpdateColumns
        /// 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql">freesql实例</param>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static IUpdate<TEntity> UpdateAll<TEntity>(this IFreeSql freeSql, TEntity entity) where TEntity : class
        {
            PropertiesCache<TEntity>.UpdateInitFunc?.Invoke(entity);
            var update = freeSql.Update<TEntity>();
            if (TableInfomation<TEntity>.PrimaryKey == default)
            {
                update.SetDto(entity).UpdateColumns(PropertiesCache<TEntity>.AllowUpdateColumns);
            }
            else
            {
                update.SetSource(entity).UpdateColumns(PropertiesCache<TEntity>.AllowUpdateColumns);
            }
            return update;
        }


        /// <summary>
        /// 增量更新
        /// 会受到 PropertiesCache<TEntity>.UpdateInitFunc 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql">freesql实例</param>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static bool UpdateIncrement<TEntity>(this IFreeSql freeSql, TEntity entity) where TEntity : class
        {
            PropertiesCache<TEntity>.UpdateInitFunc?.Invoke(entity);
            var repo = freeSql.GetRepository<TEntity>();
            //通过仓储和主键查询出一个实体
            TableInfomation<TEntity>.FillPrimary(repo,entity).First();
            return repo.Update(entity) != 0;

        }
    }
}
