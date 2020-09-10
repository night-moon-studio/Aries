using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FreeSql.Natasha.Extension
{
    public static class JoinExtension
    {
        /// <summary>
        /// 返回带有关系指定的匿名类的Join集合
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="select">查询操作实例</param>
        /// <param name="expression">匿名类</param>
        /// <returns></returns>
        public static IEnumerable<object> ToJoinList<TEntity, TReturn>(this ISelect<TEntity> select, Expression<Func<TEntity, TReturn>> expression) where TEntity : class
        {
            return JoinOperator<TEntity>.ToList(select, expression);
        }


        /// <summary>
        /// 实用强类型映射作为外联查询结果，不使用该方法将返回 object 集合
        /// 会受 PropertiesCache<TEntity>.BlockSelectFields 影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="select">查询操作实例</param>
        /// <returns></returns>
        public static ForwardJoin<TEntity, TReturn> UseMappingClass<TEntity, TReturn>(this ISelect<TEntity> select) where TEntity : class
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

        /// <summary>
        /// 在使用 UseMappingClass 方法之后，便可以使用该方法，返回强类型集合
        /// 会受 PropertiesCache<TEntity>.BlockSelectFields 影响
        /// </summary>
        /// <typeparam name="TempReturn"></typeparam>
        /// <param name="expression">匿名类</param>
        /// <returns></returns>
        public IEnumerable<TReturn> ToJoinList<TempReturn>(Expression<Func<TEntity, TempReturn>> expression)
        {
            return JoinOperator<TEntity, TReturn>.ToList(_select,expression);
        }

    }

}
