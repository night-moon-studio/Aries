using System.Collections.Immutable;
using System.Linq;

namespace FreeSql.Natasha.Extension
{
    public class PropTemplate<TEntity> : BaseTemplate<TEntity> where TEntity : class
    {
        public PropTemplate(IFreeSql freeSql) : base(freeSql)
        {

        }

        private static readonly ImmutableHashSet<string> _propMemberCache;
        static PropTemplate()
        {
            var props = typeof(TEntity).GetProperties().Select(item => item.Name);
            _propMemberCache = ImmutableHashSet.CreateRange(props);
        }

        public bool IsExistProp(string propName)
        {
            return _propMemberCache.Contains(propName);
        }

    }
}
