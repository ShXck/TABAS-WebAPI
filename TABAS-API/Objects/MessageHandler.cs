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

        /// <summary>
        /// Muestra un mensaje de éxito.
        /// </summary>
        /// <returns>Mensaje de éxito.</returns>
        public static string SuccessMSG()
        {
            return "Task successfully executed.";
        }

        /// <summary>
        /// Muestra un mensaje de error.
        /// </summary>
        /// <returns>Mensaje de error.</returns>
        public static string ErrorMSG()
        {
            return "Task could be completed.";
        }

        /// <summary>
        /// Muestra un mensaje de error al iniciar sesión.
        /// </summary>
        /// <returns>Mensaje de error al iniciar sesión</returns>
        public static string LoginFailedMSG()
        {
            return "The username or password are incorrect. Please try again.";
        }

        /// <summary>
        /// Muestra un mensaje de fallo al encontrar algún recurso.
        /// </summary>
        /// <param name="resource">El nombre del recurso.</param>
        /// <returns>El mensaje de fallo.</returns>
        public static string ResourceNotFound(string resource)
        {
            return "No " + resource + " were found.";
        }

        /// <summary>
        /// Muestra un mensaje sobre el estado de un escaneo a una maleta.
        /// </summary>
        /// <param name="pass">El resultado del escaneo.</param>
        /// <returns>El mensaje del estado del escaneo.</returns>
        public static string ScanMessage(bool pass)
        {
            if (pass) return "Accepted";
            else return "Rejected";
        }

        /// <summary>
        /// Muestra un mensaje sobre sección de avión llena.
        /// </summary>
        /// <param name="section">El id de la sección.</param>
        /// <returns>El mensaje de error.</returns>
        public static string FullSection(int section)
        {
            return "Unable to assign baggage to section " + section + ". This section is currently full.";
        } 

        /// <summary>
        /// Crea un mensaje que señala la existencia de un recurso.
        /// </summary>
        /// <param name="res">El nombre del recurso.</param>
        /// <returns>Un mensaje que señala la existencia del recurso.</returns>
        public static string ResourceAlreadyExists(string res)
        {
            return res + " already exists in the system.";
        }

        /// <summary>
        /// Muestra un mensaje que indica que no existe un usuario epecificado.
        /// </summary>
        /// <returns>El mensaje de error.</returns>
        public static string UserNotFound()
        {
            return "User id does not match any user.";
        }
    }
}