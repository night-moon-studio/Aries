using Natasha.CSharp;
using System;
using System.Collections.Immutable;

namespace FreeSql.Natasha.Extension
{
    public static class FuzzyOperator<TEntity> where TEntity : class
    {
        public static ImmutableDictionary<string, Action<ISelect<TEntity>, string>> FuzzyQueryHandlerMapping;
        static FuzzyOperator()
        {
            FuzzyQueryHandlerMapping = ImmutableDictionary.Create<string, Action<ISelect<TEntity>, string>>();
        }
        public static void FuzzyQueryModel(ISelect<TEntity> select, FuzzyModel model)
        {

            if (FuzzyQueryHandlerMapping.TryGetValue(model.FieldName, out var fuzzyAction))
            {

                fuzzyAction(select, model.FuzzyValue);

            }
            else if (PropertiesCache<TEntity>.PropMembers.Contains(model.FieldName))
            {
                Action<ISelect<TEntity>, string> action = NDelegate
                    .DefaultDomain()
                    .Action<ISelect<TEntity>, string>($"arg1.Where(obj=>obj.{model.FieldName}.Contains(arg2));");
                FuzzyQueryHandlerMapping = FuzzyQueryHandlerMapping.Add(model.FieldName, action);
                action(select, model.FuzzyValue);
            }

        }
    }
    public static class FuzzyQueryExtensions
    {
        public static void FuzzyQuery<TEntity>(this ISelect<TEntity> select, FuzzyModel model) where TEntity : class
        {
            FuzzyOperator<TEntity>.FuzzyQueryModel(select, model);

        }
    }
}
