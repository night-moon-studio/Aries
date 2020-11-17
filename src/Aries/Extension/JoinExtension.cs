using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Aries
{
    //public static class AriesInnerJoin<SourceEntity, OutEntity>
    //{
    //    public static OutEntity Condition(Expression<Func<SourceEntity, OutEntity, bool>> expression)
    //    {
    //        return default;
    //    }

    //}
    //public static class AriesLeftJoin<SourceEntity, OutEntity>
    //{
    //    public static OutEntity Condition(Expression<Func<SourceEntity, OutEntity, bool>> expression)
    //    {
    //        return default;
    //    }

    //}
    //public static class AriesRightJoin<SourceEntity, OutEntity>
    //{
    //    public static OutEntity Condition(Expression<Func<SourceEntity, OutEntity, bool>> expression)
    //    {
    //        return default;
    //    }

    //}

    public static class JoinExtension
    {
        /// <summary>
        /// 内联查询
        /// </summary>
        /// <typeparam name="OutEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="expression"></param>
        /// <param name="expression2">拼接在 On 后面仅常量表达式</param>
        /// <returns></returns>
        public static OutEntity AriesInnerJoin<OutEntity>(this object field, Expression<Func<OutEntity, object>> expression, Expression<Func<OutEntity, bool>> expression2)
        {
            return default;
        }
        /// <summary>
        /// 左联查询
        /// </summary>
        /// <typeparam name="OutEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="expression"></param>
        /// <param name="expression2">拼接在 On 后面仅常量表达式</param>
        /// <returns></returns>
        public static OutEntity AriesLeftJoin<OutEntity>(this object field, Expression<Func<OutEntity, object>> expression, Expression<Func<OutEntity, bool>> expression2)
        {
            return default;
        }
        /// <summary>
        /// 右联查询
        /// </summary>
        /// <typeparam name="OutEntity"></typeparam>
        /// <param name="field"></param>
        /// <param name="expression"></param>
        /// <param name="expression2">拼接在 On 后面仅常量表达式</param>
        /// <returns></returns>
        public static OutEntity AriesRightJoin<OutEntity>(this object field, Expression<Func<OutEntity, object>> expression, Expression<Func<OutEntity, bool>> expression2)
        {
            return default;
        }
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
        public static IEnumerable<TReturn> ToSimpleJoinList<TEntity, TReturn>(this ISelect<TEntity> select, Expression<Func<TEntity, TReturn>> expression) where TEntity : class
        {
            return SimpleJoinOperator<TEntity>.ToList(select, expression);
        }

        /// <summary>
        /// 返回带有关系指定的匿名类的Join集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="select">查询操作实例</param>
        /// <param name="expression">匿名类</param>
        /// <returns></returns>
        //public static IEnumerable<TReturn> ToJoinList<TEntity, TReturn>(this ISelect<TEntity> select, Expression<Func<TEntity, TReturn>> expression) where TEntity : class
        //{
        //    return JoinOperator<TEntity>.ToList(select, expression);
        //}


    }

}
