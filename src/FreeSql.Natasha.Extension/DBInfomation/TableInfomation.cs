using FreeSql;
using FreeSql.Natasha.Extension;
using Natasha.CSharp;
using System;


public static class TableInfomation
{
    static TableInfomation()
    {
        NatashaInitializer.InitializeAndPreheating();
    }

    public static void Initialize(IFreeSql freeSql, params Type[] types)
    {

        var domain = DomainManagement.Random;
        foreach (var item in types)
        {
           NDelegate
            .UseDomain(domain)
            .Action<IFreeSql>($"TableInfomation<{item.GetDevelopName()}>.Initialize(obj);")(freeSql);
        }
        domain.Dispose();

    }


}

public static class TableInfomation<TEntity> where TEntity : class
{

    public static string PrimaryKey;
    public static Func<TEntity, long> GetPrimaryKey;
    public static Action<TEntity, long> SetPrimaryKey;
    public static Func<IBaseRepository<TEntity>, TEntity, ISelect<TEntity>> FillPrimary;

    public static void Initialize(IFreeSql freeSql)
    {
        var tables = freeSql.DbFirst.GetTablesByDatabase();
        foreach (var item in tables)
        {

            if (item.Name == typeof(TEntity).Name)
            {

                foreach (var column in item.Columns)
                {

                    if (column.IsPrimary)
                    {

                        PrimaryKey = column.Name;
                        FillPrimary = NDelegate.DefaultDomain().Func<IBaseRepository<TEntity>, TEntity, ISelect<TEntity>>($"return arg1.Where(item=>item.{PrimaryKey} == arg2.{PrimaryKey});");
                        SetPrimaryKey = NDelegate.DefaultDomain().Action<TEntity, long>($"arg1.{PrimaryKey} = arg2;");
                        GetPrimaryKey = NDelegate.DefaultDomain().Func<TEntity, long>($"return arg.{PrimaryKey};");
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

                if (item.ForeignsDict != null)
                {
                    foreach (var foreigns in item.ForeignsDict)
                    {
                        NDelegate
                            .RandomDomain()
                            .Action($"OrmNavigate<{typeof(TEntity).Name}>.Join<{foreigns.Value.ReferencedTable.Name}>(\"{foreigns.Value.Columns[0].Name}\",\"{foreigns.Value.ReferencedColumns[0].Name}\");")();
                    }
                }
                break;
            }
        }

    }

}



