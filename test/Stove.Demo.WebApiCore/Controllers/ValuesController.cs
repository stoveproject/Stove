using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

namespace Stove.Demo.WebApiCore.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IStoveCommandContextAccessor _commandContextAccessor;

        public ValuesController(IStoveCommandContextAccessor commandContextAccessor)
        {
            _commandContextAccessor = commandContextAccessor;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2", $"CommandContext:{_commandContextAccessor.CommandContext.CorrelationId}" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
