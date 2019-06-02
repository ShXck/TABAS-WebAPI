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
        /// Obtiene una lista de los id de los vuelos existentes que no tienen un bagcart asignado.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/flights/unassigned")]
        public IHttpActionResult GetUnassignedFlights()
        {
            // NO JSON NEEDED
            return Ok(AdminSQLHandler.GetUnassignedFlights()); // OUTPUT JSON: {flights: [1,2,3, ...], http_result: X}
        }

        /// <summary>
        /// Obtiene los vuelos que ya tienen un bagcart asignado y que el vuelo no ha sido cerrado. Es decir, que el 
        /// código del bagcart no ha sido generado.
        /// </summary>
        /// <returns>La lista de vuelos activos.</returns>
        [HttpGet, Route("tabas/flights/active")]
        public IHttpActionResult GetActiveFlights()
        {
            /// NO JSON NEEDED
            return Ok(AdminSQLHandler.GetAllActiveFlights()); // OUTPUT JSON: {flights: [1,2,3, ...], http_result: X}
        }

        /// <summary>
        /// Asigna un Bagcart a un vuelo.
        /// </summary>
        /// <param name="data">La información para la asignación</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/flights/bagcart/assign")]
        public IHttpActionResult AssignBagcartToFlight([FromBody] string data)
        {
            // JSON EXPECTED: {"flight_id": X, "bagcart_id": XXXX}
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

        /// <summary>
        /// Asigna una maleta a una sección del avión correspondiente al vuelo.
        /// </summary>
        /// <param name="data">La información de maleta, avión, y vuelo.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/section/assign")]
        public IHttpActionResult AssignBagToSection([FromBody] string data)
        {
            // JSON EXPECTED: {"flight_id": X, "section_id": X, "suitcase_id": X}
            return Ok(MobileAppSQLHandler.AssignBaggageToSection(JsonConvert.DeserializeObject<BagToSectionDTO>(data)));
        }

        /// <summary>
        /// Asigna o actualiza el avión de un vuelo existente.
        /// </summary>
        /// <param name="data">La información del vuelo y avión.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/flight/plane/assign")]
        public IHttpActionResult AssignPlane([FromBody] string data)
        {
            // JSON EXPECTED: {"flight":X, "model": "XXXX"}
            return Ok(AdminSQLHandler.AssignPlaneToFlight(JsonConvert.DeserializeObject<AssignPlaneDTO>(data)));
        }


    }
}
