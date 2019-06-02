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
        /// Obtiene una lista de los ids de todas las maletas que no han sido chequeadas
        /// </summary>
        /// <returns>Lista de id de maletas.</returns>
        public static string GetUncheckedBaggage()
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT suitcase_id FROM SUITCASE WHERE NOT EXISTS (SELECT FROM SUITCASE_CHECK WHERE suitcase_id = SUITCASE.suitcase_id)";
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
            SqlConnection sconn = ConnectionHandler.GetSSMSConnection();
            sconn.Open();
            conn.Open();

            string query = "INSERT INTO SUITCASE_CHECK (suitcase_id, user_id, status, comment) VALUES (@suit_id, @user_id, @status, @comm)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("suit_id", scan_info.suitcase_id);
            cmd.Parameters.AddWithValue("user_id", SQLHelper.GetUserID(scan_info.username));
            cmd.Parameters.AddWithValue("status", scan_info.status);

            if (scan_info.status.Equals("Rejected")) cmd.Parameters.AddWithValue("comm", scan_info.comment);
            else cmd.Parameters.AddWithValue("comm", "x");

            int result = cmd.ExecuteNonQuery();

            conn.Close();
            sconn.Close();

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
        /// Asigna una maleta a una sección del avión correspondiente al vuelo.
        /// </summary>
        /// <param name="bagg_section">Las especificaciones de la inserción.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string AssignBaggageToSection(BagToSectionDTO bagg_section)
        {
            string result = JSONHandler.BuildMsg(0, MessageHandler.FullSection(bagg_section.section_id));
            if (!SQLHelper.IsSectionFull(bagg_section.section_id))
            {    
                NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
                conn.Open();

                string query = "INSERT INTO BAG_TO_SECTION VALUES (@fid, @secid, @suitid)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("fid", bagg_section.flight_id);
                cmd.Parameters.AddWithValue("secid", bagg_section.section_id);
                cmd.Parameters.AddWithValue("suitid", bagg_section.suitcase_id);

                int q_result = cmd.ExecuteNonQuery();

                conn.Close();
                cmd.Dispose();

                if (q_result == 1) return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
                else return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
            }
            return result;
        }

        /// <summary>
        /// Obtiene el nombre del usuario que escaneó la maleta.
        /// </summary>
        /// <param name="suitcase_id">El id de la maleta.</param>
        /// <returns>El nombre del usuario que escaneo la maleta.</returns>
        public static string GetCheckerUser(int suitcase_id)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT user_id FROM SUITCASE_CHECK WHERE suitcase_id = @sid";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("sid", suitcase_id);

            int user_id = 0;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    if (reader.Read()) user_id = reader.GetInt32(0);
                }
            }
            cmd.Dispose();
            conn.Close();
            return JSONHandler.BuildFullName(SQLHelper.GetUserFullName(user_id));
        }

        /// <summary>
        /// Obtiene todas las maletas que han sido escaneadas.
        /// </summary>
        /// <returns>Lista de maletas escaneadas.</returns>
        public static string GetAllScannedBaggage()
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT suitcase_id FROM SUITCASE_CHECK";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            List<int> sections = new List<int>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("baggage"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) sections.Add(reader.GetInt32(0));
                    result = JSONHandler.BuildListIntResult("baggage", sections);
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