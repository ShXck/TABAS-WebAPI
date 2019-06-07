using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TABAS_API.SQLHandlers;

namespace TABAS_API.Controllers
{
    public class DeleteController : ApiController
    {
        /// <summary>
        /// Elimina un avión de la base de datos.
        /// </summary>
        /// <param name="model">El modelo a eliminar.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/delete/airplane/{model}")]
        public IHttpActionResult DeleteAirplane([FromUri] string model)
        {
            // NO JSON NEEDED.
            return Ok(DeleteSQLHandler.DeleteAirplane(model)); 
        }

        /// <summary>
        /// Elimina un bagcart de la base de datos.
        /// </summary>
        /// <param name="model">El modelo a eliminar.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/delete/bagcart/{id}")]
        public IHttpActionResult DeleteBagcart([FromUri] int id)
        {
            // NO JSON NEEDED.
            return Ok(DeleteSQLHandler.DeleteBagcart(id));
        }

        /// <summary>
        /// Elimina un color de la base de datos.
        /// </summary>
        /// <param name="color">El modelo a eliminar.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/delete/colors/{color}")]
        public IHttpActionResult DeleteColor([FromUri] string color)
        {
            // NO JSON NEEDED.
            return Ok(DeleteSQLHandler.DeleteColor(color));
        }

        /// <summary>
        /// Elimina un vuelo de la base de datos.
        /// </summary>
        /// <param name="flight">El id del vuelo.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/delete/flights/{flight}")]
        public IHttpActionResult DeleteColor([FromUri] int flight)
        {
            // NO JSON NEEDED.
            return Ok(DeleteSQLHandler.DeleteFlight(flight));
        }

        /// <summary>
        /// Elimina un rol de la base de datos.
        /// </summary>
        /// <param name="role">El nombre del rol.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/delete/roles/{role}")]
        public IHttpActionResult DeleterRole([FromUri] string role)
        {
            // NO JSON NEEDED.
            return Ok(DeleteSQLHandler.DeleteRole(role));
        }
    }
}
