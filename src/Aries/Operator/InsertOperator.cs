using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aries
{
    public static class InsertOperator<TEntity> where TEntity : class
    {
        public static Func<IFreeSql,TEntity[], bool> Insert;
        static InsertOperator()
        {
            
            if (TableInfomation<TEntity>.PrimaryKey != default && TableInfomation<TEntity>.PrimaryKeyIsLong)
            {

                   Insert = NDelegate
                    .UseDomain(typeof(TEntity).GetDomain())
                    .Func<IFreeSql, TEntity[], bool>($@"
                        var insert = arg1.Insert(arg2).IgnoreColumns(PropertiesCache<{typeof(TEntity).GetDevelopName()}>.BlockInsertCoulmns);
                        var id = insert.ExecuteIdentity();
                        arg2[arg2.Length - 1].{TableInfomation<TEntity>.PrimaryKey} = id;
                        return id != 0;
                    ");


            }
            else
            {

                Insert = NDelegate
                   .UseDomain(typeof(TEntity).GetDomain())
                   .Func<IFreeSql, TEntity[], bool>($@"
                        var insert = arg1.Insert(arg2).IgnoreColumns(PropertiesCache<{typeof(TEntity).GetDevelopName()}>.BlockInsertCoulmns);
                        return insert.ExecuteAffrows() == arg2.Length;
                    ");

            }
        }
    }
}
