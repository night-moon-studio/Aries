using System.Linq;

namespace Aries
{

    public static class InsertExtension
    {
        /// <summary>
        /// 初始化实体并插入,返回：如果带有主键，成功则返回带主键的实体，不带主键，成功则返回原实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="freeSql">freesql 句柄</param>
        /// <param name="entity">要被插入的实体类</param>
        /// <returns></returns>
        public static bool AriesInsert<TEntity>(this IFreeSql freeSql,params TEntity[] entity) where TEntity : class
        {

            var insert = freeSql.Insert(entity).IgnoreColumns(PropertiesCache<TEntity>.GetBlockInsertFields().ToArray());
            if (TableInfomation<TEntity>.PrimaryKey!=default)
            {

                var id = insert.ExecuteIdentity();
                TableInfomation<TEntity>.SetPrimaryKey(entity[entity.Length-1], id);
                return id != 0;

            }
            else
            {

                return insert.ExecuteAffrows() == entity.Length;
               
            }

        }

    }

}
