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

    }
}