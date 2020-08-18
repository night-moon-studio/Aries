using System;
using System.Linq.Expressions;



public static class LeftJoin
{
    public static string GetJoinScript(Type type)
    {
        return type.Name + "_LEFT_JOIN";
    }
}

public static class LeftJoin<S>
{

    public static readonly string JoinAliasName;
    static LeftJoin()
    {
        JoinAliasName = InnerJoin.GetJoinScript(typeof(S));
    }


    public static TReturn MapFrom<TReturn>(Expression<Func<S, TReturn>> expression)
    {
        return default(TReturn);
    }

}
