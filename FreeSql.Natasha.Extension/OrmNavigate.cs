using System;
using System.Collections.Immutable;
using System.Linq.Expressions;


public static class OrmNavigate<TEntity>
{
    public static ImmutableDictionary<Type, (string src, string dst, string table)> JoinScriptMapping;
    static OrmNavigate()
    {
        JoinScriptMapping = ImmutableDictionary.Create<Type, (string src, string dst, string table)>();
    }


    #region 链接查询抽象
    public static void Join<TJoinEntity>(string srcFieldName, string destFieldName = default)
    {

        if (destFieldName == default)
        {
            destFieldName = TableInfomation<TJoinEntity>.PrimaryKey;
        }
        JoinScriptMapping = JoinScriptMapping.Add(typeof(TJoinEntity), (srcFieldName,destFieldName,typeof(TJoinEntity).Name));
        //$"\"{typeof(TJoinEntity).Name}\" AS {InnerJoinHelper<TJoinEntity>.JoinAliasName} ON a.\"{srcFieldName}\" = {InnerJoinHelper<TJoinEntity>.JoinAliasName}.\"{destFieldName}\"")

    }


    public static void Join<TJoinEntity>(Expression<Func<TEntity, object>> expression, string dstFieldName = default)
    {
        Join<TJoinEntity>(GetNameFromExpression(expression), dstFieldName);
    }


    public static void Join<TJoinEntity>(Expression<Func<TEntity, object>> srcExpression, Expression<Func<TJoinEntity, object>> dstExpression)
    {
        Join<TJoinEntity>(
            GetNameFromExpression(srcExpression), 
            GetNameFromExpression(dstExpression)
            );
    }
    #endregion


    public static string GetNameFromExpression<T>(Expression<Func<T, object>> expression)
    {
        if (expression.Body.NodeType != ExpressionType.MemberAccess)
        {
            var temp = (UnaryExpression)(expression.Body);
            if (temp.Operand.NodeType == ExpressionType.MemberAccess)
            {
                return ((MemberExpression)(temp.Operand)).Member.Name;
            }
        }
        else
        {
            return ((MemberExpression)(expression.Body)).Member.Name;
        }
        throw new Exception("未能解析表达式，请提交BUG!");
    }
}

public enum JoinTypeFlag
{
    Left,
    Inner,
    Right
}

