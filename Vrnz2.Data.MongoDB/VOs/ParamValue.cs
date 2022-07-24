namespace Vrnz2.Data.MongoDB.VOs
{
    public class ParamValue
    {
        public string Criteria { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortDefinition { get; set; } = null;
        public bool Ascending { get; set; } = true;

    }
}
