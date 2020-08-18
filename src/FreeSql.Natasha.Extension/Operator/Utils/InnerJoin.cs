using System;
using System.Linq.Expressions;



public static class InnerJoin
{
    public static string GetJoinScript(Type type)
    {
        return type.Name + "_INNER_JOIN";
    }
}

public static class InnerJoin<S>
{

    public static readonly string JoinAliasName;
    static InnerJoin()
    {
        JoinAliasName = InnerJoin.GetJoinScript(typeof(S));
    }


    public static TReturn MapFrom<TReturn>(Expression<Func<S, TReturn>> expression)
    {
        return default(TReturn);
    }

}
