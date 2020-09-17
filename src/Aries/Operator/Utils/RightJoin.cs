using System;
using System.Linq.Expressions;



public static class RightJoin
{
    public static string GetJoinScript(Type type)
    {
        return type.Name + "_RIGHT_JOIN";
    }
}

public static class RightJoin<S>
{

    public static readonly string JoinAliasName;
    static RightJoin()
    {
        JoinAliasName = InnerJoin.GetJoinScript(typeof(S));
    }


    public static TReturn MapFrom<TReturn>(Expression<Func<S, TReturn>> expression)
    {
        return default(TReturn);
    }

}
