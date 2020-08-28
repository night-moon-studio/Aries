using Natasha.CSharp;
using System;
using System.Collections.Immutable;

namespace FreeSql.Natasha.Extension
{
    
    public static class FuzzyQueryExtensions
    {
        public static void FuzzyQuery<TEntity>(this ISelect<TEntity> select, FuzzyModel model) where TEntity : class
        {

            FuzzyQueryOperator<TEntity>.FuzzyQueryModel(select, model);

        }
    }
}
