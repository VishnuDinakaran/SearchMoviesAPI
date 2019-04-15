using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (id > 5)
            {
                return Ok($"value for {id}");
            }
            else
            {
                return NotFound($"Object NotFound for requested id:{id}");
            }

        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }



        [HttpGet("all")]
        public ActionResult<IEnumerable<Person>> GetAll()
        {
            return new[]
            {
            new Person { FirstName = "Ana" },
            new Person { FirstName = "Felipe" },
            new Person { FirstName = "Emillia" }
        };
        }

        [HttpPost("create")]
        public ActionResult<string> Create(Person person)
        {
            Debug.WriteLine(person);
            Debug.WriteLine($"Received Person: {person}");
            return Accepted(person);
        }

        [HttpGet("GetPerson")]
        public ActionResult<string> GetPerson(Person person)
        {
            Debug.WriteLine(person);
            Debug.WriteLine($"Received Get Person: {person.FirstName}");
            return Accepted(person);
        }


        [HttpGet("SearchPerson")]
        public ActionResult<string> SearchPerson(RequestObject request)
        {

            Debug.WriteLine($"Received Request Obj:");
            Debug.WriteLine($"{request.ToString()}");
            return Ok(request.ToString());
        }
    }




    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        //public int Age { get; set; }
    }


}