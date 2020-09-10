using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSql.Natasha.Extension
{

    public static class InsertExtension
    {

        /// <summary>
        /// 初始化实体并插入
        /// 会受到 PropertiesCache<TEntity>.InsertInitFunc 的影响
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql">freesql 句柄</param>
        /// <param name="entity">要被插入的实体类</param>
        /// <returns></returns>
        public static TEntity InsertWithInited<TEntity>(this IFreeSql freeSql,TEntity entity) where TEntity : class
        {
             
            PropertiesCache<TEntity>.InsertInitFunc?.Invoke(entity);
            if (TableInfomation<TEntity>.PrimaryKey!=default)
            {
                return freeSql.GetRepository<TEntity>().Insert(entity);
            }

            if (freeSql.Insert(entity).ExecuteAffrows() == 1)
            {
                return entity;
            }
            return null;

        }

    }

}
