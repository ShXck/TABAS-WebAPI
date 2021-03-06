﻿using Newtonsoft.Json;
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
        /// Simula el escaneo de una maleta.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/scan/")]
        public IHttpActionResult ScanBaggage()
        {
            // NO JSON NEEDED.
            return Ok(MobileAppSQLHandler.ScanBaggage());
            /**
             * OUTPUT JSON:
             *     "{"http_result": 1,"pass": false,"status": "Rejected"}"

                   "{"http_result": 1,"pass": true,"status": "Accepted"}"
               */
        }

        /// <summary>
        /// Inserta una nueva maleta escaneada.
        /// </summary>
        /// <param name="bagg_details">La información del escaneo.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/scan/baggage")]
        public IHttpActionResult InsertScannedBaggage([FromBody] string bagg_details)
        {
            // JSON Expected: '{"suitcase_id":X, "username": "XXX", "status": "XXX", "comment": "XXXXX"}' if bagggage was rejected;
            // '{"suitcase_id":X, "username": "XXX", "status": "XXX"}' otherwise
            return Ok(MobileAppSQLHandler.InsertScannedBaggage(JsonConvert.DeserializeObject<ScannedBaggDTO>(bagg_details)));
        }

        /// <summary>
        /// Obtiene el nombre de la persona que escaneo una maleta específica.
        /// </summary>
        /// <param name="suit_id">El id de la maleta.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/suitcases/{suit_id}/user")]
        public IHttpActionResult GetBaggageChecker([FromUri] int suit_id)
        {
            // NO JSON NEEDED
            return Ok(MobileAppSQLHandler.GetCheckerUser(suit_id));  // OUTPUT JSON "{"http_result": 1,"user": "XXXX"}"
        }
    }
}
