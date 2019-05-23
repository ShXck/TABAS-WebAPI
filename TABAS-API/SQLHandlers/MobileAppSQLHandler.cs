using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using TABAS_API.DataObjects;
using TABAS_API.Objects;
using TABAS_API.SQLHandlers;


namespace TABAS_API.SQLHandlers
{
    public class MobileAppSQLHandler
    {
        /// <summary>
        /// Obtiene una lista de los ids de todas las maletas.
        /// </summary>
        /// <returns>Lista de id de maletas.</returns>
        public static string GetAllBaggage()
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT suitcase_id FROM SUITCASE";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            List<int> baggs_id = new List<int>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("baggage"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) baggs_id.Add(reader.GetInt32(0));
                    result = JSONHandler.BuildListIntResult("suitcases", baggs_id);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        public static string ScanBaggage()
        {
            if (PassScan()) return JSONHandler.BuildScanResult(true);
            else return JSONHandler.BuildScanResult(false);
        }


        private static bool PassScan()
        {
            Random rdn = new Random();
            int number = rdn.Next(0, 100);

            if (number >= 75) return false;

            return true;
        }




    }
}