using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Entities;

namespace WebAPI.DAL
{
    public static class TestData
    {
        private static List<Movie> _movies = new List<Movie>
        {
                new Movie { Id = 3, Title = "Rio", YearOfRelease = 2011, Genres = "Animated", AverageRating = 3.768, RunningTime = 112 },
                new Movie { Id = 4, Title = "The Mummy", YearOfRelease = 1999, Genres = "Fantasy/Adventure", AverageRating = 4.768, RunningTime= 152 },
                new Movie { Id = 5, Title = "Rio2", YearOfRelease = 2014, Genres = "Animated", AverageRating = 3.768, RunningTime = 108 },
                new Movie { Id = 6, Title = "The Mummy Returns", YearOfRelease = 2001, Genres = "Fantasy", AverageRating = 3.68, RunningTime = 112 },
                new Movie { Id = 7, Title = "Jurassic Park", YearOfRelease = 1993, Genres = "Sci-Fi/Thriller", AverageRating = 5, RunningTime = 175 },
                new Movie { Id = 8, Title = "The Lost World: Jurassic Park", YearOfRelease = 1997, Genres = "Sci-Fi/Adventure", AverageRating = 3.01, RunningTime = 122 },
                new Movie { Id = 9, Title = "Jumanji", YearOfRelease = 1995, Genres = "Fantasy/Family", AverageRating = 4.18, RunningTime = 202 },
                new Movie { Id = 10, Title = "Spider-Man", YearOfRelease = 2002, Genres = "Action, Adventure, Sci-Fi", AverageRating = 3.98, RunningTime = 180 },
                new Movie { Id = 11, Title = "The Lion King", YearOfRelease = 1994, Genres = "Animated/Musical", AverageRating = 4.76, RunningTime = 105 },
                new Movie { Id = 12, Title = "Independence Day", YearOfRelease = 1996, Genres = "Action, Adventure, Sci-Fi", AverageRating = 3.768, RunningTime = 113 },
                new Movie { Id = 13, Title = "Speed", YearOfRelease = 1994, Genres = "Action, Adventure, Crime", AverageRating = 3.268, RunningTime = 113 },
                new Movie { Id = 14, Title = "Toy Story", YearOfRelease = 1995, Genres = "Animation, Adventure, Comedy", AverageRating = 2.768, RunningTime = 113 },
                new Movie { Id = 15, Title = "Tomorrow Never Dies", YearOfRelease = 1997, Genres = "Action, Adventure, Thriller", AverageRating = 3.48, RunningTime = 113 },
                new Movie { Id = 16, Title = "Mr. Nice Guy", YearOfRelease = 1997, Genres = "Action, Comedy, Crime", AverageRating = 3.768, RunningTime = 168 },
                new Movie { Id = 17, Title = "Who Am I?", YearOfRelease = 1998, Genres = "Action, Adventure, Comedy", AverageRating = 3.28, RunningTime = 113 },
                new Movie { Id = 18, Title = "Captain America: The First Avenger", YearOfRelease = 1998, Genres = "Action, Adventure, Sci-Fi", AverageRating = 4.768 },
                new Movie { Id = 19, Title = "Zootopia", YearOfRelease = 2016, Genres = "Animation, Adventure, Comedy", AverageRating = 3.48, RunningTime = 198 },
                new Movie { Id = 20, Title = "Dumbo", YearOfRelease = 2019, RunningTime = 113, Genres = "Adventure, Family, Fantasy", AverageRating = 3.48 },

        };

        static List<User> _users = new List<User>
        {
                new User { Name = "John" },
                new User { Name = "Tom" },
        };

        static List<UserMovieRating> _userMovieRatings = new List<UserMovieRating>
        {
                new UserMovieRating { MovieId = 3, MovieTitle = "Rio", UserName = "John", UserRatingValue = 3.25 },
                new UserMovieRating { MovieId = 4, MovieTitle = "The Mummy", UserName = "John", UserRatingValue = 4.66 },
                new UserMovieRating { MovieId = 7, MovieTitle = "Jurassic Park", UserName = "John", UserRatingValue = 4.88 },

                new UserMovieRating { MovieId = 3, MovieTitle = "Rio", UserName = "Tom", UserRatingValue = 4.25 },
                new UserMovieRating { MovieId = 4, MovieTitle = "The Mummy", UserName = "Tom", UserRatingValue = 4.66 },
                new UserMovieRating { MovieId = 20, MovieTitle = "Dumbo", UserName = "Tom", UserRatingValue = 4.25 },
                new UserMovieRating { MovieId = 7, MovieTitle = "Jurassic Park", UserName = "Tom", UserRatingValue = 3.88 },
                new UserMovieRating { MovieId = 19, MovieTitle = "Zootopia", UserName = "Tom", UserRatingValue = 3.88 },
        };

        public static List<Movie> Movies { get { return _movies; } }
        public static List<User> Users { get { return _users; } }
        public static List<UserMovieRating> UserMovieRatings { get { return _userMovieRatings; } }

        
    }
}
