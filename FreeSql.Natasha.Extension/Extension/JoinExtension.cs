using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FreeSql.Natasha.Extension
{
    public static class JoinExtension
    {
        public static IEnumerable<object> ToJoinList<TEntity, TReturn>(this ISelect<TEntity> select, Expression<Func<TEntity, TReturn>> expression) where TEntity : class
        {
            return JoinOperator<TEntity>.ToList(select, expression);
        }
        public static IEnumerable<TReturn> ToJoinList<TEntity, TReturn>(this ISelect<TEntity> select, object expression, TReturn instance = default) where TEntity : class
        {
            return JoinOperator<TEntity, TReturn>.ToList(select, expression);
        }
    }
}
