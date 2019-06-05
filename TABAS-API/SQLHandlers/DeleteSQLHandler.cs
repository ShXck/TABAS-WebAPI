using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TABAS_API.Objects;

namespace TABAS_API.SQLHandlers
{
    public class DeleteSQLHandler
    {
        /// <summary>
        /// Elimina un avión de la base de datos.
        /// </summary>
        /// <param name="model">El modelo del avión.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string DeleteAirplane(string model)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString());
            conn.Open();

            string query = "SELECT delete_airplane(@pid)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("pid", SQLHelper.GetPlaneID(model));

            string result = String.Empty;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if(reader.HasRows) result = JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
                else result = JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
            }
            conn.Close();
            cmd.Dispose();
            return result;            
        }

        /// <summary>
        /// Elimina un bagcart.
        /// </summary>
        /// <param name="bagc_id">El id del bagcart.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string DeleteBagcart(int bagc_id)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString());
            conn.Open();

            string query = "SELECT delete_bagcart(@bid)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("bid", bagc_id);

            string result = String.Empty;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows) result = JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
                else result = JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
            }
            conn.Close();
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Elimina un bagcart.
        /// </summary>
        /// <param name="color">El id del bagcart.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string DeleteColor(string color)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString());
            conn.Open();

            string query = "SELECT delete_color(@color)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("color", SQLHelper.GetColorID(color));

            string result = String.Empty;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows) result = JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
                else result = JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
            }
            conn.Close();
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Elimina un vuelo.
        /// </summary>
        /// <param name="flight">El id del vuelo.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string DeleteFlight(int flight)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString());
            conn.Open();

            string query = "SELECT delete_flight(@fid)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("fid", flight);

            string result = String.Empty;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows) result = JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
                else result = JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
            }
            conn.Close();
            cmd.Dispose();
            return result;
        }

        /// <summary>
        /// Elimina un rol de la base de datos.
        /// </summary>
        /// <param name="role">El nombre del rol.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string DeleteRole(string role)
        {
            SqlConnection connection = new SqlConnection(ConnectionHandler.GetSSMSString());
            connection.Open();
            string req = "delete_role";

            SqlCommand cmd = new SqlCommand(req, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("role_id", SQLHelper.GetRoleID(role)));

            int result = cmd.ExecuteNonQuery();

            connection.Close();

            return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
        }
    }
}