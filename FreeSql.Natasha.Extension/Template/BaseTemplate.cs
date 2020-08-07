namespace FreeSql.Natasha.Extension
{
    public class BaseTemplate<TEntity> where TEntity : class
    {
        public readonly IFreeSql SqlHandler;
        public BaseTemplate(IFreeSql freeSql)
        {
            SqlHandler = freeSql;
        }
    }
}
