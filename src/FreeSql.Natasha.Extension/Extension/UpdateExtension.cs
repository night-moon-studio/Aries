namespace FreeSql.PgSql.Natasha.Extension.Extension
{
    public static class UpdateExtension
    {

        /// <summary>
        /// 更新全部
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="update"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IUpdate<TEntity> UpdateAll<TEntity>(this IUpdate<TEntity> update,TEntity entity) where TEntity : class
        {
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
