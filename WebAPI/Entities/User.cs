using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Entities
{
    public class User
    {
        [Key]
        public string Name { get; set; }

        public List<UserMovieRating> MovieRatings { get; set; }
    }
}
