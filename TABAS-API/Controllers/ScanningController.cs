using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TABAS_API.DataObjects;
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

        /// <summary>
        /// Simula el escaneo de una maleta.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/scan/")]
        public IHttpActionResult ScanBaggage()
        {
            return Ok(MobileAppSQLHandler.ScanBaggage());
        }

        /// <summary>
        /// Inserta una nueva maleta escaneada.
        /// </summary>
        /// <param name="bagg_details">La información del escaneo.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/scan/baggage")]
        public IHttpActionResult InsertScannedBaggage([FromBody] string bagg_details)
        {
            // JSON Expected: '{"suitcase_id":X, "username": "XXX", "status": "XXX", "comment": "XXXXX"}'
            return Ok(MobileAppSQLHandler.InsertScannedBaggage(JsonConvert.DeserializeObject<ScannedBaggDTO>(bagg_details)));
        }

        /// TODO: Asignar maleta a sección de avión en un vuelo.
    }
}
