using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebAPI.Entities
{
    public class SearchRequest
    {
        public SearchRequest()
        {
          
        }
        public Guid RequestId { get; set; }

        [Required]
        public Query Query { get; set; }

        public string Type { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder($"Req Id : {RequestId}");
            if (Query != null)
            {
                stringBuilder.AppendLine($" ");
                stringBuilder.AppendLine($"{Query}");
            }
            else
            {
                stringBuilder.AppendLine("Query is null.");
            }

            return stringBuilder.ToString();
        }
    }
}