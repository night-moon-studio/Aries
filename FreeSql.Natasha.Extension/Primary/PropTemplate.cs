using FreeSql.PgSql.Natasha.Extension.DBInfomation;

namespace FreeSql.Natasha.Extension
{
    public class PrimaryTemplate<T> : PropTemplate<T>
    {

        protected static readonly string PrimaryKey;
        static PrimaryTemplate()
        {
            PrimaryKey = TableInfomation<T>.PrimaryKey;
        }
        public PrimaryTemplate(IFreeSql freeSql) : base(freeSql)
        {
            
        }

    }


}
