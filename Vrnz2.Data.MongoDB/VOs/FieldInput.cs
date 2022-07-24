namespace Vrnz2.Data.MongoDB.VOs
{
    public enum FieldType
    {
        LOCALIZED,
        DATETIME,
        STRING,
        DATE,
        BOOLEAN,
        NUMERIC,
        TEXT,
    }

    public class FieldInput
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
    }
}
