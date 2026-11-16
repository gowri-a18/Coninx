using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DemoApi.Controllers
{
    [Authorize]
    public class HomeController : ApiController
    {

        [HttpGet]
        [Route("Api/Home/GetData")]
        public string GetData()
        {
            return "sucess";
        }
    }
}
