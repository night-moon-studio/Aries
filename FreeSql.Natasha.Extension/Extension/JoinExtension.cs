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

        public static ForwardJoin<TEntity, TReturn> UseStrongClass<TEntity, TReturn>(this ISelect<TEntity> select) where TEntity : class
        {
            return  new ForwardJoin<TEntity, TReturn>(select);
        }

    }
    public class ForwardJoin<TEntity, TReturn> where TEntity : class
    {
        private readonly ISelect<TEntity> _select;
        public ForwardJoin(ISelect<TEntity> select)
        {
            _select = select;
        }

        public IEnumerable<TReturn> ToJoinList<TempReturn>(Expression<Func<TEntity, TempReturn>> expression)
        {
            return JoinOperator<TEntity, TReturn>.ToList(_select,expression);
        }
    }
}
