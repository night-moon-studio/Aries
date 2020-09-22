using FreeSql;
using Natasha.CSharp;
using System;
using System.Collections.Immutable;

namespace Aries
{
    public static class FuzzyQueryOperator<TEntity> where TEntity : class
    {

        public static ImmutableDictionary<string, Action<ISelect<TEntity>, string>> FuzzyQueryHandlerMapping;
        public static ImmutableDictionary<string, Action<IUpdate<TEntity>, string>> FuzzyModifyHandlerMapping;
        public static ImmutableDictionary<string, Action<IDelete<TEntity>, string>> FuzzyDeleteHandlerMapping;
        static FuzzyQueryOperator()
        {
            FuzzyQueryHandlerMapping = ImmutableDictionary.Create<string, Action<ISelect<TEntity>, string>>();
            FuzzyModifyHandlerMapping = ImmutableDictionary.Create<string, Action<IUpdate<TEntity>, string>>();
            FuzzyDeleteHandlerMapping = ImmutableDictionary.Create<string, Action<IDelete<TEntity>, string>>();
        }


        public static void FuzzyQueryModel(ISelect<TEntity> select, FuzzyModel model)
        {

            if (FuzzyQueryHandlerMapping.TryGetValue(model.FieldName, out var fuzzyAction))
            {

                fuzzyAction(select, model.FuzzyValue);

            }
            else if (PropertiesCache<TEntity>.PropMembers.Contains(model.FieldName) &&
                !PropertiesCache<TEntity>.GetBlockWhereFields().Contains(model.FieldName))
            {
                Action<ISelect<TEntity>, string> action = NDelegate
                    .DefaultDomain()
                    .Action<ISelect<TEntity>, string>($"arg1.Where(obj=>obj.{model.FieldName}.Contains(arg2));");
                FuzzyQueryHandlerMapping = FuzzyQueryHandlerMapping.Add(model.FieldName, action);
                action(select, model.FuzzyValue);
            }

        }


        public static void FuzzyQueryModel(IUpdate<TEntity> update, FuzzyModel model)
        {

            if (FuzzyModifyHandlerMapping.TryGetValue(model.FieldName, out var fuzzyAction))
            {

                fuzzyAction(update, model.FuzzyValue);

            }
            else if (PropertiesCache<TEntity>.PropMembers.Contains(model.FieldName) &&
                !PropertiesCache<TEntity>.GetBlockWhereFields().Contains(model.FieldName))
            {
                Action<IUpdate<TEntity>, string> action = NDelegate
                    .DefaultDomain()
                    .Action<IUpdate<TEntity>, string>($"arg1.Where(obj=>obj.{model.FieldName}.Contains(arg2));");
                FuzzyModifyHandlerMapping = FuzzyModifyHandlerMapping.Add(model.FieldName, action);
                action(update, model.FuzzyValue);
            }

        }


        public static void FuzzyQueryModel(IDelete<TEntity> delete, FuzzyModel model)
        {

            if (FuzzyDeleteHandlerMapping.TryGetValue(model.FieldName, out var fuzzyAction))
            {

                fuzzyAction(delete, model.FuzzyValue);

            }
            else if (PropertiesCache<TEntity>.PropMembers.Contains(model.FieldName) &&
                !PropertiesCache<TEntity>.GetBlockWhereFields().Contains(model.FieldName))
            {
                Action<IDelete<TEntity>, string> action = NDelegate
                    .DefaultDomain()
                    .Action<IDelete<TEntity>, string>($"arg1.Where(obj=>obj.{model.FieldName}.Contains(arg2));");
                FuzzyDeleteHandlerMapping = FuzzyDeleteHandlerMapping.Add(model.FieldName, action);
                action(delete, model.FuzzyValue);
            }

        }
    }
}
