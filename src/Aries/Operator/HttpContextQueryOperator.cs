using BTFindTree;
using FreeSql;
using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Aries
{
    public static class HttpContextQueryOperator<TEntity> where TEntity : class
    {
        public static readonly Func<string, TEntity, Expression<Func<TEntity,bool>>> WhereHandler;
        static HttpContextQueryOperator()
        {

            var stringBuilder = new StringBuilder();
            var propNames = typeof(TEntity).GetProperties().Select(item=>item.Name);
            var blockWhereList = PropertiesCache<TEntity>.GetBlockWhereFields();
            stringBuilder.AppendLine($"Expression<Func<{typeof(TEntity).GetDevelopName()},bool>> exp = default;");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var name in propNames)
            {

                if (!blockWhereList.Contains(name))
                {
                    dict[name] = $"exp = obj => obj.{name} == arg2.{name};";
                }
                
            }
            stringBuilder.AppendLine(BTFTemplate.GetGroupPrecisionPointBTFScript(dict,"arg1"));
            stringBuilder.AppendLine("return exp;");
            var result = stringBuilder.ToString();
            WhereHandler += NDelegate
   .DefaultDomain()
   .UnsafeFunc<string, TEntity, Expression<Func<TEntity, bool>>>(result);


        }
    }
}
