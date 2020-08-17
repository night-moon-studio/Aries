using FreeSql.Natasha.Extension;
using Natasha.CSharp;
using System;


public static class TableInfomationInitor
{
    static TableInfomationInitor()
    {
        NatashaInitializer.InitializeAndPreheating();
    }

    public static void Initialize(IFreeSql freeSql, params Type[] types)
    {

        foreach (var item in types)
        {
            NDelegate
            .DefaultDomain()
            .Action<IFreeSql>($"TableInfomationInitor<{item.GetDevelopName()}>.Initialize(obj);")(freeSql);
        }


    }


}

public static class TableInfomationInitor<TEntity>
{

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
                        TableInfomation<TEntity>.PrimaryKey = column.Name;
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

public class TableInfomation<TEntity>
{
    public static string PrimaryKey;
}


