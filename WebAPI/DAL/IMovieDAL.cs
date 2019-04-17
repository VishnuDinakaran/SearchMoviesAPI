using System.Linq;
using WebAPI.Entities;

namespace WebAPI.DAL
{
    public interface IMovieDAL
    {
        IQueryable<Movie> Movies { get; }
        IQueryable<UserMovieRating> UserRatings { get; }
        IQueryable<User> Users { get; }

        void UpdateMovieAvgUserRaing(Movie movie, double movieAvgUserRating);

        void UpdateUserRatings(UserMovieRating userMovieRating);
        void AddUserRatings(UserMovieRating userMovieRating);

        int SaveChanges();
    }
}
