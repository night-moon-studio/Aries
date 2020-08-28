using System;
using System.Collections.Immutable;
using System.Linq.Expressions;


public static class OrmNavigate<TEntity> where TEntity : class
{
    public static ImmutableDictionary<Type, (string src, string dst, string table)> JoinScriptMapping;
    static OrmNavigate()
    {
        JoinScriptMapping = ImmutableDictionary.Create<Type, (string src, string dst, string table)>();
    }


    #region 链接查询抽象
    public static void Connect<TJoinEntity>(string srcFieldName, string destFieldName = default) where TJoinEntity : class
    {

        if (destFieldName == default)
        {
            destFieldName = TableInfomation<TJoinEntity>.PrimaryKey;
        }
        JoinScriptMapping = JoinScriptMapping.Add(typeof(TJoinEntity), (srcFieldName,destFieldName,typeof(TJoinEntity).Name));

    }


    public static void Connect<TJoinEntity>(Expression<Func<TEntity, object>> expression, string dstFieldName = default) where TJoinEntity : class
    {
        Connect<TJoinEntity>(GetNameFromExpression(expression), dstFieldName);
    }


    public static void Connect<TJoinEntity>(Expression<Func<TEntity, object>> srcExpression, Expression<Func<TJoinEntity, object>> dstExpression) where TJoinEntity : class
    {
        Connect<TJoinEntity>(
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

