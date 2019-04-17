using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Entities
{
    public class UserMovieRating : IUserMovieRating
    {
        //[Key]
        //public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }

        [Required]
        public string MovieTitle { get; set; }

        [Required]
        public string UserName { get; set; }

        [Range(0, 5)]
        public double UserRatingValue { get; set; }

        [ForeignKey("UserName")]
        public User User { get; set; }

        public override string ToString()
        {
            return $"{UserName}  {MovieId} {MovieTitle} {UserRatingValue}";
        }
    }
}
