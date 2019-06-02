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

        /// <summary>
        /// Obtiene los datos relacionados al equipaje en un vuelo.
        /// </summary>
        /// <returns>Tablas de datos de equipaje por vuelo</returns>
        [HttpGet, Route("tabas/report/baggage/information/{flight}")]
        public IHttpActionResult GetBaggageInformation([FromUri] int flight)
        {
            // NO JSON NEEDED.
            return Ok(ReportSQLHandler.GetBaggageReport(flight)); // OUTPUT JSON: "{"http_result": 1,"flight": X,"model": "XXXX","weight": XXX.X,"total_suitcase": XXX,"suitcase_rejected": XX,"suitcase_acepted": XX}"
        }
    }
}
