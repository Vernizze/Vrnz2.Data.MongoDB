using System.Collections.Generic;
using System.Linq;

namespace Vrnz2.Data.MongoDB.VOs
{
    public class CriteriaInput
    {
        public List<CriterionInput> Items { get; set; } = new List<CriterionInput>();
        public List<CriteriaInput> Or { get; set; } = new List<CriteriaInput>();
        public List<CriteriaInput> And { get; set; } = new List<CriteriaInput>();

        public bool IsEmpty()
        {
            return !Items.Any() || !Or.Any() || !And.Any();
        }

    }
}
