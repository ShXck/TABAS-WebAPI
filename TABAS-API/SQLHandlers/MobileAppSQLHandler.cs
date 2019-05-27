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

        /// <summary>
        /// Produce un resultado de escaneo.
        /// </summary>
        /// <returns>El resultado del escaneo.</returns>
        public static string ScanBaggage()
        {
            if (PassScan()) return JSONHandler.BuildScanResult(true);
            else return JSONHandler.BuildScanResult(false);
        }

        /// <summary>
        /// Inserta una maleta escaneada en la base de datos.
        /// </summary>
        /// <param name="scan_info">La información del Escaneo.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string InsertScannedBaggage(ScannedBaggDTO scan_info)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "INSERT INTO SUITCASE_CHECK (suitcase_id, user_id, status) VALUES (@suit_id, @user_id, @status)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("suit_id", scan_info.suitcase_id);
            cmd.Parameters.AddWithValue("user_id", SQLHelper.GetUserID(scan_info.username));
            cmd.Parameters.AddWithValue("status", scan_info.status);

            if (scan_info.comment != null || !scan_info.comment.Equals(String.Empty))  cmd.Parameters.AddWithValue("comment", scan_info.comment);

            int result = cmd.ExecuteNonQuery();

            if (result == 1) return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
            else return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
        }

        /// <summary>
        /// Obtiene una lista de las secciones de un avión dado el vuelo asociado a ese avión.
        /// </summary>
        /// <param name="flight">el id del vuelo.</param>
        /// <returns>La lista con las secciones.</returns>
        public static string GetFlightPlaneSections(int flight)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT section_id FROM AIRPLANE_SECTION WHERE plane_id = @plane";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("plane", SQLHelper.GetPlaneIDByFlight(flight));

            List<int> sections = new List<int>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("sections"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) sections.Add(reader.GetInt32(0));
                    result = JSONHandler.BuildListIntResult("sections", sections);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Genera un resultado de escaneo basado en probabilidad.
        /// </summary>
        /// <returns>El resultado del escaneo.</returns>
        private static bool PassScan()
        {
            Random rdn = new Random();
            int number = rdn.Next(0, 100);

            if (number >= 75) return false;

            return true;
        }




    }
}