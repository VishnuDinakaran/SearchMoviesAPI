namespace WebAPI.Entities
{
    public class UserMovieRatingSummary : IUserMovieRating
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string UserName { get; set; }
        public double UserRatingValue { get; set; }
    }
}