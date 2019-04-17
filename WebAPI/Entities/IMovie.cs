namespace WebAPI.Entities
{
    public interface IMovie
    {
        double AverageRating { get; set; }
        string Genres { get; set; }
        int Id { get; set; }
        int RunningTime { get; set; }
        string Title { get; set; }
        int YearOfRelease { get; set; }
    }
}