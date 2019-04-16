using System;
using System.Collections.Generic;
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

        [Fact]
        public void MoviesControllerContructor_DbCtx_Log_Null_Test()
        {
            Assert.Throws<ArgumentNullException>(() => new MoviesController(null, null));
        }

        //[Fact]
        //public void MoviesControllerContructor_DbCtx_Null_Test()
        //{
        //    Assert.Throws<ArgumentNullException>(() => new MoviesController(null, new Logf));
        //}
    }
}
