using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSql.PgSql.Natasha.Extension.DBInfomation
{
    public static class TableInfomationInitor 
    {
        private static IFreeSql _freeSql;

        public static void AddFreeSqlInstance(IFreeSql freeSql)
        {
            _freeSql = freeSql;
        }

        public static void Init<TEntity>()
        {
            var tables = _freeSql.DbFirst.GetTablesByDatabase();
            foreach (var item in tables)
            {

                if (item.Name == typeof(TEntity).Name)
                {

                    foreach (var column in item.Columns)
                    {

                        if (column.IsPrimary)
                        {
                            TableInfomation<TEntity>.PrimaryKey = column.Name;
                            _freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(column.Name).IsIdentity(true));
                        }
                        else if (column.CsType == typeof(string))
                        {
                            _freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(column.Name).StringLength(column.MaxLength));
                        }

                    }

                    break;
                }
            }
        }
    }

    public class TableInfomation<TEntity>
    {
        public static string PrimaryKey;
    }

}
