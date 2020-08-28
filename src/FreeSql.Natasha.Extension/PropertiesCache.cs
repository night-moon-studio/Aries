using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace FreeSql.Natasha.Extension
{
    public static class PropertiesCache<TEntity>
    {
        public static readonly ImmutableHashSet<string> PropMembers;
        public static ImmutableHashSet<string> BlockWhereFields;
        public static ImmutableHashSet<string> BlockSelectFields;
        public static ImmutableHashSet<string> AllowUpdateFields;
        static PropertiesCache()
        {
            PropMembers = ImmutableHashSet.CreateRange(typeof(TEntity).GetProperties().Select(item => item.Name));
            BlockWhereFields = ImmutableHashSet.Create<string>();
            AllowUpdateFields = ImmutableHashSet.CreateRange(PropMembers);
            BlockSelectFields = ImmutableHashSet.Create<string>();
        }

        public static void SetSelectBlockFields(params string[] fields)
        {
            BlockSelectFields = ImmutableHashSet.CreateRange(fields);
            ResetSelectScript();
        }
        public static void SetWhereBlockFields(params string[] fields)
        {
            BlockWhereFields = ImmutableHashSet.CreateRange(fields);
        }
        public static void SetUpdateAllowFields(params string[] fields)
        {
            AllowUpdateFields = ImmutableHashSet.CreateRange(fields);
        }


        public static void ResetSelectScript()
        {
            StringBuilder script = new StringBuilder();
            foreach (var item in PropMembers)
            {
                if (!BlockSelectFields.Contains(item))
                {
                    script.Append($"a.\"{item}\",");
                }

            }
            if (script.Length>1)
            {
                script.Length -= 1;
            }
            LimitReturnOperator<TEntity>.ReturnScript = script.ToString();
        }

    }
}
