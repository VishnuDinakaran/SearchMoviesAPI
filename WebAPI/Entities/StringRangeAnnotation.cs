using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Entities
{
    public class StringRangeAttribute : ValidationAttribute
    {
        public string[] AllowableValues { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {            
            if (AllowableValues.Any(x => string.Compare(x, value?.ToString(), true) == 0))
            {
                return ValidationResult.Success;
            }

            var msg = $"Please enter valid values: {string.Join(", ", (AllowableValues ?? new string[] { $"InValid property value." }))}.";
            return new ValidationResult(msg);
        }
    }
}
