using System.Collections.Immutable;
using System.Linq;

namespace FreeSql.Natasha.Extension
{
    public class PropTemplate<T> : BaseTemplate<T>
    {
        public PropTemplate(IFreeSql freeSql) : base(freeSql)
        {

        }

        private static readonly ImmutableHashSet<string> _propMemberCache;
        static PropTemplate()
        {
            var props = typeof(T).GetProperties().Select(item => item.Name);
            _propMemberCache = ImmutableHashSet.CreateRange(props);
        }

        public bool IsExistProp(string propName)
        {
            return _propMemberCache.Contains(propName);
        }

    }
}
