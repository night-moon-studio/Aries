using FreeSql;
using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Aries
{
    public static class FuzzyQueryOperator<TEntity> where TEntity : class
    {


        public static ImmutableDictionary<string, Action<ISelect<TEntity>, string>> FuzzyQueryHandlerMapping;
        static FuzzyQueryOperator()
        {
            FuzzyQueryHandlerMapping = ImmutableDictionary.Create<string, Action<ISelect<TEntity>, string>>();
        }


        public static void FuzzyQueryModel(ISelect<TEntity> select, FuzzyModel model)
        {

            if (FuzzyQueryHandlerMapping.TryGetValue(model.FieldName, out var fuzzyAction))
            {

                fuzzyAction(select, model.FuzzyValue);

            }
            else if (PropertiesCache<TEntity>.PropMembers.Contains(model.FieldName) &&
                !PropertiesCache<TEntity>.BlockWhereFields.Contains(model.FieldName))
            {
                Action<ISelect<TEntity>, string> action = NDelegate
                    .DefaultDomain()
                    .Action<ISelect<TEntity>, string>($"arg1.Where(obj=>obj.{model.FieldName}.Contains(arg2));");
                FuzzyQueryHandlerMapping = FuzzyQueryHandlerMapping.Add(model.FieldName, action);
                action(select, model.FuzzyValue);
            }

        }
    }
}
