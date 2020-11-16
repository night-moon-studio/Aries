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
    /// <summary>
    /// 将遍历属性，排除主键，针对上传的字段进行更新。
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public static class HttpContextUpdateOperator<TEntity> where TEntity : class
    {
        public static readonly Func<string, TEntity, Expression<Func<TEntity, bool>>> UpdateFieldsHandler;
        static HttpContextUpdateOperator()
        {

            var stringBuilder = new StringBuilder();
            var propNames = typeof(TEntity).GetProperties().Select(item=>item.Name);
            var allowList = PropertiesCache<TEntity>.GetAllowUpdateFields();
            stringBuilder.AppendLine($"Expression<Func<{typeof(TEntity).GetDevelopName()},bool>> exp = default;");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var name in propNames)
            {

                if (allowList.Contains(name))
                {
                    dict[name] = $"exp = obj => obj.{name} == arg2.{name};";
                }

            }
            stringBuilder.AppendLine(BTFTemplate.GetGroupPrecisionPointBTFScript(dict, "arg1"));
            stringBuilder.AppendLine("return exp;");

            UpdateFieldsHandler += NDelegate
    .DefaultDomain()
    .UnsafeFunc<string, TEntity, Expression<Func<TEntity, bool>>>(stringBuilder.ToString());

        }
    }
}
