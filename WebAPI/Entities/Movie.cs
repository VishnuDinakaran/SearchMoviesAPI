using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Entities
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
       
        public int RunningTime { get; set; }
        public string Genres { get; set; }
        public double AverageRating { get; set; }

        public List<UserMovieRating> UserRatings { get; set; }

        public override string ToString()
        {
            return $@"{Id} - ""{Title}"" - {YearOfRelease} - ({RunningTime})mins - ""{Genres}"" {AverageRating}";
        }
    }
}
