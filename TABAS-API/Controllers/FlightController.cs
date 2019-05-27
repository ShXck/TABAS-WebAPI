using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TABAS_API.DataObjects;
using TABAS_API.Objects;
using TABAS_API.SQLHandlers;

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
            return Ok(AdminSQLHandler.GetAllPlanes()); // OUT JSON: {airplanes: [A,B,C,...], http_result: X}
        }

        /// <summary>
        /// Obtiene una lista de los id de los vuelos existentes.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/flights")]
        public IHttpActionResult GetFlights()
        {
            // NO JSON NEEDED
            return Ok(AdminSQLHandler.GetAllFlights()); // OUTPUT JSON: {flights: [1,2,3, ...], http_result: X}
        }

        /// <summary>
        /// Asigna un Bagcart a un vuelo.
        /// </summary>
        /// <param name="data">La información para la asignación</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/flights/bagcart/assign")]
        public IHttpActionResult AssignBagcartToFlight([FromBody] string data)
        {
            // JSON EXPECTED: {"flight_id": X, "bg_brand": XXXX}
            return Ok(AdminSQLHandler.AssignBagcart(JsonConvert.DeserializeObject<FlightBagCartDTO>(data)));
        }

        /// <summary>
        /// Obtiene las secciones de un avión.
        /// </summary>
        /// <param name="flight">El id del vuelo del que se quieren obtener las secciones de avión.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/{flight}/sections")]
        public IHttpActionResult GetFlightSections([FromUri] int flight)
        {
            /// NO JSON NEEDED.
            return Ok(MobileAppSQLHandler.GetFlightPlaneSections(flight)); // OUTPUT JSON: {"sections": [1,2,3,...], "http_result": X}
        }

        [HttpPost, Route("tabas/section/assign")]
        public IHttpActionResult AssignBagToSection([FromBody] string data)
        {
            // JSON EXPECTED: {"flight_id": X, "section_id": X, "suitcase_id": X, "user_id": X}
            return Ok();
        }
    }
}
