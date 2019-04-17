using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using WebAPI.Controllers;
using WebAPI.Entities;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace WebApi_Tests
{
    public class MoviesControllerTest
    {
       

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

        [Theory]
        [InlineData("Id","==","3", 1)]
        [InlineData("Title","stringcontains","2", 1)]
        [InlineData("Title","==","Rio2", 1)]       
        [InlineData("Title", "stringcontains", "Rio", 2)]
        public void MoviesSearch_Test(string propertyName, string operatorStr, string value, int count)
        {
            MoviesController moviesController = new MoviesController(new Microsoft.Extensions.Logging.LoggerFactory(), new MockMovieDbContext());
            SearchRequest searchRequest = new SearchRequest { Query = new Query { PropertyName = propertyName, Operator = operatorStr, Value = value } };

            var result = moviesController.SearchMovies(searchRequest);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            OkObjectResult ok = result.Result as OkObjectResult;
            Assert.IsAssignableFrom<IEnumerable<IMovie>>(ok.Value);
            Assert.True(((IEnumerable<IMovie>)ok.Value).Count() == count);
        }

        [Theory]
        [InlineData("UserName", "==", "Tom", typeof(OkObjectResult))]
        [InlineData("UserName", "==", "Tom1", typeof(NotFoundObjectResult))]
        [InlineData("UserName", "=", "Tom1", typeof(BadRequestObjectResult))]
        [InlineData("UserName1", "=", "Tom1", typeof(BadRequestObjectResult))]
        public void SearchTop5MoviesByOneUserRating_Test(string propertyName, string operatorStr, string value, Type resultType)
        {
            MoviesController moviesController = new MoviesController(new Microsoft.Extensions.Logging.LoggerFactory(), new MockMovieDbContext());
            SearchRequest searchRequest = new SearchRequest { Query = new Query { PropertyName = propertyName, Operator = operatorStr, Value = value } };

            var result = moviesController.SearchTop5MoviesByOneUserRating(searchRequest);

            Assert.NotNull(result);
            Assert.IsType(resultType, result.Result);
            //OkObjectResult ok = result.Result as OkObjectResult;
            //Assert.IsAssignableFrom<IEnumerable<IMovie>>(ok.Value);
        }

    }
}
