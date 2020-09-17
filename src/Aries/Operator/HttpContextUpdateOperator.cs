using FreeSql;
using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aries
{
    /// <summary>
    /// 将遍历属性，排除主键，针对上传的字段进行更新。
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public static class HttpContextUpdateOperator<TEntity> where TEntity : class
    {
        public static readonly Action<IUpdate<TEntity>, IEnumerable<string>, TEntity> UpdateFieldsHandler;
        static HttpContextUpdateOperator()
        {

            var stringBuilder = new StringBuilder();
            var props = typeof(TEntity).GetProperties().Select(item=>item.Name);
            stringBuilder.AppendLine("foreach(var field in arg2){");
            foreach (var item in props)
            {

                if (item!=TableInfomation<TEntity>.PrimaryKey)
                {

                    if (PropertiesCache<TEntity>.AllowUpdateFields.Contains(item))
                    {
                        stringBuilder.AppendLine($"if(field == \"{item}\"){{ arg1.Set(obj=>obj.{item}==arg3.{item}); }}");
                        stringBuilder.Append("else ");
                    }
                   
                }
                
            }
            stringBuilder.Length -= 5;
            stringBuilder.Append("}");
            var result = stringBuilder.ToString();

            UpdateFieldsHandler += NDelegate
    .DefaultDomain()
    .Action<IUpdate<TEntity>, IEnumerable<string>, TEntity>(result);

        }
    }
}
