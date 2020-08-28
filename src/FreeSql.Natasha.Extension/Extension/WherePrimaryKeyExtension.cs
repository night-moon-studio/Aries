namespace FreeSql.Natasha.Extension
{
    public static class WherePrimaryKeyExtension
    {

        public static IUpdate<TEntity> WherePrimaryKeyFromEntity<TEntity>(this IUpdate<TEntity> update, TEntity entity) where TEntity : class
        {
            PrimaryKeyOperator<TEntity>.UpdateWhere(update, entity);
            return update;
        }


        public static ISelect<TEntity> WherePrimaryKeyFromEntity<TEntity>(this ISelect<TEntity> select, TEntity entity) where TEntity : class
        {
            PrimaryKeyOperator<TEntity>.SelectWhere(select, entity);
            return select;
        }


        public static IDelete<TEntity> WherePrimaryKeyFromEntity<TEntity>(this IDelete<TEntity> delete, TEntity entity) where TEntity : class
        {
            PrimaryKeyOperator<TEntity>.DeleteWhere(delete, entity);
            return delete;
        }


        public static IUpdate<TEntity> WherePrimaryKey<TEntity, TPrimary>(this IUpdate<TEntity> update, TPrimary primaryKey) where TEntity : class 
        {
            PrimaryKeyOperator<TEntity, TPrimary>.UpdateWhere(update, primaryKey);
            return update;
        }


        public static ISelect<TEntity> WherePrimaryKey<TEntity, TPrimary>(this ISelect<TEntity> select, TPrimary primaryKey) where TEntity : class
        {
            PrimaryKeyOperator<TEntity, TPrimary>.SelectWhere(select, primaryKey);
            return select;
        }


        public static IDelete<TEntity> WherePrimaryKey<TEntity, TPrimary>(this IDelete<TEntity> delete, TPrimary primaryKey) where TEntity : class
        {
            PrimaryKeyOperator<TEntity, TPrimary>.DeleteWhere(delete, primaryKey);
            return delete;
        }

    }

}
