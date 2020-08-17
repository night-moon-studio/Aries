using System.Collections.Immutable;
using System.Linq;

namespace FreeSql.Natasha.Extension
{
    public static class PropertiesCache<TEntity>
    {
        public static readonly ImmutableHashSet<string> PropMembers;
        static PropertiesCache()
        {
            PropMembers = ImmutableHashSet.CreateRange(typeof(TEntity).GetProperties().Select(item => item.Name));
        }
    }
}
