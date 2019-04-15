using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Entities;

namespace WebAPI.DAL
{
    public class MovieDbContext : DbContext
    {
       
        public DbSet<Movie> Movies { get; set; }
        public DbSet<UserMovieRating> UserRatings { get; set; }
        public DbSet<User> Users { get; set; }


        public MovieDbContext(DbContextOptions options) : base(options)
        {          
            if (Movies != null && !Movies.Any())
            {
                Movies.Add(new Movie { Id = 3, Title = "Rio", YearOfRelease = 2011, Genres = "Animated" , AverageRating = 3.768 });
                Movies.Add(new Movie { Id = 4, Title = "The Mummy", YearOfRelease = 1999, Genres = "Fantasy/Adventure", AverageRating = 4.768 });
                Movies.Add(new Movie { Id = 5, Title = "Rio2", YearOfRelease = 2014, Genres = "Animated", AverageRating = 3.768 });
                Movies.Add(new Movie { Id = 6, Title = "The Mummy Returns", YearOfRelease = 2001, Genres = "Fantasy", AverageRating = 3.68 });
                Movies.Add(new Movie { Id = 7, Title = "Jurassic Park", YearOfRelease = 1993, Genres = "Sci-Fi/Thriller", AverageRating = 5 });
                Movies.Add(new Movie { Id = 8, Title = "The Lost World: Jurassic Park", YearOfRelease = 1997, Genres = "Sci-Fi/Adventure", AverageRating = 3.01 });
                Movies.Add(new Movie { Id = 9, Title = "Jumanji", YearOfRelease = 1995, Genres = "Fantasy/Family", AverageRating = 4.18 });
                Movies.Add(new Movie { Id = 10, Title = "Spider-Man", YearOfRelease = 2002, Genres = "Action, Adventure, Sci-Fi", AverageRating = 3.98 });
                Movies.Add(new Movie { Id = 11, Title = "The Lion King", YearOfRelease = 1994, Genres = "Animated/Musical", AverageRating = 4.76 });
                Movies.Add(new Movie { Id = 12, Title = "Independence Day", YearOfRelease = 1996, Genres = "Action, Adventure, Sci-Fi", AverageRating = 3.768 });
                Movies.Add(new Movie { Id = 13, Title = "Speed", YearOfRelease = 1994, Genres = "Action, Adventure, Crime", AverageRating = 3.268 });
                Movies.Add(new Movie { Id = 14, Title = "Toy Story", YearOfRelease = 1995, Genres = "Animation, Adventure, Comedy", AverageRating = 2.768 });
                Movies.Add(new Movie { Id = 15, Title = "Tomorrow Never Dies", YearOfRelease = 1997, Genres = "Action, Adventure, Thriller", AverageRating = 3.48 });
                Movies.Add(new Movie { Id = 16, Title = "Mr. Nice Guy", YearOfRelease = 1997, Genres = "Action, Comedy, Crime", AverageRating = 3.768 });
                Movies.Add(new Movie { Id = 17, Title = "Who Am I?", YearOfRelease = 1998, Genres = "Action, Adventure, Comedy", AverageRating = 3.28 });
                Movies.Add(new Movie { Id = 18, Title = "Captain America: The First Avenger", YearOfRelease = 1998, Genres = "Action, Adventure, Sci-Fi", AverageRating = 4.768 });
                Movies.Add(new Movie { Id = 19, Title = "Zootopia", YearOfRelease = 2016, Genres = "Animation, Adventure, Comedy", AverageRating = 3.48 });
                Movies.Add(new Movie { Id = 20, Title = "Dumbo", YearOfRelease = 2019, RunningTime = new TimeSpan(1,52,0), Genres = "Adventure, Family, Fantasy", AverageRating = 3.48 });


                UserRatings.Add(new UserMovieRating {  MovieId = 3, MovieTitle = "Rio", UserName = "John", UserRatingValue = 3.25 });
                UserRatings.Add(new UserMovieRating {  MovieId = 4, MovieTitle = "The Mummy", UserName = "John", UserRatingValue = 4.66 });
                UserRatings.Add(new UserMovieRating {  MovieId = 7, MovieTitle = "Jurassic Park", UserName = "John", UserRatingValue = 4.88 });

                UserRatings.Add(new UserMovieRating { MovieId = 3, MovieTitle = "Rio", UserName = "Tom", UserRatingValue = 4.25 });
                UserRatings.Add(new UserMovieRating { MovieId = 4, MovieTitle = "The Mummy", UserName = "Tom", UserRatingValue = 4.66 });
                UserRatings.Add(new UserMovieRating { MovieId = 20, MovieTitle = "Dumbo", UserName = "Tom", UserRatingValue = 4.66 });
                UserRatings.Add(new UserMovieRating { MovieId = 7, MovieTitle = "Jurassic Park", UserName = "Tom", UserRatingValue = 3.88 });
                UserRatings.Add(new UserMovieRating { MovieId = 19, MovieTitle = "Zootopia", UserName = "Tom", UserRatingValue = 3.88 });

                Users.Add(new User { Name = "John" });
                Users.Add(new User { Name = "Tom" }); 
                SaveChanges(); 
            }         
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer(connectionString);
            //}
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMovieRating>()
            .HasKey(u => new { u.MovieId, u.UserName });


            modelBuilder.Entity<UserMovieRating>()
            .HasOne(u => u.Movie)
            .WithMany(m => m.UserRatings);

            modelBuilder.Entity<UserMovieRating>()
            .HasOne(u => u.User)
            .WithMany(u=> u.MovieRatings);

            base.OnModelCreating(modelBuilder);
        }


        public void InsertOrUpdate(UserMovieRating entity)
        {
            var rowsAffected = Database.ExecuteSqlCommand($"UPDATE UserRating SET UserRatingValue = {entity.UserRatingValue} WHERE MovieId = {entity.MovieId} and UserName = '{entity.UserName}'");
            if (rowsAffected == 0)
            {
                //execute insert
            }
            // If an immediate save is needed, can be slow though
            // if iterating through many entities:
            SaveChanges();
        }
    }


}
