using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TABAS_API.DataObjects;
using TABAS_API.Objects;

namespace TABAS_API.Controllers
{
    public class FlightController : ApiController
    {
        /// <summary>
        /// Crea un nuevo vuelo.
        /// </summary>
        /// <param name="flight">La información del vuelo.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/flights/new")]
        public IHttpActionResult CreateFlight([FromBody] string flight)
        {
            // JSON Expected: {model:XX}
            return Ok(AdminSQLHandler.CreateNewFlight(JsonConvert.DeserializeObject<FlightDTO>(flight)));
        }

        /// <summary>
        /// Obtiene todos los aviones.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/planes")]
        public IHttpActionResult GetPlanes()
        {
            // NO JSON NEEDED.
            return Ok(AdminSQLHandler.GetAllPlanes()); // OUT JSON: {airplanes: [A,B,C,...]}
        }
    }
}
