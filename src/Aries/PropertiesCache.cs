using FreeSql;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Aries
{

    public static class PropertiesCache<TEntity> where TEntity :class
    {

        public static readonly ImmutableHashSet<string> PropMembers;
        public static ImmutableHashSet<string> BlockWhereFields;
        public static ImmutableHashSet<string> BlockSelectFields;
        public static ImmutableHashSet<string> AllowUpdateFields;
        public static ImmutableHashSet<string> BlockInsertFields;
        public static string[] AllowUpdateColumns;
        public static Action<TEntity> UpdateInitFunc;
        public static Action<TEntity> InsertInitFunc;
        public static Action<ISelect<TEntity>> SelectInitFunc;

        static PropertiesCache()
        {
            PropMembers = ImmutableHashSet.CreateRange(typeof(TEntity).GetProperties().Select(item => item.Name));
            var props = PropMembers;
            if (TableInfomation<TEntity>.PrimaryKey != default)
	        {
                props = PropMembers.Remove(TableInfomation<TEntity>.PrimaryKey);
                BlockInsertFields = ImmutableHashSet.Create(TableInfomation<TEntity>.PrimaryKey);
            }
            else
            {
                BlockInsertFields = ImmutableHashSet.Create<string>();
            }

            BlockWhereFields = ImmutableHashSet.Create<string>();
            AllowUpdateFields = ImmutableHashSet.CreateRange(props);
            BlockSelectFields = ImmutableHashSet.Create<string>();
        }


        /// <summary>
        /// 设置查询字段黑名单，在黑名单中的字段可能不会参与结果返回
        /// </summary>
        /// <param name="fields"></param>
        public static void SetSelectBlockFields(params string[] fields)
        {
            BlockSelectFields = ImmutableHashSet.CreateRange(fields);
            ResetSelectScript();
        }


        /// <summary>
        /// 设置where查询字段黑名单，在黑名单中的字段不会作为条件进行查询
        /// </summary>
        /// <param name="fields"></param>
        public static void SetWhereBlockFields(params string[] fields)
        {
            BlockWhereFields = ImmutableHashSet.CreateRange(fields);
        }
        public static void SetWhereBlockAllExcept(params string[] fields)
        {
            var temp = new HashSet<string>(PropMembers);
            temp.ExceptWith(fields);
            BlockWhereFields = ImmutableHashSet.CreateRange(temp);
        }
        /// <summary>
        /// 设置where查询字段黑名单，在黑名单中的字段不会作为条件进行查询
        /// </summary>
        /// <param name="fields"></param>
        public static void SetWhereAllowFields(params string[] fields)
        {

            if (BlockWhereFields.Count>0)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    BlockWhereFields = BlockWhereFields.Remove(fields[i]);
                }
            }   
              
        }


        /// <summary>
        /// 更新字段白名单，在白名单中的字段才允许参与更新操作
        /// 默认所有字段均参与更新
        /// </summary>
        /// <param name="fields"></param>
        public static void SetUpdateAllowFields(params string[] fields)
        {
            AllowUpdateFields = ImmutableHashSet.CreateRange(fields);
            AllowUpdateColumns = AllowUpdateFields.ToArray();
        }


        /// <summary>
        /// 设置更新初始化委托，在实体类更新之前将对实体类进行默认值设置
        /// </summary>
        /// <param name="action"></param>
        public static void SetUpdateInit(Action<TEntity> action)
        {

            UpdateInitFunc += action;

        }


        /// <summary>
        /// 设置插入初始化委托，在实体类插入之前对实体类进行默认值设置
        /// </summary>
        /// <param name="action"></param>
        public static void SetInsertInit(Action<TEntity> action)
        {

            InsertInitFunc += action;

        }


        /// <summary>
        /// 设置选择初始化委托，在查询之前，设置固定的 Where 查询语句
        /// </summary>
        /// <param name="action"></param>
        public static void SetSelectInit(Action<ISelect<TEntity>> action)
        {

            SelectInitFunc += action;

        }


        /// <summary>
        /// 与更新白名单做比较，获取允许被更新的字段
        /// </summary>
        /// <param name="keys">外部需要更新的字段</param>
        /// <returns></returns>
        public static IEnumerable<string> GetUpdateFields(IEnumerable<string> keys)
        {
            var result = new HashSet<string>(keys);
            result.IntersectWith(AllowUpdateFields);
            return result;
        }


        /// <summary>
        /// 与查询条件字段黑名单做比较，获取允许被查询的字段
        /// </summary>
        /// <param name="keys">外部需要查询的字段</param>
        /// <returns></returns>
        public static IEnumerable<string> GetWhereFields(IEnumerable<string> keys)
        {
            var result = new HashSet<string>(keys);
            result.ExceptWith(BlockWhereFields);
            return result;
        }


        /// <summary>
        /// 与查询字段黑名单做比较，获取允许被返回的字段
        /// </summary>
        /// <param name="keys">外部需要返回的字段</param>
        /// <returns></returns>
        public static IEnumerable<string> GetSelectFields(IEnumerable<string> keys)
        {
            var result = new HashSet<string>(keys);
            result.ExceptWith(BlockSelectFields);
            return result;
        }


        /// <summary>
        /// 每次 SetSelectBlockFields 都会触发生成返回脚本
        /// </summary>
        public static void ResetSelectScript()
        {
            StringBuilder script = new StringBuilder();
            foreach (var item in PropMembers)
            {
                if (!BlockSelectFields.Contains(item))
                {
                    script.Append($"a.\"{item}\",");
                }

            }
            if (script.Length>1)
            {
                script.Length -= 1;
            }
            LimitReturnOperator<TEntity>.ReturnScript = script.ToString();
        }

    }
}
