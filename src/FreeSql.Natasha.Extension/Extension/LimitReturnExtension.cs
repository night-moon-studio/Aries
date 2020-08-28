using System.Collections.Generic;

namespace FreeSql.Natasha.Extension
{
    public static class LimitReturnExtension
    {
        public static IEnumerable<TEntity> ToLimitList<TEntity>(this ISelect<TEntity> select) where TEntity : class
        {
            return select.ToList<TEntity>(LimitReturnOperator<TEntity>.ReturnScript);
        }
    }
}
