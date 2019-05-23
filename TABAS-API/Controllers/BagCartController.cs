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
            // Expected JSON: '{"brand":"XXX", "model": XXXX}'
            return Ok(AdminSQLHandler.CreateBagCart(JsonConvert.DeserializeObject<BagCart>(bc_data)));
        }

        [HttpPost, Route("tabas/bagcart/brands/new")]
        public IHttpActionResult InsertNewBrand([FromBody] string model)
        {
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
            return Ok(AdminSQLHandler.GetAllBagCartBrands()); // OUT JSON: {brands: [A,B,C,...]}
        }
    }
}
