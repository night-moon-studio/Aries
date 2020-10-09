using FreeSql;
using System.Collections.Generic;

namespace Aries
{
    public static class UpdateExtension
    {

        /// <summary>
        /// 按需更新，不自带条件，需要补充查询条件
        /// 会受到 PropertiesCache<TEntity>.AllowUpdateColumns
        /// 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql">freesql实例</param>
        /// <param name="collection">需要更新的字段</param>
        /// <param name="entity">更新的实体</param>
        /// <returns></returns>
        public static IUpdate<TEntity> UpdateWithEntity<TEntity>(this IFreeSql freeSql, IEnumerable<string> collection, TEntity entity) where TEntity : class
        {
            var update = freeSql.Update<TEntity>();
            HttpContextUpdateOperator<TEntity>.UpdateFieldsHandler(update, PropertiesCache<TEntity>.GetUpdateFields(collection), entity);
            return update;
        }


        /// <summary>
        /// 更新全部
        /// 会受到 PropertiesCache<TEntity>.AllowUpdateColumns
        /// 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql">freesql实例</param>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static IUpdate<TEntity> UpdateAll<TEntity>(this IFreeSql freeSql, TEntity entity) where TEntity : class
        {
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

    }
}
