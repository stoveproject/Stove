using System.Web.Http;

namespace Stove.Demo.WebApi.Controllers
{
    [RoutePrefix("")]
    public class HelpController : ApiController
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Index()
        {
            return Redirect($"{Request.RequestUri.AbsoluteUri}help/index");
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("healthcheck")]
        public IHttpActionResult Ping()
        {
            return Ok();
        }
    }
}
