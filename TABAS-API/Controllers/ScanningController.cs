using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TABAS_API.SQLHandlers;

namespace TABAS_API.Controllers
{
    public class ScanningController : ApiController
    {
        /// <summary>
        /// Obtiene la lista de maletas.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/baggage")]
        public IHttpActionResult GetBaggageList()
        {
            return Ok(MobileAppSQLHandler.GetAllBaggage());
        }

        [HttpGet, Route("tabas/scan/{suitcase}")]
        public IHttpActionResult ScanBaggage([FromUri] int suitcase)
        {
            return Ok(MobileAppSQLHandler.ScanBaggage());
        }
    }
}
