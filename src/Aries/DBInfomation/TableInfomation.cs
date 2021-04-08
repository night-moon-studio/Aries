using FreeSql;
using Aries;
using Natasha.CSharp;
using System;
using System.Reflection;
using FreeSql.DataAnnotations;
using System.Collections.Concurrent;

public static class TableInfomation
{

    private static readonly ConcurrentDictionary<Type, string> _realTableNameMapping;
    static TableInfomation()
    {
        NatashaInitializer.InitializeAndPreheating();
        _realTableNameMapping = new ConcurrentDictionary<Type, string>();
    }

    public static void Initialize(IFreeSql freeSql, params Type[] types)
    {

        var domain = DomainManagement.Random;
        foreach (var item in types)
        {
            if (item.IsClass)
            {
                _realTableNameMapping[item] = NDelegate
                   .UseDomain(domain)
                   .Func<IFreeSql, string>($"TableInfomation<{item.GetDevelopName()}>.Initialize(arg);return TableInfomation<{item.GetDevelopName()}>.TableName;")(freeSql);

            }

        }
        domain.Dispose();

    }

    public static string GetRealTableName(Type type)
    {
        return _realTableNameMapping[type];
    }


}

public static class TableInfomation<TEntity> where TEntity : class
{

    public static string PrimaryKey;
    public static string TableName;
    public static bool PrimaryKeyIsLong;
    public static Func<TEntity, long> GetPrimaryKey;
    public static Action<TEntity, long> SetPrimaryKey;


    public static void Initialize(IFreeSql freeSql)
    {
        var type = typeof(TEntity);
        var tableAttr = type.GetCustomAttribute<TableAttribute>();
        if (tableAttr != default)
        {
            TableName = tableAttr.Name;
        }
        else
        {
            TableName = type.Name;
        }

        var table = freeSql.DbFirst.GetTableByName(TableName);
        if (table == null)
        {

            //同步结构
            AriesPrimaryKeyAttribute primaryKey = type.GetCustomAttribute<AriesPrimaryKeyAttribute>();
            if (primaryKey == null)
            {
                PropertyInfo memberInfo = type.GetProperty("Id") ?? type.GetProperty("id") ?? type.GetProperty("_id");
                if (memberInfo != null && memberInfo.PropertyType == typeof(long))
                {
                    freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(memberInfo.Name).IsPrimary(true).IsIdentity(true));
                }
            }
            else
            {

                if (primaryKey.KeyName != null)
                {
                    freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(primaryKey.KeyName).IsPrimary(true).IsIdentity(true));
                }

            }
            var  descInfo = type.GetProperty("Description") ?? type.GetProperty("Desc") ?? type.GetProperty("desc");
            if (descInfo != null && descInfo.PropertyType == typeof(string))
            {
                freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(descInfo.Name).StringLength(-1));
            }
            var contentInfo = type.GetProperty("Content") ?? type.GetProperty("content");
            if (contentInfo != null && contentInfo.PropertyType == typeof(string))
            {
                freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(contentInfo.Name).StringLength(-1));
            }

            freeSql.CodeFirst.SyncStructure(type, TableName);

        }
        if (table != null)
        {
            foreach (var column in table.Columns)
            {

                if (column.IsPrimary)
                {

                    PrimaryKey = column.Name;
                    PrimaryKeyIsLong = type.GetProperty(PrimaryKey).PropertyType == typeof(long);

                    if (PrimaryKeyIsLong)
                    {
                        SetPrimaryKey = NDelegate.DefaultDomain().Action<TEntity, long>($"arg1.{PrimaryKey} = arg2;");
                        GetPrimaryKey = NDelegate.DefaultDomain().Func<TEntity, long>($"return arg.{PrimaryKey};");
                    }

                    freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(column.Name).IsPrimary(true).IsIdentity(true));

                }
                else if (column.CsType == typeof(string))
                {

                    if (PropertiesCache<TEntity>.PropMembers.Contains(column.Name))
                    {
                        freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(column.Name).StringLength(column.MaxLength));
                    }

                }

            }
        }

    }

}



