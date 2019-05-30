using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TABAS_API.Controllers
{
    public class ReportsController : ApiController
    {
        public IHttpActionResult GetBaggageByClient()
        {
            return Ok();
        }
    }
}
