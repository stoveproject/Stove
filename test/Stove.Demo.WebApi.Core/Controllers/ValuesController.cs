using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using Stove.Demo.WebApi.Core.Domain;

namespace Stove.Demo.WebApi.Core.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : Controller
	{
		private readonly SomeDomainService _domainService;

		public ValuesController(SomeDomainService domainService)
		{
			_domainService = domainService;
		}

		// GET api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			_domainService.DoSomeStuff();
			return "value";
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
	}
}
