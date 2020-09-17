public class SqlModel<T> where T : class
{
    public QueryModel QueryModel { get; set; }
    public OperatorModel<T> QueryInstance { get; set; }
    public OperatorModel<T> ModifyInstance { get; set; }
}