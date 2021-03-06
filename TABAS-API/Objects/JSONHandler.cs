﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TABAS_API.DataObjects;

namespace TABAS_API.Objects
{
    public class JSONHandler
    {
        /// <summary>
        /// Construye un json con el mensaje de resultado de acción.
        /// </summary>
        /// <param name="result">Entero que representa el resultado. (0 error, 1 no hay error)</param>
        /// <param name="message">El mensaje del resultado.</param>
        /// <returns>El json con la información.</returns>
        public static string BuildMsg(int result, string message)
        {
            JObject json = new JObject();
            json["msg"] = message;
            json["http_result"] = result;
            return json.ToString();
        }

        /// <summary>
        /// Convierte un JArray a una lista de C#.
        /// </summary>
        /// <param name="jarray">El arreglo.</param>
        /// <returns>La lista de valores del arreglo.</returns>
        public static List<string> JArrayToList(string jarray)
        {
            JObject json = JObject.Parse(jarray);
            //JArray arr = JArray.Parse(jarray);
            return json["roles"].ToObject<List<string>>();
        }

        /// <summary>
        /// Crea un JSON array con resultado de una búsqueda.
        /// </summary>
        /// <param name="attribute">El nombre del atributo.</param>
        /// <param name="results">La lista de resultados.</param>
        /// <returns>El JSON con la información.</returns>
        public static string BuildListStrResult(string attribute, List<string> results)
        {
            JArray array = new JArray();
            for (int i = 0; i < results.Count; i++)
            {
                array.Add(results.ElementAt(i));
            }
            JObject result = new JObject();
            result["http_result"] = 1;
            result[attribute] = array;

            return result.ToString();
        }

        /// <summary>
        /// Crea un JSON array con resultado de una búsqueda.
        /// </summary>
        /// <param name="attribute">El nombre del atributo.</param>
        /// <param name="results">La lista de resultados.</param>
        /// <returns>El JSON con la información.</returns>
        public static string BuildListIntResult(string attribute, List<int> results)
        {
            JArray array = new JArray();
            for (int i = 0; i < results.Count; i++)
            {
                array.Add(results.ElementAt(i));
            }
            JObject result = new JObject();
            result["http_result"] = 1;
            result[attribute] = array;

            return result.ToString();
        }

        /// <summary>
        /// Construye un JSON con el código de sefuridad generado para el bagcart.
        /// </summary>
        /// <param name="seal">El código de seguridad generado.</param>
        /// <returns>El json con el código.</returns>
        public static string BuildSeal(string seal)
        {
            JObject json = new JObject();
            json["seal"] = seal;
            json["http_result"] = 1;
            return json.ToString();
        }

        /// <summary>
        /// Construye un JSON con el resultado del scaneo.
        /// </summary>
        /// <param name="pass">El resultado del scaneo.</param>
        /// <returns>El mensaje con el resultado del escaneo.</returns>
        public static string BuildScanResult(bool pass)
        {
            JObject json = new JObject();
            json["http_result"] = 1;
            json["pass"] = pass;
            json["status"] = MessageHandler.ScanMessage(pass);
            return json.ToString();
        }

        /// <summary>
        /// Construye un JSON con el nombre completo de un usuario.
        /// </summary>
        /// <param name="user">El nombre del usuario.</param>
        /// <returns>JSON con el nombre del usuario.</returns>
        public static string BuildFullName(string user)
        {
            JObject json = new JObject();
            json["http_result"] = 1;
            json["user"] = user;
            return json.ToString();
        }

        /// <summary>
        /// Construye un JSON con el conteo de maletas de un usuario.
        /// </summary>
        /// <param name="user">El nombre del Usuario.</param>
        /// <param name="count">El número de maletas del usuario.</param>
        /// <returns>El JSON con el conteo.</returns>
        public static string BuildBaggageCount(string user, int count)
        {
            JObject json = new JObject();
            json["user"] = user;
            json["count"] = count;
            return json.ToString();
        }

        /// <summary>
        /// Crea un JSON array específico para el reporte de maletas.
        /// </summary>
        /// /// <param name="flight">Identificador del vuelo.</param>
        /// <param name="model">Modelo del avión asignado al vuelo.</param>
        /// <param name="weight">Peso en equipaje máximo del vuelo.</param>
        /// <param name="total_suitcase">Maletas totales.</param>
        /// <param name="suitcase_rejected">Maletas rechazadas.</param>
        /// <param name="suitcase_acepted">Maletas en el avión.</param>
        /// <returns>El JSON con la información.</returns>
        public static string BuildBaggageReport(int flight, string model, double weight, int total_suitcase,
            int suitcase_rejected, int suitcase_acepted)
        {
            JObject result = new JObject();
            result["http_result"] = 1;
            result["flight"] = flight;
            result["model"] = model;
            result["weight"] = weight;
            result["total_suitcase"] = total_suitcase;
            result["suitcase_rejected"] = suitcase_rejected;
            result["suitcase_acepted"] = suitcase_acepted;

            return result.ToString();
        }
    }
}