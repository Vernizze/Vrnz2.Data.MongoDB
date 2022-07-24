namespace Vrnz2.Data.MongoDB.VOs
{
    public enum CriteriaOperator
    {
        CONTAINS,
        EQUAL,
        IN
    }

    public class CriterionInput
    {
        public FieldInput Field { get; set; }

        public CriteriaOperator Operator { get; set; }

        public string Value { get; set; }

    }
}
