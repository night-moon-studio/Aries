using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Aries
{

    
    public static class JoinExtension
    {

        public static OutEntity AriesInnerJoin<OutEntity>(this object field, Expression<Func<OutEntity, object>> expression)
        {
            return default;
        }
        public static OutEntity AriesLeftJoin<OutEntity>(this object field, Expression<Func<OutEntity, object>> expression)
        {
            return default;
        }
        public static OutEntity AriesRightJoin<OutEntity>(this object field, Expression<Func<OutEntity, object>> expression)
        {
            return default;
        }


        /// <summary>
        /// 返回带有关系指定的匿名类的Join集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="select">查询操作实例</param>
        /// <param name="expression">匿名类</param>
        /// <returns></returns>
        public static IEnumerable<TReturn> ToJoinList<TEntity, TReturn>(this ISelect<TEntity> select, Expression<Func<TEntity, TReturn>> expression) where TEntity : class
        {
            return JoinOperator<TEntity>.ToList(select, expression);
        }


    }

}
