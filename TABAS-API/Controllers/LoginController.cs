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
    public class LoginController : ApiController
    {
        /// <summary>
        /// Maneja el inicio de sesión de los usuarios.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/login")]
        public IHttpActionResult Login([FromBody] string credentials)
        {
            /// Expected JSON: {"username": "XXXX", "password": "XXXX", "role": "XXXXX"}
            return Ok(AdminSQLHandler.UserLogin(JsonConvert.DeserializeObject<LoginDTO>(credentials)));
        }
    }
}
