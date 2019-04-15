using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using WebAPI.DAL;
using WebAPI.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : Controller
    {
        private readonly MovieDbContext _moviesDbContext;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(MovieDbContext moviesDbContext, ILoggerFactory loggerFactory)
        {
            _moviesDbContext = moviesDbContext;
            _logger = loggerFactory.CreateLogger<MoviesController>();
            _logger.LogDebug("Constructed new instance on MoviesController");
        }
        // GET api/moviess
        [HttpGet]
        public ActionResult<string> Get()
        {
            StringBuilder stringBuilder = new StringBuilder("Available methods on Movies API");
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine(" ");

            stringBuilder.AppendLine("(API A) - SearchMovies");
            stringBuilder.AppendLine(@" Eg 1             curl -H ""Content-Type: application/json"" -X GET -d ""{ 'RequestId':'278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query':{ 'PropertyName':'Title', 'operator':'==', 'value':'Rio', 'Queries': [{'PropertyName':'YearOfRelease', 'operator':'==', 'value':'2011', 'Junction':'And', 'Index':'2'}, {'PropertyName':'Genres', 'operator':'!=', 'value':'Action', 'Junction':'And', 'Index':'1'}]}}"" https://localhost:44371/api/Movies/SearchMovies -i");
            stringBuilder.AppendLine(@" Eg 2             curl -H ""Content-Type: application/json"" -X GET -d ""{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'Genres', 'operator': 'stringcontains', 'value': 'Action', 'Queries': [ { 'PropertyName': 'YearOfRelease', 'operator': '>', 'value': '1996', 'Junction': 'AND' } ] } }"" https://localhost:44371/api/Movies/SearchMovies -i");

            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine("(API B) - SearchTop5MoviesByUserAverageRating");
            stringBuilder.AppendLine(@"                curl -H ""Content-Type: application/json"" -X GET -d ""{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'Genres', 'operator': 'stringcontains', 'value': 'Action', 'Queries': [ { 'PropertyName': 'YearOfRelease', 'operator': '>', 'value': '1993', 'Junction': 'AND' } ] } }"" https://localhost:44371/api/Movies/SearchTop5MoviesByUserAverageRating -i");
            
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine("(API C) - SearchTop5MoviesByOneUserRating");
            stringBuilder.AppendLine(@"                curl - H ""Content-Type: application/json"" - X GET - d ""{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'UserName', 'operator': '==', 'value': 'Tom' } }"" https://localhost:44371/api/Movies/SearchTop5MoviesByOneUserRating -i");


            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine("(API D) - Update User Ratting");
            stringBuilder.AppendLine(@"                curl -H ""Content-Type: application/json"" -X PUT -d ""{ 'Id': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'UserRating': { 'MovieId': '4', 'MovieTitle':'The Mummy' , 'UserName':'John', 'UserRatingValue': 2.2} }"" https://localhost:44371/api/Movies/UpdateUserRating -i");



            return stringBuilder.ToString();
        }

        [HttpGet("SearchMovies")]
        public ActionResult<string> SearchMovies(SearchRequest request)
        {
            if (request == null || request.Query == null || (request.Query.Queries != null && request.Query.Queries.Any(x => string.IsNullOrWhiteSpace(x.Junction))))
            {
                _logger.LogError("Bad Search Request", request);
                return BadRequest("Bad Search Request");
            }
            _logger.LogDebug($"Received Request Obj: {request.ToString()}");

            StringBuilder resultStr = new StringBuilder(request.ToString());

            var result = _moviesDbContext.Movies
                .Where(QueryBuilder.GetCompiledFunction<Movie>(request.Query))
                .OrderBy(x => x.Title).ToList();

            if (result != null)
            {
                if (!result.Any())
                {
                    resultStr.AppendLine(" ");
                    resultStr.AppendLine(" NO MOVIE FOUND. ");
                    //When no results found return 404 Not Found
                    _logger.LogDebug("No result found for given search criteria");
                    return NotFound(resultStr);
                }
                else
                {
                    resultStr.AppendLine($"Serach Result: Count {result.Count}");
                }

                resultStr.AppendLine(" ");
                foreach (var item in result)
                {
                    item.AverageRating = RoundDouble(item.AverageRating);
                    resultStr.AppendLine(item.ToString());
                }
            }
            _logger.LogDebug("Search Result:", resultStr.ToString());
            return Ok(resultStr.ToString());
        }

        [HttpGet("Top5MoviesByUserRating")]
        public ActionResult<string> Top5MoviesByUserRating()
        {
            StringBuilder resultStr = new StringBuilder();

            List<Movie> result = _moviesDbContext.Movies.OrderByDescending(x => x.AverageRating).ThenBy(x=> x.Title).Take(5).ToList();

            if (result != null)
            {
                if (!result.Any())
                {
                    resultStr.AppendLine(" ");
                    resultStr.AppendLine(" NO MOVIE FOUND. ");
                    //When no results found return 404 Not Found
                    _logger.LogDebug("No result found for given search criteria");
                    return NotFound(resultStr);
                }
                else
                {
                    resultStr.AppendLine($"Serach Result: Count {result.Count}");
                }

                resultStr.AppendLine(" ");
                foreach (var item in result)
                {
                    item.AverageRating = RoundDouble(item.AverageRating);
                    resultStr.AppendLine(item.ToString());
                }
            }
            _logger.LogDebug("Top 5 Move by User Avg Rating Result:", resultStr.ToString());
            return Ok(resultStr.ToString());
        }

        [HttpGet("SearchTop5MoviesByUserAverageRating")]
        public ActionResult<string> SearchTop5MoviesByUserAverageRating(SearchRequest request)
        {
            if (request == null || request.Query == null || (request.Query.Queries != null && request.Query.Queries.Any(x => string.IsNullOrWhiteSpace(x.Junction))))
            {
                _logger.LogError("Bad Search Request", request);
                return BadRequest("Bad Search Request");
            }
            _logger.LogDebug($"Received Request Obj: {request.ToString()}");

            StringBuilder resultStr = new StringBuilder(request.ToString());
            
            List<Movie> result = _moviesDbContext.Movies
                .Where(QueryBuilder.GetCompiledFunction<Movie>(request.Query))
                .OrderByDescending(x => x.AverageRating)
                .ThenBy(x => x.Title)
                .Take(5).ToList();

            if (result != null)
            {
                if (!result.Any())
                {
                    resultStr.AppendLine(" ");
                    resultStr.AppendLine(" NO MOVIE FOUND. ");
                    //When no results found return 404 Not Found
                    _logger.LogDebug("No result found for given search criteria");
                    return NotFound(resultStr);
                }
                else
                {
                    resultStr.AppendLine($"Serach Result: Count {result.Count}");
                }

                resultStr.AppendLine(" ");
                foreach (var item in result)
                {
                    item.AverageRating = RoundDouble(item.AverageRating);
                    resultStr.AppendLine(item.ToString());
                }
            }
            _logger.LogDebug("Top 5 Move by User Avg Rating Result:", resultStr.ToString());
            return Ok(resultStr.ToString());
        }


        [HttpGet("SearchTop5MoviesByOneUserRating")]
        public ActionResult<string> SearchTop5MoviesByOneUserRating(SearchRequest request)
        {
            if (request == null || request.Query == null || (request.Query.Queries != null && request.Query.Queries.Any(x => string.IsNullOrWhiteSpace(x.Junction))))
            {
                _logger.LogError("Bad Search Request", request);
                return BadRequest("Bad Search Request");
            }
            _logger.LogDebug($"Received Request Obj: {request.ToString()}");

            StringBuilder resultStr = new StringBuilder(request.ToString());

            List<UserMovieRating> result = _moviesDbContext.UserRatings.Include(x => x.Movie)
                .Where(QueryBuilder.GetCompiledFunction<UserMovieRating>(request.Query))
                .OrderByDescending(x => x.UserRatingValue)
                .ThenBy(x => x.MovieTitle)
                .Take(5).ToList();


            if (result != null)
            {
                if (!result.Any())
                {
                    resultStr.AppendLine(" ");
                    resultStr.AppendLine(" NO MOVIE FOUND. ");
                    //When no results found return 404 Not Found
                    _logger.LogDebug("No result found for given search criteria");
                    return NotFound(resultStr);
                }
                else
                {
                    resultStr.AppendLine($"Serach Result: Count {result.Count}");
                }

                resultStr.AppendLine(" ");
                foreach (var item in result)
                {
                    item.UserRatingValue = RoundDouble(item.UserRatingValue);
                    resultStr.AppendLine(item.ToString());
                }
            }
            _logger.LogDebug("Top 5 Move by User Avg Rating Result:", resultStr.ToString());
            return Ok(resultStr.ToString());
        }

        [HttpPut("UpdateUserRating")]
        public ActionResult<string> UpdateUserRating(UpdateUserRatingRequest updateUserRatingRequest)
        {
            if (updateUserRatingRequest == null || updateUserRatingRequest.UserRating == null)
            {
                _logger.LogDebug("Bad Request received in UpdateUserRating");
                return BadRequest();
            }

            bool updateMovieAvg = false;
            EntityEntry<UserMovieRating> entityEntry = null;
            //If the UserRating exist for a move by a existing user, just update it
            if (_moviesDbContext.UserRatings.Any(x => updateUserRatingRequest.UserRating.MovieId == x.MovieId && updateUserRatingRequest.UserRating.UserName == x.UserName))
            {
                _logger.LogDebug($"Found User Rating for User:{updateUserRatingRequest.UserRating.UserName} and movie:{updateUserRatingRequest.UserRating.MovieTitle}");
                entityEntry = _moviesDbContext.UserRatings.Update(updateUserRatingRequest.UserRating);
                updateMovieAvg = true;
            }
            else
            {
                //check and find if the Move exist
                if (!_moviesDbContext.Movies.Any(x => x.Id == updateUserRatingRequest.UserRating.MovieId))
                {
                    //404 not found
                    _logger.LogDebug($"Movie:{updateUserRatingRequest.UserRating.MovieTitle}, NOT FOUND");
                    return NotFound($"Movie not found. id:{updateUserRatingRequest.UserRating.MovieId}, Title:{updateUserRatingRequest.UserRating.MovieTitle}");
                }
                else if (!_moviesDbContext.Users.Any(u => u.Name == updateUserRatingRequest.UserRating.UserName))
                {
                    //404 not found
                    _logger.LogDebug($"User:{ updateUserRatingRequest.UserRating.UserName} , NOT FOUND");
                    return NotFound($"User not found. UserName:{updateUserRatingRequest.UserRating.UserName}");
                }
                else
                {
                    _logger.LogDebug($"Inserting new User Rating for User:{updateUserRatingRequest.UserRating.UserName} and movie:{updateUserRatingRequest.UserRating.MovieTitle}");
                    entityEntry = _moviesDbContext.UserRatings.Add(updateUserRatingRequest.UserRating);
                    updateMovieAvg = true;
                }
            }
            //Saving
            _moviesDbContext.SaveChanges();

            //reaches this line since all data sets are availabe
            //Update Movie avg user rating
            if (updateMovieAvg)
            {
                UpdateMovieAvgUserRatingOnUserRatingUpdate(updateUserRatingRequest);
            }

            _logger.LogDebug($"Saved User Rating for User:{updateUserRatingRequest.UserRating.UserName} and movie:{updateUserRatingRequest.UserRating.MovieTitle}");

            return Ok();
        }

        #region Private Methods
        private void UpdateMovieAvgUserRatingOnUserRatingUpdate(UpdateUserRatingRequest updateUserRatingRequest)
        {
            //Get over all Users Rating Avg for the movie
            double movieAvgUserRating = _moviesDbContext.UserRatings.Where(x => x.MovieId == updateUserRatingRequest.UserRating.MovieId).Average(x => x.UserRatingValue);
            //Get Movie to update the AvgUserRating field with new value
            Movie movie = _moviesDbContext.Movies.FirstOrDefault(x => x.Id == updateUserRatingRequest.UserRating.MovieId);
            movie.AverageRating = movieAvgUserRating;
            _moviesDbContext.Entry(movie).Property("AverageRating").IsModified = true;
            var movieEntry = _moviesDbContext.Entry(movie);
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
            //movieEntry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _moviesDbContext.SaveChanges();
        }

        private double RoundDouble(double val)
        {
            return Math.Round(val, 1);
        } 
        #endregion


    }
}
