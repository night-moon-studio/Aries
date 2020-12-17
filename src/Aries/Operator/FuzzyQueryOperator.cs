using FreeSql;
using Natasha.CSharp;
using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Aries
{

    public static class FuzzyQueryOperator<TEntity> where TEntity : class
    {
        private static DynamicDictionaryBase<string,Func<FuzzyModel, Expression<Func<TEntity, bool>>>> FuzzyQueryHandlerMapping;
        private static readonly ConcurrentDictionary<string, Func<FuzzyModel,Expression<Func<TEntity,bool>>>> _dict;
        static FuzzyQueryOperator()
        {
            _dict = new ConcurrentDictionary<string, Func<FuzzyModel, Expression<Func<TEntity, bool>>>>();
            FuzzyQueryHandlerMapping = _dict.PrecisioTree();

        }


        public static Expression<Func<TEntity, bool>> GetFuzzyExpression(FuzzyModel model)
        {
            var func = FuzzyQueryHandlerMapping[model.FuzzyField];
            if (func!=default)
            {
                return func(model);

            }
            else if (PropertiesCache<TEntity>.PropMembers.Contains(model.FuzzyField) &&
                !PropertiesCache<TEntity>.GetBlockWhereFields().Contains(model.FuzzyField))
            {

                var action = NDelegate
                    .DefaultDomain()
                    .Func<FuzzyModel, Expression<Func<TEntity, bool>>>($@"
Expression<Func<{typeof(TEntity).GetDevelopName()},bool>> exp = default;
if(arg.IgnoreCase) {{
    exp = obj => obj.{model.FuzzyField}.Contains(arg.FuzzyValue,StringComparison.CurrentCultureIgnoreCase);
}} else {{
    exp = obj => obj.{model.FuzzyField}.Contains(arg.FuzzyValue);
}}
return exp;
");
                _dict[model.FuzzyField] = action;
                FuzzyQueryHandlerMapping = _dict.PrecisioTree();
                return action(model);
            }
            return default;
        }

    }
}
