public class QueryModel
{

    public int Page { get; set; }

    public int Size { get; set; }

    public bool Total { get; set; }

    public OrderModel[] Orders { get; set; }

    public FuzzyModel[] Fuzzy { get; set; }
}






