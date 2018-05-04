using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PolitiekeBarometer_MVC.Controllers
{
    public class AndroidApiController : ApiController
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "User1", "User2" };
        }
    }
}
