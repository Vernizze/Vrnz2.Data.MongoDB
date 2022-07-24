using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vrnz2.Data.MongoDB.VOs;

namespace Vrnz2.Data.MongoDB.Extensions
{
    public static class CriteriaInputExtension
    {
        public static string GetFilter(this CriteriaInput criteria)
        {

            var result = new List<string>();

            if (criteria.And.Any())
                criteria.And.ForEach(item => result.Add(GetFilterObject(item.Items)));

            if (criteria.Items.Any())
                result.Add(GetFilterObject(criteria.Items));

            if (criteria.Or.Any())
            {
                var resultOr = new List<string>();
                criteria.Or.ForEach(item => resultOr.Add(GetFilterObject(item.Items)));
                result.Add($"{{ $or:[ {string.Join(", ", resultOr)}] }}");
            }

            return $"{{ $and:[{string.Join(", ", result)}] }}";
        }

        private static string GetFilterObject(List<CriterionInput> values)
        {
            var result = new List<string>();
            values.ToList().ForEach(p =>
            {
                var o = new StringBuilder();
                switch (p.Operator)
                {
                    case CriteriaOperator.EQUAL:

                        if (p.Field.Type == FieldType.DATE || p.Field.Type == FieldType.DATETIME)
                        {

                            if (DateTimeOffset.TryParse(p.Value, out var startDate))
                            {
                                var endDate = startDate.AddDays(1).AddMilliseconds(-1);
                                o.Append($"{p.Field.Name}: ");
                                o.Append($"{{\"$gte\": new Date(\"{startDate.ToString("yyyy-MM-dd THH:mm:ss.fffZ")}\")");
                                o.Append($", \"$lt\": new Date(\"{endDate.ToString("yyyy-MM-dd THH:mm:ss.fffZ")}\")}}");
                            }
                        }
                        else if (p.Field.Type == FieldType.TEXT || p.Field.Type == FieldType.STRING)
                            o.Append($"{p.Field.Name}: \"{p.Value}\"");
                        else
                            o.Append($"{p.Field.Name}: {p.Value}");

                        break;

                    case CriteriaOperator.CONTAINS:
                        o.Append($"{p.Field.Name}:  /.*{p.Value}.*/i");

                        break;

                    case CriteriaOperator.IN:
                        var values = p.Value.Split(',');
                        if (p.Field.Type == FieldType.TEXT || p.Field.Type == FieldType.STRING)
                        {
                            var stringValues = values.ToList().Select(q => $"'{q.Trim()}'").ToList();
                            o.Append($"{p.Field.Name}: {{ $in: [{string.Join(", ", stringValues)}] }}");
                        }
                        else
                            o.Append($"{p.Field.Name}: {{ $in: [{string.Join(", ", values)}] }}");

                        break;

                    default:
                        o.Append($"{p.Field.Name}: \"{p.Value}\"");

                        break;
                }

                result.Add(o.ToString());
            });

            return $" {{ {string.Join(", ", result)} }}";
        }
    }
}
