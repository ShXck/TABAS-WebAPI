using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TABAS_API.SQLHandlers;

namespace TABAS_API.Controllers
{
    public class ReportsController : ApiController
    {
        /// <summary>
        /// Obtiene el conteo de maletas por cliente.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/reports/baggage")]
        public IHttpActionResult GetBaggageByClient()
        {
            /// NO JSON NEEDED
            return Ok(ReportSQLHandler.GetBaggageByUser());

            // OUTPUT JSON "{"http_result": 1,"baggage": ["{"user": "NAME","count": X}",  "{"user": "NAME","count": X}", ...]}"
        }
    }
}
