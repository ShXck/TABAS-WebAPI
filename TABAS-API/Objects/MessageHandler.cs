using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TABAS_API.Objects
{
    public class MessageHandler
    {
        /// <summary>
        /// Devuelve mensaje de error al insertar un nuevo usuario, si este ya existe.
        /// </summary>
        /// <returns>Mensaje de error.</returns>
        public static string UserExistsMSG()
        {
            return "User information is already on the system.";
        }

        public static string SuccessMSG()
        {
            return "Task successfully executed.";
        }

        public static string ErrorMSG()
        {
            return "Task could be completed.";
        }
    }
}