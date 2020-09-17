using FreeSql;

namespace Aries
{
    public static class WherePrimaryKeyExtension
    {

        /// <summary>
        /// 更新操作实例添加主键查询条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="update">更新操作实例</param>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static IUpdate<TEntity> WherePrimaryKeyFromEntity<TEntity>(this IUpdate<TEntity> update, TEntity entity) where TEntity : class
        {
            PrimaryKeyOperator<TEntity>.UpdateWhere(update, entity);
            return update;
        }

        /// <summary>
        /// 查询操作实例添加主键查询条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="select">查询操作实例</param>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static ISelect<TEntity> WherePrimaryKeyFromEntity<TEntity>(this ISelect<TEntity> select, TEntity entity) where TEntity : class
        {
            PrimaryKeyOperator<TEntity>.SelectWhere(select, entity);
            return select;
        }


        /// <summary>
        /// 删除操作实例添加主键查询条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="delete">删除操作实例</param>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static IDelete<TEntity> WherePrimaryKeyFromEntity<TEntity>(this IDelete<TEntity> delete, TEntity entity) where TEntity : class
        {
            PrimaryKeyOperator<TEntity>.DeleteWhere(delete, entity);
            return delete;
        }


        /// <summary>
        /// 更新操作实例添加主键查询条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimary"></typeparam>
        /// <param name="update">更新操作实例</param>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public static IUpdate<TEntity> WherePrimaryKey<TEntity, TPrimary>(this IUpdate<TEntity> update, TPrimary primaryKey) where TEntity : class 
        {
            PrimaryKeyOperator<TEntity, TPrimary>.UpdateWhere(update, primaryKey);
            return update;
        }


        /// <summary>
        /// 查询操作实例添加主键查询条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimary"></typeparam>
        /// <param name="select">查询操作实例</param>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public static ISelect<TEntity> WherePrimaryKey<TEntity, TPrimary>(this ISelect<TEntity> select, TPrimary primaryKey) where TEntity : class
        {
            PrimaryKeyOperator<TEntity, TPrimary>.SelectWhere(select, primaryKey);
            return select;
        }


        /// <summary>
        /// 删除操作实例添加主键查询条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimary"></typeparam>
        /// <param name="delete">删除操作实例</param>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public static IDelete<TEntity> WherePrimaryKey<TEntity, TPrimary>(this IDelete<TEntity> delete, TPrimary primaryKey) where TEntity : class
        {
            PrimaryKeyOperator<TEntity, TPrimary>.DeleteWhere(delete, primaryKey);
            return delete;
        }

    }

}
