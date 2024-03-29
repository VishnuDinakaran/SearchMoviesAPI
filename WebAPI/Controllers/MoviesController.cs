﻿using System;
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
        #region Private read-only properties        
        private readonly ILogger<MoviesController> _logger;
        readonly IMovieDAL _movieDAL;
        private static List<string> _queryPropertyNames = new List<string>();
        #endregion

        #region Private constans
        const string BadSearchReqErrorMessage = "Bad Search Request received. Please verify query obj.";
        #endregion

        #region Constructor
        /// <summary>
        /// Construct new MoviesController
        /// </summary>
        /// <param name="moviesDbContext"></param>
        /// <param name="loggerFactory"></param>
        public MoviesController(ILoggerFactory loggerFactory, IMovieDAL movieDAL)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            if (movieDAL == null)
            {
                throw new ArgumentNullException(nameof(movieDAL));
            }

            _logger = loggerFactory.CreateLogger<MoviesController>();
            _logger.LogDebug("Constructed new instance on MoviesController");
            _movieDAL = movieDAL;
        }
        #endregion

        #region GET methods- Search Movies, UserRating etc
        // GET api/movies
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
            stringBuilder.AppendLine(@"                curl -H ""Content-Type: application/json"" -X GET -d ""{ 'RequestId': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'Query': { 'PropertyName': 'UserName', 'operator': '==', 'value': 'Tom' } }"" https://localhost:44371/api/Movies/SearchTop5MoviesByOneUserRating -i");


            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine("(API D) - Update User Ratting");
            stringBuilder.AppendLine(@"                curl -H ""Content-Type: application/json"" -X PUT -d ""{ 'Id': '278C48BE-27E4-497B-BFD1-80404FD67C97', 'UserRating': { 'MovieId': '4', 'MovieTitle':'The Mummy' , 'UserName':'John', 'UserRatingValue': 2.2} }"" https://localhost:44371/api/Movies/UpdateUserRating -i");



            return stringBuilder.ToString();
        }

        [HttpGet("SearchMovies")]
        public ActionResult<IEnumerable<IMovie>> SearchMovies(SearchRequest request)
        {
            try
            {
                if (!IsSearchRequestValid(request))
                {
                    _logger.LogError(BadSearchReqErrorMessage, request);
                    return BadRequest(BadSearchReqErrorMessage);
                }
                _logger.LogDebug($"Received Request Obj: {request.ToString()}");

                StringBuilder resultStr = new StringBuilder(request.ToString());
                //Search Database          
                List<IMovie> result = _movieDAL.Movies
                   .Where(QueryBuilder.GetCompiledFunction<Movie>(request.Query))
                   .OrderBy(x => x.Title)
                   .Select<IMovie, IMovie>(SelectMovie(resultStr))
                   .ToList();

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
                }

                _logger.LogDebug("Search Result:", resultStr.ToString());
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR in SearchMovies: {ex}");
                var result = new ObjectResult("Error processing request. Internal Server error.")
                {
                    StatusCode = (int)Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
                };
                return result;
            }
        }


        [HttpGet("SearchTop5MoviesByUserAverageRating")]
        public ActionResult<Movie> SearchTop5MoviesByUserAverageRating(SearchRequest request)
        {
            try
            {
                if (!IsSearchRequestValid(request))
                {
                    _logger.LogError(BadSearchReqErrorMessage, request);
                    return BadRequest(BadSearchReqErrorMessage);
                }
                _logger.LogDebug($"Received Request Obj: {request.ToString()}");

                StringBuilder resultStr = new StringBuilder(request.ToString());

                List<IMovie> result = _movieDAL.Movies
                    .Where(QueryBuilder.GetCompiledFunction<Movie>(request.Query))
                    .OrderByDescending(x => x.AverageRating)
                    .ThenBy(x => x.Title)
                    .Take(5)
                    .Select(SelectMovie(resultStr))
                    .ToList();

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


                }
                _logger.LogDebug("Top 5 Move by User Avg Rating Result:", resultStr.ToString());
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR in SearchTop5MoviesByUserAverageRating: {ex}");
                var result = new ObjectResult("Error processing request. Internal Server error.")
                {
                    StatusCode = (int)Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
                };
                return result;
            }
        }

        [HttpGet("SearchTop5MoviesByOneUserRating")]
        public ActionResult<IEnumerable<IUserMovieRating>> SearchTop5MoviesByOneUserRating(SearchRequest request)
        {
            try
            {
                if (!IsSearchRequestValid(request))
                {
                    _logger.LogError(BadSearchReqErrorMessage, request);
                    return BadRequest(BadSearchReqErrorMessage);
                }
                _logger.LogDebug($"Received Request Obj: {request.ToString()}");

                StringBuilder resultStr = new StringBuilder(request.ToString());

                List<IUserMovieRating> result = _movieDAL.UserRatings.Include(x => x.Movie)
                    .Where(QueryBuilder.GetCompiledFunction<UserMovieRating>(request.Query))
                    .OrderByDescending(x => x.UserRatingValue)
                    .ThenBy(x => x.MovieTitle)
                    .Take(5)
                    .Select(SelectUserMovieRating(resultStr))
                    .ToList();


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
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR in SearchTop5MoviesByOneUserRating: {ex}");
                var result = new ObjectResult("Error processing request. Internal Server error.")
                {
                    StatusCode = (int)Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
                };
                return result;
            }
        }

        [HttpGet("Top5MoviesByUserRating")]
        public ActionResult<IEnumerable<IMovie>> Top5MoviesByUserRating()
        {
            try
            {
                StringBuilder resultStr = new StringBuilder();

                List<IMovie> result = _movieDAL.Movies
                    .OrderByDescending(x => x.AverageRating)
                    .ThenBy(x => x.Title)
                    .Take(5)
                    .Select<IMovie, IMovie>(SelectMovie(resultStr))
                    .ToList();

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
                }
                _logger.LogDebug("Top 5 Move by User Avg Rating Result:", resultStr.ToString());
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR in Top5MoviesByUserRating: {ex}");
                var result = new ObjectResult("Error processing request. Internal Server error.")
                {
                    StatusCode = (int)Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
                };
                return result;
            }
        }
        #endregion

        #region PUT method to update user rating
        [HttpPut("UpdateUserRating")]
        public ActionResult<string> UpdateUserRating(UpdateUserRatingRequest updateUserRatingRequest)
        {
            try
            {
                if (updateUserRatingRequest == null || updateUserRatingRequest.UserRating == null)
                {
                    _logger.LogDebug("Bad Request received in UpdateUserRating");
                    return BadRequest("Bad Request received in UpdateUserRating");
                }

                bool updateMovieAvg = false;
                //EntityEntry<UserMovieRating> entityEntry = null;
                //If the UserRating exist for a move by a existing user, just update it
                if (_movieDAL.UserRatings.Any(x => updateUserRatingRequest.UserRating.MovieId == x.MovieId && updateUserRatingRequest.UserRating.UserName == x.UserName))
                {
                    _logger.LogDebug($"Found User Rating for User:{updateUserRatingRequest.UserRating.UserName} and movie:{updateUserRatingRequest.UserRating.MovieTitle}");
                    //entityEntry = _movieDAL.UserRatings.Update(updateUserRatingRequest.UserRating);
                    _movieDAL.UpdateUserRatings(updateUserRatingRequest.UserRating);
                    updateMovieAvg = true;
                }
                else
                {
                    //check and find if the Move exist
                    if (!_movieDAL.Movies.Any(x => x.Id == updateUserRatingRequest.UserRating.MovieId))
                    {
                        //404 not found
                        _logger.LogDebug($"Movie:{updateUserRatingRequest.UserRating.MovieTitle}, NOT FOUND");
                        return NotFound($"Movie not found. id:{updateUserRatingRequest.UserRating.MovieId}, Title:{updateUserRatingRequest.UserRating.MovieTitle}");
                    }
                    else if (!_movieDAL.Users.Any(u => u.Name == updateUserRatingRequest.UserRating.UserName))
                    {
                        //404 not found
                        _logger.LogDebug($"User:{ updateUserRatingRequest.UserRating.UserName} , NOT FOUND");
                        return NotFound($"User not found. UserName:{updateUserRatingRequest.UserRating.UserName}");
                    }
                    else
                    {
                        _logger.LogDebug($"Inserting new User Rating for User:{updateUserRatingRequest.UserRating.UserName} and movie:{updateUserRatingRequest.UserRating.MovieTitle}");
                        //entityEntry = _movieDAL.UserRatings.Add(updateUserRatingRequest.UserRating);
                        _movieDAL.AddUserRatings(updateUserRatingRequest.UserRating);
                        updateMovieAvg = true;
                    }
                }
                //Saving
                _movieDAL.SaveChanges();

                //reaches this line since all data sets are availabe
                //Update Movie avg user rating
                if (updateMovieAvg)
                {
                    UpdateMovieAvgUserRatingOnUserRatingUpdate(updateUserRatingRequest);
                }

                _logger.LogDebug($"Saved User Rating for User:{updateUserRatingRequest.UserRating.UserName} and movie:{updateUserRatingRequest.UserRating.MovieTitle}");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR in UpdateUserRating: {ex}");
                var result = new ObjectResult("Error processing request. Internal Server error.")
                {
                    StatusCode = (int)Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError
                };
                return result;
            }
        }
        #endregion

        #region Private Methods
        private Func<IMovie, IMovie> SelectMovie(StringBuilder resultStr)
        {
            return (x) =>
            {
                resultStr?.AppendLine(x.ToString());
                return new MovieSummay
                {
                    Id = x.Id,
                    Title = x.Title,
                    Genres = x.Genres,
                    YearOfRelease = x.YearOfRelease,
                    RunningTime = x.RunningTime,
                    AverageRating = RoundDouble(x.AverageRating)
                };
            };
        }

        private Func<IUserMovieRating, IUserMovieRating> SelectUserMovieRating(StringBuilder resultStr)
        {
            return (x) =>
            {
                resultStr?.AppendLine(x.ToString());
                return new UserMovieRatingSummary
                {
                    UserName = x.UserName,
                    MovieId = x.MovieId,
                    MovieTitle = x.MovieTitle,
                    UserRatingValue = RoundDouble(x.UserRatingValue)
                };
            };
        }


        private void UpdateMovieAvgUserRatingOnUserRatingUpdate(UpdateUserRatingRequest updateUserRatingRequest)
        {
            //Get over all Users Rating Avg for the movie
            double movieAvgUserRating = _movieDAL.UserRatings.Where(x => x.MovieId == updateUserRatingRequest.UserRating.MovieId).Average(x => x.UserRatingValue);
            //Get Movie to update the AvgUserRating field with new value
            Movie movie = _movieDAL.Movies.FirstOrDefault(x => x.Id == updateUserRatingRequest.UserRating.MovieId);
            //Update  movieAvgUserRating and save
            _movieDAL.UpdateMovieAvgUserRaing(movie, movieAvgUserRating);
        }

        private double RoundDouble(double val)
        {
            return Math.Round(val, 1);
        }

        private bool IsSearchRequestValid(SearchRequest request)
        {
            lock (_queryPropertyNames)
            {
                if (!_queryPropertyNames.Any())
                {
                    _queryPropertyNames.AddRange(typeof(MovieSummay).GetProperties().Select(x => x.Name));
                    _queryPropertyNames.AddRange(typeof(User).GetProperties().Select(x => x.Name));
                    _queryPropertyNames.AddRange(typeof(UserMovieRating).GetProperties().Select(x => x.Name));
                }
            }

            return ModelState.IsValid && request != null && request.Query != null
                //|| (request.Query.Queries != null && request.Query.Queries.Any(x => string.IsNullOrWhiteSpace(x.Junction)))
                && IsQueryValid(request.Query);
        }

        List<string> _queryOperators = new List<string> { "==", "!=", "Equals", "NOTEQUALS", "<", "LESSTHAN", "<=", "LESSTHANOREQUAL", ">", "GREATERTHAN", ">=", "GREATERTHANOREQUAL", "STRINGCONTAINS" };

        public bool IsQueryValid(Query query)
        {
            bool result = _queryPropertyNames.Any(x => string.Compare(x, query.PropertyName) == 0)
                && _queryOperators.Any(x => string.Compare(query.Operator, x, true) == 0);

            if (result && query.Queries != null)
            {
                result = !query.Queries.Any(x => string.IsNullOrWhiteSpace(x.Junction) || (IsQueryValid(x) == false));
            }

            return result;
        }
        #endregion
    }
}
