using FreeSql.PgSql.Natasha.Extension.DBInfomation;
using FreeSql.PgSql.Natasha.Extension.Join.Utils;
using System;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Text;

namespace FreeSql.PgSql.Natasha.Extension
{
    public static class Navigate<TEntity>
    {
        public static readonly ImmutableDictionary<Type, string> JoinScriptMapping;
        static Navigate()
        {
            JoinScriptMapping = ImmutableDictionary.Create<Type, string>();
        }


        #region 链接查询抽象
        static void Join<TJoinEntity>(JoinTypeFlag flag, string srcFieldName, string destFieldName = default)
        {

            if (destFieldName == default)
            {
                destFieldName = TableInfomation<TJoinEntity>.PrimaryKey;
            }
            var builder = new StringBuilder();
            switch (flag)
            {
                case JoinTypeFlag.Left:
                    builder.AppendLine($"LEFT JOIN ");
                    break;
                case JoinTypeFlag.Inner:
                    builder.AppendLine($"INNER JOIN ");
                    break;
                case JoinTypeFlag.Right:
                    builder.AppendLine($"RIGHT JOIN ");
                    break;
                default:
                    break;
            }
            builder.AppendLine($"{JoinHelper<TJoinEntity>.JoinAliasName} ON {srcFieldName} = {JoinHelper<TJoinEntity>.JoinAliasName}.{destFieldName}");
            JoinScriptMapping.Add(typeof(TJoinEntity), builder.ToString());
        }
        #endregion

        #region 左连接查询
        public static void LefJoin<TJoinEntity>(Expression<Func<TEntity, object>> expression, string fieldName = default)
        {
            Join<TJoinEntity>(JoinTypeFlag.Left, ((MemberExpression)expression.Body).Member.Name, fieldName);
        }


        public static void LefJoin<TJoinEntity>(string srcFieldName, string destFieldName = default)
        {
            Join<TJoinEntity>(JoinTypeFlag.Left, srcFieldName, destFieldName);
        }
        #endregion

        #region 右连接查询
        public static void RightJoin<TJoinEntity>(Expression<Func<TEntity, object>> expression, string fieldName = default)
        {
            Join<TJoinEntity>(JoinTypeFlag.Right, ((MemberExpression)expression.Body).Member.Name, fieldName);
        }


        public static void RightJoin<TJoinEntity>(string srcFieldName, string destFieldName = default)
        {
            Join<TJoinEntity>(JoinTypeFlag.Right, srcFieldName, destFieldName);
        }
        #endregion

        #region 内连接查询
        public static void InnerJoin<TJoinEntity>(Expression<Func<TEntity, object>> expression, string fieldName = default)
        {
            Join<TJoinEntity>(JoinTypeFlag.Inner, ((MemberExpression)expression.Body).Member.Name, fieldName);
        }


        public static void InnerJoin<TJoinEntity>(string srcFieldName, string destFieldName = default)
        {
            Join<TJoinEntity>(JoinTypeFlag.Inner, srcFieldName, destFieldName);
        }
        #endregion

    }

    public enum JoinTypeFlag
    {
        Left,
        Inner,
        Right
    }
}
