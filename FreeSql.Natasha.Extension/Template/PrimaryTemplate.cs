namespace FreeSql.Natasha.Extension
{
    public class PrimaryTemplate<TEntity> : PropTemplate<TEntity> where TEntity : class
    {

        protected static readonly string PrimaryKey;
        static PrimaryTemplate()
        {
            PrimaryKey = TableInfomation<TEntity>.PrimaryKey;
        }
        public PrimaryTemplate(IFreeSql freeSql) : base(freeSql)
        {
            
        }

    }


}
