﻿using System;
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

        public static string ScanMessage(bool pass)
        {
            if (pass) return "Nothing found.";
            else return "Prohibited Items in Baggage.";
        }
    }
}