using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Text;
using WebAPI.Controllers;
using Xunit;

namespace WebApi_Tests
{
    public class MoviesControllerTest
    {
       
        public MoviesControllerTest()
        {            
        }

        #region Constructor Unit tests
        [Fact]
        public void MoviesControllerContructor_DbCtx_Log_Null_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(null, null));
        }

        [Fact]
        public void MoviesControllerContructor_Log_Null_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(null, new MockMovieDbContext()));
        }

        [Fact]
        public void MoviesControllerContructor_Test()
        {
            MoviesController moviesController = new MoviesController(new Microsoft.Extensions.Logging.LoggerFactory(), new MockMovieDbContext());
            Assert.NotNull(moviesController);
        } 
        #endregion

        

    }
}
