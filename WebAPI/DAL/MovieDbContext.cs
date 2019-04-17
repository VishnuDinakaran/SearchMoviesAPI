using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Entities;

namespace WebAPI.DAL
{
    /// <summary>
    /// DataAccesslayer for Movies data store
    /// </summary>
    public class MovieDbContext : DbContext
    {
        private static object _lock = new object();
        #region Data Sets
        public DbSet<Movie> Movies { get; set; }
        public DbSet<UserMovieRating> UserRatings { get; set; }
        public DbSet<User> Users { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Construct Movies DbContext
        /// </summary>
        /// <param name="options"></param>
        public MovieDbContext(DbContextOptions options) 
            : base(options)
        {
            lock (_lock)
            {
                if (Movies != null && !Movies.Any())
                {
                    Movies.AddRange(TestData.Movies);
                    Users.AddRange(TestData.Users);
                    UserRatings.AddRange(TestData.UserMovieRatings);
                    SaveChanges();
                } 
            }
        }

        #endregion

        #region OnModelCreating 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Set composit key for UsrMoveRating
            modelBuilder.Entity<UserMovieRating>()
            .HasKey(u => new { u.MovieId, u.UserName });

            //Set relationship
            modelBuilder.Entity<UserMovieRating>()
            .HasOne(u => u.Movie)
            .WithMany(m => m.UserRatings);

            modelBuilder.Entity<UserMovieRating>()
            .HasOne(u => u.User)
            .WithMany(u => u.MovieRatings);

            base.OnModelCreating(modelBuilder);
        } 
        #endregion

    }
}
