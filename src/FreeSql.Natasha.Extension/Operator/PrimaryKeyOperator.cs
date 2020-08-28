using Natasha.CSharp;
using System;

namespace FreeSql.Natasha.Extension
{

    public static class PrimaryKeyOperator<TEntity,TPrimary> where TEntity : class
    {

        public readonly static Action<IUpdate<TEntity>, TPrimary> UpdateWhere;
        public readonly static Action<ISelect<TEntity>, TPrimary> SelectWhere;
        public readonly static Action<IDelete<TEntity>, TPrimary> DeleteWhere;
        public readonly static Func<TEntity,TPrimary> PriamryKeyGetter;
        static PrimaryKeyOperator()
        {
            var primaryKey = TableInfomation<TEntity>.PrimaryKey;
            UpdateWhere = NDelegate
                .DefaultDomain()
                .Action<IUpdate<TEntity>, TPrimary>($"arg1.Where(item=>item.{primaryKey}==arg2);");
            SelectWhere = NDelegate
                .DefaultDomain()
                .Action<ISelect<TEntity>, TPrimary>($"arg1.Where(item=>item.{primaryKey}==arg2);");
            DeleteWhere = NDelegate
                .DefaultDomain()
                .Action<IDelete<TEntity>, TPrimary>($"arg1.Where(item=>item.{primaryKey}==arg2);");
            PriamryKeyGetter = NDelegate
                .DefaultDomain()
                .Func<TEntity, TPrimary>($"return arg.{primaryKey};");
            
        }

    }

    public static class PrimaryKeyOperator<TEntity> where TEntity : class
    {

        public readonly static Action<IUpdate<TEntity>, TEntity> UpdateWhere;
        public readonly static Action<ISelect<TEntity>, TEntity> SelectWhere;
        public readonly static Action<IDelete<TEntity>, TEntity> DeleteWhere;
        static PrimaryKeyOperator()
        {

            var primaryKey = TableInfomation<TEntity>.PrimaryKey;
            var type = typeof(TEntity).GetProperty(primaryKey).PropertyType;
            UpdateWhere = NDelegate
                .DefaultDomain()
                .Action<IUpdate<TEntity>, TEntity>($"var temp = PrimaryKeyOperator<{typeof(TEntity).GetDevelopName()},{type.GetDevelopName()}>.PriamryKeyGetter(arg2); arg1.Where(item=>item.{primaryKey}==temp);");
            SelectWhere = NDelegate
                .DefaultDomain()
                .Action<ISelect<TEntity>, TEntity>($"var temp = PrimaryKeyOperator<{typeof(TEntity).GetDevelopName()},{type.GetDevelopName()}>.PriamryKeyGetter(arg2); arg1.Where(item=>item.{primaryKey}==temp);");
            DeleteWhere = NDelegate
                .DefaultDomain()
                .Action<IDelete<TEntity>, TEntity>($"var temp = PrimaryKeyOperator<{typeof(TEntity).GetDevelopName()},{type.GetDevelopName()}>.PriamryKeyGetter(arg2); arg1.Where(item=>item.{primaryKey}==temp);");

        }

    }

}
