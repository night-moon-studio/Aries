using FreeSql.DataAnnotations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

public static class TableInfomation
{

    internal static readonly ConcurrentDictionary<Type, string> _realTableNameMapping;
    static TableInfomation()
    {
        _realTableNameMapping = new ConcurrentDictionary<Type, string>();
    }

    internal static bool _useNewIdentity;
    public static void UseNewPgsqlIdentity()
    {
        _useNewIdentity = true;
    }

    /// <summary>
    /// 对类型进行初始化配置
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="types"></param>
    public static void InitializeTypes(IFreeSql freeSql, params Type[] types)
    {

        foreach (var item in types)
        {
            if (item.IsClass)
            {

                var callerType = typeof(TableInfomation<>).MakeGenericType(item);
                var initializeMethod = callerType.GetMethod("Initialize", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                DynamicMethod method = new DynamicMethod(Guid.NewGuid().ToString(), null, new Type[] { typeof(IFreeSql) });
                ILGenerator il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, initializeMethod);
                il.Emit(OpCodes.Ret);
                ((Action<IFreeSql>)method.CreateDelegate(typeof(Action<IFreeSql>)))(freeSql);

            }

        }

    }
    /// <summary>
    /// 获取真实表名
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetRealTableName(Type type)
    {
        return _realTableNameMapping[type];
    }
    /// <summary>
    /// 对程序集进行初始化配置
    /// </summary>
    /// <param name="freeSql"></param>
    /// <param name="assembly"></param>
    public static void InitializeAssembly(IFreeSql freeSql, Assembly assembly)
    {

        var types = assembly.GetTypes();
        InitializeTypes(freeSql, types);

    }
    /// <summary>
    /// 根据程序集中的类所实现的接口来进行初始化配置
    /// </summary>
    /// <typeparam name="Interface"></typeparam>
    /// <param name="freeSql"></param>
    /// <param name="assembly"></param>
    public static void InitializeAssembly<Interface>(IFreeSql freeSql, Assembly assembly)
    {

        var types = assembly.GetTypes();
        for (int i = 0; i < types.Length; i++)
        {               
            try
            {
                if (typeof(Interface).IsAssignableFrom(types[i]))
                {
                    InitializeTypes(freeSql, types[i]);
                }

            }
            catch (Exception ex)
            {

            }

        }
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

        TableInfomation._realTableNameMapping[typeof(TEntity)] = TableName;
        var table = freeSql.DbFirst.GetTableByName(TableName);
        if (table == null)
        {

            //同步结构
            PropertyInfo primaryInfo = default;
            AriesPrimaryKeyAttribute primaryKey = type.GetCustomAttribute<AriesPrimaryKeyAttribute>();
            if (primaryKey == null)
            {
                primaryInfo = type.GetProperty("Id") ?? type.GetProperty("id") ?? type.GetProperty("_id");
                if (primaryInfo != null && (primaryInfo.PropertyType == typeof(long) || primaryInfo.PropertyType == typeof(int)))
                {
                    freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(primaryInfo.Name).IsPrimary(true).IsIdentity(true));
                }
            }
            else
            {

                if (primaryKey.KeyName != null)
                {
                    freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(primaryKey.KeyName).IsPrimary(true).IsIdentity(true));
                }

            }
            var descInfo = type.GetProperty("Description") ?? type.GetProperty("Desc") ?? type.GetProperty("desc");
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
            if (primaryInfo != default && (primaryInfo.PropertyType == typeof(long) || primaryInfo.PropertyType == typeof(int)) && TableInfomation._useNewIdentity)
            {
                CreateNewPrimaryKey(freeSql, TableName);
            }

        }
        if (table != null)
        {
            foreach (var column in table.Columns)
            {

                if (column.IsPrimary)
                {

                    PrimaryKey = column.Name;
                    PrimaryKeyIsLong = type.GetProperty(PrimaryKey).PropertyType == typeof(long);
                    freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(column.Name).IsPrimary(true).IsIdentity(true));

                }
                else if (column.CsType == typeof(string))
                {

                    freeSql.CodeFirst.ConfigEntity<TEntity>(config => config.Property(column.Name).StringLength(column.MaxLength));

                }

            }
        }

    }

    /// <summary>
    /// 设置 PGSQL 表自增主键约束
    /// </summary>
    /// <param name="sqlHandler"></param>
    /// <param name="tableName"></param>
    public static void CreateNewPrimaryKey(IFreeSql sqlHandler, string tableName)
    {
        var result1 = sqlHandler.Ado.ExecuteNonQuery($@"
ALTER TABLE public.""{tableName}"" DROP COLUMN ""Id"";

ALTER TABLE public.""{tableName}""ADD COLUMN ""Id"" 
bigint NOT NULL GENERATED ALWAYS AS IDENTITY
(INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 );

ALTER TABLE public.""{tableName}"" ADD CONSTRAINT ""{tableName}_zz_pkey"" PRIMARY KEY(""Id""); ");
    }

}



