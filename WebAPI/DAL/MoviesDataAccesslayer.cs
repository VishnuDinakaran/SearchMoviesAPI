using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WebAPI.Entities;

namespace WebAPI.DAL
{
    public class MoviesDataAccesslayer: IMovieDAL
    {
        readonly MovieDbContext _movieDbContext;
        public MoviesDataAccesslayer(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public IQueryable<Movie> Movies { get { return _movieDbContext.Movies; } }

        public IQueryable<UserMovieRating> UserRatings { get { return _movieDbContext.UserRatings; } }

        public IQueryable<User> Users { get { return _movieDbContext.Users; } }

        public void AddUserRatings(UserMovieRating userMovieRating)
        {
           _movieDbContext.UserRatings.Add(userMovieRating);
        }

        public int SaveChanges()
        {
           return _movieDbContext.SaveChanges();
        }

        public void UpdateMovieAvgUserRaing(Movie movie, double movieAvgUserRating)
        {
            movie.AverageRating = movieAvgUserRating;
            _movieDbContext.Entry(movie).Property("AverageRating").IsModified = true;
            var movieEntry = _movieDbContext.Entry(movie);
            foreach (var item in movieEntry.Properties)
            {
                if (item.Metadata.Name != "AverageRating")
                {
                    item.IsModified = false;
                }
                else
                {
                    item.IsModified = true;
                }
            }
            _movieDbContext.SaveChanges();
        }

        public void UpdateUserRatings(UserMovieRating userMovieRating)
        {
            _movieDbContext.UserRatings.Update(userMovieRating);
        }
    }
}
