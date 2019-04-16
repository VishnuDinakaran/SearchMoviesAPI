using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entities
{
    public class Query
    {
        #region Constructor
        public Query()
        {
            Junction = "And";
            Index = 1;
        } 
        #endregion

        /// <summary>
        /// C# Type's Property Name
        /// eg: Movie's Title or YearOfRelease or RunningTime
        /// </summary>
        [Required]
        public string PropertyName { get; set; }

        /// <summary>
        /// Operator 
        /// </summary>
        [Required]
        [StringRange(AllowableValues = new string[] { "==","!=", "Equals", "NOTEQUALS","<", "LESSTHAN", "<=", "LESSTHANOREQUAL", ">", "GREATERTHAN", ">=", "GREATERTHANOREQUAL", "STRINGCONTAINS" })]
        public string Operator { get; set; }

        /// <summary>
        /// Value of the Property 
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// Junction to be used with Parent
        /// </summary>
        //[Required]
        [StringRange(AllowableValues = new string[] { "AND", "OR" })]
        public string Junction { get; set; }

        /// <summary>
        /// Add more quries with Junction and Index so that they for fill condition
        /// eg: (Title == 'Rio' AND YearOfRelease == 2011)
        /// </summary>
        public List<Query> Queries { get; set; }

        /// <summary>
        /// Index of the query so thats they line up in right priority witnin List<Query> Queries when query is constructed
        /// </summary>
        public int Index { get; set; }

        

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