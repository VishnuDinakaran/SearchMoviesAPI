using System;
using System.ComponentModel.DataAnnotations;
using WebAPI.Entities;

namespace WebAPI.Entities
{
    public class UpdateUserRatingRequest
    {
        public Guid Id { get; set; }

        [Required]
        public UserMovieRating UserRating { get; set; }
    }
}