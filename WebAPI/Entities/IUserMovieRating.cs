namespace WebAPI.Entities
{
    public interface IUserMovieRating
    {
        int MovieId { get; set; }
        string MovieTitle { get; set; }
        string UserName { get; set; }
        double UserRatingValue { get; set; }
    }
}