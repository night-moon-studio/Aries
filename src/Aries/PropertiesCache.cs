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
        private static ImmutableHashSet<string> _blockWhereFields;
        private static ImmutableHashSet<string> _blockSelectFields;
        private static ImmutableHashSet<string> _allowUpdateFields;
        private static ImmutableHashSet<string> _blockInsertFields;
        public static string[] AllowUpdateColumns;

        static PropertiesCache()
        {
            PropMembers = ImmutableHashSet.CreateRange(typeof(TEntity).GetProperties().Select(item => item.Name));
            var props = PropMembers;
            if (TableInfomation<TEntity>.PrimaryKey != default)
	        {
                props = PropMembers.Remove(TableInfomation<TEntity>.PrimaryKey);
                _blockInsertFields = ImmutableHashSet.Create(TableInfomation<TEntity>.PrimaryKey);
            }
            else
            {
                _blockInsertFields = ImmutableHashSet.Create<string>();
            }

            _blockWhereFields = ImmutableHashSet.Create<string>();
            _allowUpdateFields = ImmutableHashSet.CreateRange(props);
            _blockSelectFields = ImmutableHashSet.Create<string>();
        }

        public static ImmutableHashSet<string> GetBlockWhereFields()
        {
            return _blockWhereFields;
        }
        public static ImmutableHashSet<string> GetBlockSelectFields()
        {
            return _blockSelectFields;
        }
        public static ImmutableHashSet<string> GetBlockInsertFields()
        {
            return _blockInsertFields;
        }
        public static ImmutableHashSet<string> GetAllowUpdateFields()
        {
            return _allowUpdateFields;
        }

        #region 黑名单操作
        public static void BlockAllUpdateFields()
        {
            _allowUpdateFields = ImmutableHashSet.Create<string>();
            AllowUpdateColumns = _allowUpdateFields.ToArray();
        }
        public static void BlockAllWhereFields()
        {
            _blockWhereFields = ImmutableHashSet.CreateRange(PropMembers);
        }
        public static void BlockAllSelectFields()
        {
            _blockSelectFields = ImmutableHashSet.CreateRange(PropMembers);
        }
        public static void BlockAllInsertFields()
        {
            _blockInsertFields = ImmutableHashSet.CreateRange(PropMembers);
        }
        #endregion

        #region 白名单操作
        public static void AllowAllUpdateFields()
        {
            _allowUpdateFields = ImmutableHashSet.CreateRange(PropMembers);
            AllowUpdateColumns = _allowUpdateFields.ToArray();
        }
        public static void AllowAllWhereFields()
        {
            _blockWhereFields = ImmutableHashSet.Create<string>();
        }
        public static void AllowAllSelectFields()
        {
            _blockSelectFields = ImmutableHashSet.Create<string>();
        }
        public static void AllowAllInsertFields()
        {
            _blockInsertFields = ImmutableHashSet.Create<string>();
        }
        #endregion


        #region 增加白名单
        public static void AllowUpdateFields(params string[] fields)
        {
            _allowUpdateFields = _allowUpdateFields.Union(fields);
        }
        public static void AllowWhereFields(params string[] fields)
        {
            _blockWhereFields = _blockWhereFields.Except(fields);
        }
        public static void AllowSelectFields(params string[] fields)
        {
            _blockSelectFields = _blockSelectFields.Except(fields);
        }
        public static void AllowInsertFields(params string[] fields)
        {
            _blockInsertFields = _blockInsertFields.Except(fields);
        }
        #endregion

        #region 增加黑名单
        public static void BlockUpdateFields(params string[] fields)
        {
            _allowUpdateFields = _allowUpdateFields.Except(fields);
        }
        public static void BlockWhereFields(params string[] fields)
        {
            _blockWhereFields = _blockWhereFields.Union(fields);
        }
        public static void BlockSelectFields(params string[] fields)
        {
            _blockSelectFields = _blockSelectFields.Union(fields);
        }
        public static void BlockInsertFields(params string[] fields)
        {
            _blockInsertFields = _blockInsertFields.Union(fields);
        }
        #endregion
        
        /// <summary>
        /// 与更新白名单做比较，获取允许被更新的字段
        /// </summary>
        /// <param name="keys">外部需要更新的字段</param>
        /// <returns></returns>
        public static IEnumerable<string> GetUpdateFields(IEnumerable<string> keys)
        {
            var result = new HashSet<string>(keys);
            result.IntersectWith(_allowUpdateFields);
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
            result.ExceptWith(_blockWhereFields);
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
            result.ExceptWith(_blockSelectFields);
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
                if (!_blockSelectFields.Contains(item))
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
