using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSql.Natasha.Extension
{

    public static class InsertExtension
    {

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
