using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public class SqlModel<T> where T : class
{
    public QueryModel QueryModel { get; set; }
    public QueryInstanceModel<T> QueryInstance { get; set; }
    public OperatorModel<T> ModifyInstance { get; set; }

    private readonly List<Expression<Func<T, bool>>> _whereList;

    public SqlModel()
    {
        _whereList = new List<Expression<Func<T, bool>>>();
    }


    public List<Expression<Func<T, bool>>> GetWhereExpressions()
    {
        return _whereList;
    }

    /// <summary>
    /// 增加需要更新的字段
    /// </summary>
    /// <param name="field"></param>
    public void AddModifyField(string field)
    {
        ModifyInstance.Fields.Add(field);
    }

    public void AddModifyField<TReturn>(Expression<Func<T,TReturn>> expModify)
    {

        var exp = expModify.Body;
        if (exp.NodeType == ExpressionType.MemberAccess)
        {
            var memberExp = (MemberExpression)exp;
            AddModifyField(memberExp.Member.Name);
        }

    }
    /// <summary>
    /// 增加要查询的字段
    /// </summary>
    /// <param name="field"></param>
    public void AddQueryCondition(Expression<Func<T, bool>> expQuery)
    {
        _whereList.Add(expQuery);
        //if (expQuery.Body is BinaryExpression binExp)
        //{
        //    if (binExp.Left.NodeType == ExpressionType.MemberAccess)
        //    {
        //        QueryInstance.Fields.Add(((MemberExpression)binExp.Left).Member.Name);
        //    }
        //    else if (binExp.Right.NodeType == ExpressionType.MemberAccess)
        //    {
        //        QueryInstance.Fields.Add(((MemberExpression)binExp.Right).Member.Name);
        //    }
        //}

    }



}