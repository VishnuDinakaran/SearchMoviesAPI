using System.Collections.Generic;
using System.Linq;
using WebAPI.DAL;
using WebAPI.Entities;

namespace WebApi_Tests
{
    public class MockMovieDbContext : IMovieDAL
    {
        List<Movie> _movies = new List<Movie>();
        List<UserMovieRating> _userMovieRating = new List<UserMovieRating>();
        List<User> _users = new List<User>();

        public MockMovieDbContext()
        {
            _movies.AddRange(TestData.Movies);
            _users.AddRange(TestData.Users);
            _userMovieRating.AddRange(TestData.UserMovieRatings);
        }

        public IQueryable<Movie> Movies { get { return _movies.AsQueryable(); } }

        public IQueryable<UserMovieRating> UserRatings { get { return _userMovieRating.AsQueryable(); } }

        public IQueryable<User> Users { get { return _users.AsQueryable(); } }

        public void AddUserRatings(UserMovieRating userMovieRating)
        {

        }

        public int SaveChanges()
        {
            return 1;
        }

        public void UpdateMovieAvgUserRaing(Movie movie, double movieAvgUserRating)
        {

        }

        public void UpdateUserRatings(UserMovieRating userMovieRating)
        {

        }
    }
}
