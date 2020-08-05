using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace FreeSql.PgSql.Natasha.Extension.Join.Utils
{

    public static class JoinHelper 
    {
        public static string GetJoinScript(Type type)
        {
            return type.Name + "_JOIN";
        }
    }

    public static class JoinHelper<S>
    {

        public static readonly string JoinAliasName;
        static JoinHelper()
        {
            JoinAliasName = JoinHelper.GetJoinScript(typeof(S));
        }


        public static TReturn Field<TReturn>(Expression<Func<S, TReturn>> expression)
        {
            return default(TReturn);
        }

    }
}
