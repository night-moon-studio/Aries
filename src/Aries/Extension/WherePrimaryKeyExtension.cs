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
        public static IUpdate<TEntity> WhereByPrimaryKeyFromEntity<TEntity>(this IUpdate<TEntity> update, TEntity entity) where TEntity : class
        {
            PrimaryKeyOperator<TEntity>.UpdateWhere(update, entity);
            return update;
        }
        public static bool AriesUpdateByPrimaryKey<TEntity>(this IFreeSql freeSql, TEntity entity) where TEntity : class
        {

            return freeSql
                .Update<TEntity>()
                .SetSource(entity)
                .WhereByPrimaryKeyFromEntity(entity)
                .ExecuteAffrows() == 1;

        }

        /// <summary>
        /// 查询操作实例添加主键查询条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="select">查询操作实例</param>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static ISelect<TEntity> WhereByPrimaryKeyFromEntity<TEntity>(this ISelect<TEntity> select, TEntity entity) where TEntity : class
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
        public static IDelete<TEntity> WhereByPrimaryKeyFromEntity<TEntity>(this IDelete<TEntity> delete, TEntity entity) where TEntity : class
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
        public static IUpdate<TEntity> WhereByPrimaryKey<TEntity, TPrimary>(this IUpdate<TEntity> update, TPrimary primaryKey) where TEntity : class 
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
        public static ISelect<TEntity> WhereByPrimaryKey<TEntity, TPrimary>(this ISelect<TEntity> select, TPrimary primaryKey) where TEntity : class
        {
            PrimaryKeyOperator<TEntity, TPrimary>.SelectWhere(select, primaryKey);
            return select;
        }
        public static TEntity AriesSelectByPrimaryKey<TEntity>(this IFreeSql freeSql, long id) where TEntity : class
        {

            return freeSql
                .Select<TEntity>()
                .WhereByPrimaryKey(id)
                .First();

        }
        public static TEntity AriesSelectByPrimaryKey<TEntity>(this IFreeSql freeSql, string id) where TEntity : class
        {

            return freeSql
                .Select<TEntity>()
                .WhereByPrimaryKey(id)
                .First();

        }

        /// <summary>
        /// 删除操作实例添加主键查询条件
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimary"></typeparam>
        /// <param name="delete">删除操作实例</param>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        public static IDelete<TEntity> WhereByPrimaryKey<TEntity, TPrimary>(this IDelete<TEntity> delete, TPrimary primaryKey) where TEntity : class
        {
            PrimaryKeyOperator<TEntity, TPrimary>.DeleteWhere(delete, primaryKey);
            return delete;
        }


        public static bool AriesDeleteByPrimaryKey<TEntity>(this IFreeSql freeSql, long id) where TEntity : class
        {

            return freeSql
                .Delete<TEntity>()
                .WhereByPrimaryKey(id)
                .ExecuteAffrows() == 1;

        }
        public static bool AriesDeleteByPrimaryKey<TEntity>(this IFreeSql freeSql, string id) where TEntity : class
        {

            return freeSql
                .Delete<TEntity>()
                .WhereByPrimaryKey(id)
                .ExecuteAffrows() == 1;

        }

    }

}
