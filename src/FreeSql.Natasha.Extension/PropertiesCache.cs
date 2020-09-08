using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace FreeSql.Natasha.Extension
{

    public static class PropertiesCache<TEntity> where TEntity :class
    {

        public static readonly ImmutableHashSet<string> PropMembers;
        public static ImmutableHashSet<string> BlockWhereFields;
        public static ImmutableHashSet<string> BlockSelectFields;
        public static ImmutableHashSet<string> AllowUpdateFields;
        public static string[] AllowUpdateColumns;
        public static Action<TEntity> UpdateInitFunc;
        public static Action<TEntity> InsertInitFunc;
        public static Action<ISelect<TEntity>> SelectInitFunc;

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
            AllowUpdateColumns = AllowUpdateFields.ToArray();
        }


        public static void SetUpdateInit(Action<TEntity> action)
        {

            UpdateInitFunc += action;

        }

        public static void SetInsertInit(Action<TEntity> action)
        {

            InsertInitFunc += action;

        }
        public static void SetSelectInit(Action<ISelect<TEntity>> action)
        {

            SelectInitFunc += action;

        }

        public static IEnumerable<string> GetUpdateFields(IEnumerable<string> keys)
        {
            var result = new HashSet<string>(keys);
            result.IntersectWith(AllowUpdateFields);
            return result;
        }

        public static IEnumerable<string> GetWhereFields(IEnumerable<string> keys)
        {
            var result = new HashSet<string>(keys);
            result.ExceptWith(BlockWhereFields);
            return result;
        }

        public static IEnumerable<string> GetSelectFields(IEnumerable<string> keys)
        {
            var result = new HashSet<string>(keys);
            result.ExceptWith(BlockSelectFields);
            return result;
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
