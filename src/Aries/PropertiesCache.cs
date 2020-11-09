using FreeSql;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aries
{

    public static class PropertiesCache<TEntity> where TEntity :class
    {

        public static readonly HashSet<string> PropMembers;
        private readonly static HashSet<string> _blockWhereFields;
        private readonly static HashSet<string> _blockSelectFields;
        private readonly static HashSet<string> _allowUpdateFields;
        private readonly static HashSet<string> _blockInsertFields;
        public static string[] AllowUpdateColumns;

        static PropertiesCache()
        {

            PropMembers = new HashSet<string>(typeof(TEntity).GetProperties().Select(item => item.Name));
            _blockInsertFields = new HashSet<string>();
            if (TableInfomation<TEntity>.PrimaryKey != default)
	        {
                PropMembers.Remove(TableInfomation<TEntity>.PrimaryKey);
                _blockInsertFields.Add(TableInfomation<TEntity>.PrimaryKey);
            }

            _blockWhereFields = new HashSet<string>();
            _allowUpdateFields = new HashSet<string>(PropMembers);
            _blockSelectFields = new HashSet<string>();
            AllowUpdateColumns = _allowUpdateFields.ToArray();
        }

        public static HashSet<string> GetBlockWhereFields()
        {
            return _blockWhereFields;
        }
        public static HashSet<string> GetBlockSelectFields()
        {
            return _blockSelectFields;
        }
        public static HashSet<string> GetBlockInsertFields()
        {
            return _blockInsertFields;
        }
        public static HashSet<string> GetAllowUpdateFields()
        {
            return _allowUpdateFields;
        }

        #region 黑名单操作
        /// <summary>
        /// 屏蔽所有字段，不让其进行更新操作
        /// </summary>
        public static void BlockAllUpdateFields()
        {
            _allowUpdateFields.Clear();
            AllowUpdateColumns = _allowUpdateFields.ToArray();
        }
        /// <summary>
        /// 屏蔽所有字段，不让其参与WHERE操作
        /// </summary>
        public static void BlockAllWhereFields()
        {
            _blockWhereFields.UnionWith(PropMembers);
        }
        /// <summary>
        /// 屏蔽所有字段，不让其参与SELECT返回操作
        /// </summary>
        public static void BlockAllSelectFields()
        {
            _blockSelectFields.UnionWith(PropMembers);
        }
        /// <summary>
        /// 屏蔽所有字段，不让其参与新增操作
        /// </summary>
        public static void BlockAllInsertFields()
        {
            _blockInsertFields.UnionWith(PropMembers);
        }
        #endregion

        #region 白名单操作
        /// <summary>
        /// 允许所有字段参与更新操作
        /// </summary>
        public static void AllowAllUpdateFields()
        {
            _allowUpdateFields.UnionWith(PropMembers);
            AllowUpdateColumns = _allowUpdateFields.ToArray();
        }
        /// <summary>
        /// 允许所有字段参与WHERE查询
        /// </summary>
        public static void AllowAllWhereFields()
        {
            _blockWhereFields.Clear();
        }
        /// <summary>
        /// 允许所有字段在SELECT查询后返回
        /// </summary>
        public static void AllowAllSelectFields()
        {
            _blockSelectFields.Clear();
        }
        /// <summary>
        /// 允许所有字段插入更新
        /// </summary>
        public static void AllowAllInsertFields()
        {
            _blockInsertFields.Clear();
        }
        #endregion


        #region 增加白名单
        /// <summary>
        /// 允许参数中的字段参与更新操作
        /// </summary>
        /// <param name="fields"></param>
        public static void AllowUpdateFields(params string[] fields)
        {
            _allowUpdateFields.UnionWith(fields);
            AllowUpdateColumns = _allowUpdateFields.ToArray();
        }
        /// <summary>
        /// 允许参数中的字段参与WHERE查询操作
        /// </summary>
        /// <param name="fields"></param>
        public static void AllowWhereFields(params string[] fields)
        {
            _blockWhereFields.ExceptWith(fields);
        }
        /// <summary>
        /// 允许参数中的字段在SELECT时返回
        /// </summary>
        /// <param name="fields"></param>
        public static void AllowSelectFields(params string[] fields)
        {
            _blockSelectFields.ExceptWith(fields);
        }
        /// <summary>
        /// 允许参数中的字段参与插入操作
        /// </summary>
        /// <param name="fields"></param>
        public static void AllowInsertFields(params string[] fields)
        {
            _blockInsertFields.ExceptWith(fields);
        }
        #endregion

        #region 增加黑名单
        /// <summary>
        /// 不允许参数中的字段参与更新操作
        /// </summary>
        /// <param name="fields"></param>
        public static void BlockUpdateFields(params string[] fields)
        {
            _allowUpdateFields.ExceptWith(fields);
            AllowUpdateColumns = _allowUpdateFields.ToArray();
        }
        /// <summary>
        /// 不允许参数中的字段参与WHERE查询操作
        /// </summary>
        /// <param name="fields"></param>
        public static void BlockWhereFields(params string[] fields)
        {
            _blockWhereFields.UnionWith(fields);
        }
        /// <summary>
        /// 不允许参数中的字段在SELECT时返回
        /// </summary>
        /// <param name="fields"></param>
        public static void BlockSelectFields(params string[] fields)
        {
            _blockSelectFields.UnionWith(fields);
        }
        /// <summary>
        /// 不允许参数中的字段参与插入操作
        /// </summary>
        /// <param name="fields"></param>
        public static void BlockInsertFields(params string[] fields)
        {
            _blockInsertFields.UnionWith(fields);
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
