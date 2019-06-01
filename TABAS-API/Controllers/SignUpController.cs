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
    public class SignUpController : ApiController
    {
        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="data">Los datos de registro.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/signup")]
        public IHttpActionResult SignUp([FromBody] string data)
        {
            // Expected JSON: {full_name: XX, email: XX@XX, phone_number: XX, username: XX, password: XX}
            return Ok(AdminSQLHandler.AdminSignUp(JsonConvert.DeserializeObject<Admin>(data)));
        }

        /// <summary>
        /// Asigna los roles a un usuario.
        /// </summary>
        /// <param name="roles">La lista de roles.</param>
        /// <param name="user">El nombre de usuario.</param>
        /// <returns>El resultado de la acción.</returns>
        [HttpPost, Route("tabas/signup/{user}/roles")]
        public IHttpActionResult AssignRoles([FromBody] string roles, [FromUri] string user) 
        {
            // Expected JSON: {"roles":["A", "B"]}
            List<string> roles_lst = JSONHandler.JArrayToList(roles);
            return Ok(AdminSQLHandler.AssignUserRoles(user, roles_lst));
        }

        /// <summary>
        /// Obtiene una lista de todos los roles.
        /// </summary>
        /// <returns>La lista de roles</returns>
        [HttpGet, Route("tabas/roles")]
        public IHttpActionResult GetRoles()
        {
            // NO JSON NEEDED.
            return Ok(AdminSQLHandler.GetAllRoles()); // OUTPUT JSON: {"roles": ["a", "b", ...]}
        }
    }
}
