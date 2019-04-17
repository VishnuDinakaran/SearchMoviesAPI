namespace WebAPI.Entities
{
    public class MovieSummay : IMovie
    {
        public double AverageRating { get; set; }
        public string Genres { get; set; }
        public int Id { get; set; }
        public int RunningTime { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
    }
}