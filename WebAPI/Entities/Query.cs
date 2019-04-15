using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entities
{
    public class Query
    {
        public Guid Id { get; set; }
        [Required]
        public string PropertyName { get; set; }
        [Required]
        public string Operator { get; set; }
        [Required]
        public string Value { get; set; }
        //[Required]
        public string Junction { get; set; }

        public List<Query> Queries { get; set; }

        public int Index { get; set; }

        public Guid ParentQueryId { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder($"Query: ");
            stringBuilder.AppendLine($" ");
            stringBuilder.AppendLine($"     {PropertyName} {Operator} {Value}");

            if (Queries != null && Queries.Any())
            {
                IEnumerable<Query> queries = Queries.OrderBy(x => x.Index);
                foreach (Query item in queries)
                {
                    stringBuilder.AppendLine($" {item.Junction} {item.PropertyName} {item.Operator} {item.Value}");
                }
            }
            return stringBuilder.ToString();
        }
    }


}