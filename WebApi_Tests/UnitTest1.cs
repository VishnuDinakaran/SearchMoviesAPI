using System;
using System.Collections.Generic;
using WebAPI.Controllers;
using WebAPI.Entities;
using Xunit;
using System.Linq;
//using WebAPI.Entities;


namespace WebApi_Tests
{
    public class QueryBuilderTests
    {
        List<Movie> _movies = new List<Movie>();
        public QueryBuilderTests()
        {
            _movies.Add(new Movie { Id = 3, Title = "Rio", YearOfRelease = 2011, Genres = "Animated" });
            _movies.Add(new Movie { Id = 4, Title = "The Mummy", YearOfRelease = 1999, Genres = "Fantasy/Adventure" });
            _movies.Add(new Movie { Id = 5, Title = "Rio2", YearOfRelease = 2014, Genres = "Animated" });
            _movies.Add(new Movie { Id = 6, Title = "The Mummy Returns", YearOfRelease = 2001, Genres = "Fantasy" });

        }

        [Fact]
        public void Test1()
        {
            Query query = new Query { PropertyName = "Title", Operator = "Equals", Value = "Rio" };

            var q = QueryBuilder.GetCompiledFunction<Movie>(query);
            
            var val = _movies.Where(q).ToList();

            Assert.True(2 == val.Count);
        }

        [Fact]
        public void Test2()
        {
            Query query = new Query { PropertyName = "Title", Operator = "Equals", Value = "Rio" };
            query.Queries = new List<Query>();
            query.Queries.Add(new Query { Junction = "And", Index = 1, PropertyName = "Id", Operator = "<", Value = "10" });

            var val = _movies.Where(QueryBuilder.GetCompiledFunction<Movie>(query)).ToList();

            Assert.True(1 == val.Count);
        }

        [Fact]
        public void Test3()
        {
            Query query = new Query { PropertyName = "Title", Operator = "STRINGCONTAINS", Value = "Rio" };
            query.Queries = new List<Query>();
            
            var val = _movies.Where(QueryBuilder.GetCompiledFunction<Movie>(query)).ToList();

            Assert.True(2 == val.Count);
        }



    }
}
