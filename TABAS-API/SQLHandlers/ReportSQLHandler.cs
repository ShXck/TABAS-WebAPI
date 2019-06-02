using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TABAS_API.Objects;

namespace TABAS_API.SQLHandlers
{
    public class ReportSQLHandler
    {
        /// <summary>
        /// Obtiene el número de maletas por cliente.
        /// </summary>
        /// <returns>El mensaje con el conteo de las maletas de todos los usuarios.</returns>
        public static string GetBaggageByUser()
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT user_id, COUNT (user_id) FROM SUITCASE GROUP BY user_id;";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            List<string> count = new List<string>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("Baggage"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count.Add(JSONHandler.BuildBaggageCount(SQLHelper.GetUserFullName(reader.GetInt32(0)),
                                                                reader.GetInt32(1)));
                    }
                    result = JSONHandler.BuildListStrResult("baggage", count);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Obtiene el peso máximo, en equipaje, de un avión.
        /// </summary>
        /// <param name="flight">El identificador del avión.</param>
        /// <returns>El peso máximo del avión.</returns>
        private static int GetTotalSuitcase(int flight)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT COUNT(*) FROM BAG_TO_SECTION WHERE flight_id = @flight";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("flight", flight);

            int suitcase_count = 0;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    suitcase_count = reader.GetInt32(0);
                }
            }
            cmd.Dispose();
            conn.Close();
            return suitcase_count;
        }

        /// <summary>
        /// Obtiene el total de maletas rechazas.
        /// </summary>
        /// <param name="flight">El identificador del avión.</param>
        /// <returns>El peso máximo del avión.</returns>
        private static int GetRejectedSuitcases(int flight)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT COUNT(*) FROM (SUITCASE_CHECK INNER JOIN BAG_TO_SECTION ON(SUITCASE_CHECK.suitcase_id = BAG_TO_SECTION.suitcase_id)) " +
                "WHERE(flight_id = @flight AND status = 'Rejected')";

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("flight", flight);

            int suitcase_count = 0;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    suitcase_count = reader.GetInt32(0);
                }
            }
            cmd.Dispose();
            conn.Close();
            return suitcase_count;
        }

        /// <summary>
        /// Obtiene el reporte de equipaje por vuelo.
        /// </summary>
        /// <returns>Tabla con la información de maletas por vuelo.</returns>
        public static string GetBaggageReport(int flight)
        {
            string model = SQLHelper.GetPlaneModel(flight);
            double weight = SQLHelper.GetMaxWeight(flight);
            int total_suitcase = GetTotalSuitcase(flight);
            int suitcase_rejected = GetRejectedSuitcases(flight);
            int suitcase_acepted = total_suitcase - suitcase_rejected;

            return JSONHandler.BuildBaggageReport(flight, model, weight, total_suitcase,
                                      suitcase_rejected, suitcase_acepted);
        }

    }
}