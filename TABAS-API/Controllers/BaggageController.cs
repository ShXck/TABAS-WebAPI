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
    public class BaggageController : ApiController
    {
        [HttpGet, Route("tabas/colors")]
        public IHttpActionResult GetColors()
        {
            // No JSON needed.
            return Ok(AdminSQLHandler.GetColors()); 
        }

        /// <summary>
        /// Crea una nueva maleta en el sistema.
        /// </summary>
        /// <param name="bagg_data">La información de la maleta.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/baggage/create")]
        public IHttpActionResult CreateBaggage([FromBody] string bagg_data)
        {
            //Expected JSON: '{"username":"XXX", "weight":"XX.XX", "color": "XXX"}'
            return Ok(AdminSQLHandler.InsertNewBaggage(JsonConvert.DeserializeObject<BaggageDTO>(bagg_data)));
        }
    }
}
