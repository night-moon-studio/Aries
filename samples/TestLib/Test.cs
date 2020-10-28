using FreeSql.DataAnnotations;

namespace TestLib
{

    public class TestResult
    {
        public long Id { get; set; }
        public string TESTName { get; set; }
        public string DomainName { get; set; }
        public string TypeName { get; set; }
    }

    public class Test
    {
        public long Id { get; set; }
        public short Domain { get; set; }
        public short Type { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    [Table(Name = "Test2")]
    public class Test21
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class Test3
    {
        public long Id { get; set; }
        public string TypeName { get; set; }
    }
}
