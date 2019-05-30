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
    public class BagCartController : ApiController
    {
        [HttpPost, Route("tabas/bagcart/create")]
        public IHttpActionResult CreateBagCart([FromBody] string bc_data)
        {
            // JSON EXPECTED: '{"brand":"XXX", "model": XXXX, "capacity": XXX}'
            return Ok(AdminSQLHandler.CreateBagCart(JsonConvert.DeserializeObject<BagCart>(bc_data)));
        }

        [HttpPost, Route("tabas/bagcart/brands/new")]
        public IHttpActionResult InsertNewBrand([FromBody] string model)
        {
            /// JSON EXPECTED: {"brand": "XXXXX"}
            return Ok(AdminSQLHandler.InsertNewCartBrand(JsonConvert.DeserializeObject<BagCart>(model)));
        }

        /// <summary>
        /// Crea una lista con las marcas de bagcarts.
        /// </summary>
        /// <returns>La lista de marcas.</returns>
        [HttpGet, Route("tabas/bagcart/brands")]
        public IHttpActionResult GetBagCartBrands()
        {
            // NO JSON needed
            return Ok(AdminSQLHandler.GetAllBagCartBrands()); // OUTPUT JSON: {brands: [A,B,C,...]}
        }

        /// <summary>
        /// Genera el código de seguridad para el cierre del bagcart.
        /// </summary>
        /// <param name="flight">El id del vuelo.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/bagcart/{flight}/close")]
        public IHttpActionResult CloseBagcart([FromUri] int flight)
        {
            // NO JSON NEEDED
            return Ok(AdminSQLHandler.CloseBagcart(flight)); // OUTPUT JSON: "{"seal": "xxxxxx", "http_result\": "X"}"
        }

        /// <summary>
        /// Obtiene una lista de todos los bagcarts creados.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        [HttpGet, Route("tabas/bagcarts")]
        public IHttpActionResult GetBagcarts()
        {
            // NO JSON NEEDED
            return Ok(AdminSQLHandler.GetBagcarts()); // OUTPUT JSON: {"bagcarts": [1,2,3,...], "http_result": X}
        }
    }
}
