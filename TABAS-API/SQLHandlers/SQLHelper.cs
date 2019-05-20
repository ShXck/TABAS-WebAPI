using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TABAS_API.DataObjects;
using TABAS_API.Objects;

namespace TABAS_API.SQLHandlers
{
    public class SQLHelper
    {
        /// <summary>
        /// Verifica si el nombre de usuario ya existe
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Si el usuario existe o no.</returns>
        public static bool UserExists(string username, string email, string phone)
        {
            SqlConnection conn = ConnectionHandler.GetSSMSConnection();
            string query = "SELECT user_id FROM [USER] WHERE username = @user OR email = @email OR phone_number = @phone";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("user", username);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("phone", phone);

            bool result = false;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows) result = true;
            }

            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Obtiene el user_id de un usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <returns>El user_id</returns>
        public static int GetUserID(string username)
        {
            System.Diagnostics.Debug.WriteLine("US: " + username);
            SqlConnection conn = ConnectionHandler.GetSSMSConnection();
            conn.Open();

            string query = "SELECT user_id FROM [USER] WHERE username = @us";
            
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("us", username);

            int user_id = -1;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    if (reader.Read()) user_id = reader.GetInt32(0);
                }
            }            
            conn.Close();
            return user_id;           
        }

        /// <summary>
        /// Obtiene el id de un role.
        /// </summary>
        /// <param name="role_name">El nombre del rol.</param>
        /// <returns>El id del role.</returns>
        public static int GetRoleID(string role_name)
        {
            SqlConnection conn = ConnectionHandler.GetSSMSConnection();
            
            string query = "SELECT role_id FROM ROLE WHERE role = @role";
            int role_id = -1;

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("role", role_name);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read()) role_id = reader.GetInt32(0);
                    }
                }
            }
            return role_id;           
        }

        /// <summary>
        /// Obtiene el color_id y el user_id para la creación de una maleta.
        /// </summary>
        /// <param name="bag_dto">Los datos de la maleta.</param>
        /// <returns>Una tupla de la forma (user_id, color_id)</returns>
        public static Tuple<int, int> GetBaggageIDs(BaggageDTO bag_dto)
        {
            NpgsqlConnection pconn = ConnectionHandler.GetPGConnection();

            pconn.Open();

            string color_id_qry = "SELECT color_id FROM COLOR WHERE color_name = @color";

            NpgsqlCommand color_cmd = new NpgsqlCommand(color_id_qry, pconn);

            color_cmd.Parameters.AddWithValue("color", bag_dto.color);

            int color_id = -1;

            using (NpgsqlDataReader reader = color_cmd.ExecuteReader())
            {
                if (reader.Read()) color_id = reader.GetInt32(0);
            }

            color_cmd.Dispose();

            pconn.Close();

            return new Tuple<int, int>(GetUserID(bag_dto.username), color_id);
        }

        public static int GetBrandID(string brand)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();

            string query = "SELECT brand_id FROM BAGCART_BRAND WHERE brand = @brand";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("brand", brand);

            int b_id = -1;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read()) b_id = reader.GetInt32(0);
            }
            return b_id;
        }
       


    }
}