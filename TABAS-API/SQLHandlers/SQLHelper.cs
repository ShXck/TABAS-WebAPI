using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
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
            SqlConnection conn = ConnectionHandler.GetSSMSConnection();
            
            string query = "SELECT user_id FROM [USER] WHERE username = @user";
            int user_id = -1;
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("user", username);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read()) user_id = reader.GetInt32(0);
                    }
                }
            }
            conn.Close();
            return user_id;           
        }

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
    }
}