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
                   .Func<IFreeSql,string>($"TableInfomation<{item.GetDevelopName()}>.Initialize(arg);return TableInfomation<{item.GetDevelopName()}>.TableName;")(freeSql);
               
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

        var tableAttr = typeof(TEntity).GetCustomAttribute<TableAttribute>();
        if (tableAttr!=default)
        {
            TableName = tableAttr.Name;
        }
        else
        {
            TableName = typeof(TEntity).Name;
        }  

        var table = freeSql.DbFirst.GetTableByName(TableName);
        if (table != null)
        {
            foreach (var column in table.Columns)
            {

                if (column.IsPrimary)
                {

                    PrimaryKey = column.Name;
                    PrimaryKeyIsLong = typeof(TEntity).GetProperty(PrimaryKey).PropertyType == typeof(long);  

                    if (PrimaryKeyIsLong)
                    {
                        SetPrimaryKey = NDelegate.DefaultDomain().Action<TEntity, long>($"arg1.{PrimaryKey} = arg2;");
                        GetPrimaryKey = NDelegate.DefaultDomain().Func<TEntity, long>($"return arg.{PrimaryKey};");
                    }

                    freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(column.Name).IsIdentity(true));

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



