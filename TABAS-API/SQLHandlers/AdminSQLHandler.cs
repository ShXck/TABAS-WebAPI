using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TABAS_API.DataObjects;
using TABAS_API.SQLHandlers;

namespace TABAS_API.Objects
{
    public class AdminSQLHandler
    {
        /// <summary>
        /// Inserta un nuevo administrador en la base de datos.
        /// </summary>
        /// <param name="admin">Los datos del admin.</param>
        /// <returns>El mensaje de resultado de la acción.</returns>
        public static string AdminSignUp(Admin admin)
        {
            if (!SQLHelper.UserExists(admin.username, admin.email, admin.phone_number))
            {
                using (SqlConnection conn = ConnectionHandler.GetSSMSConnection())
                {
                    string query = "INSERT INTO [USER] VALUES(@name, @phone, @email, @user, @pass)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("name", admin.full_name);
                        cmd.Parameters.AddWithValue("phone", admin.phone_number);
                        cmd.Parameters.AddWithValue("email", admin.email);
                        cmd.Parameters.AddWithValue("user", admin.username);
                        cmd.Parameters.AddWithValue("pass", Cipher.Encrypt(admin.password));

                        int query_result = cmd.ExecuteNonQuery();

                        if (query_result == 1) return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
                    }
                    return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
                }
            }
            return JSONHandler.BuildMsg(1, MessageHandler.UserExistsMSG());
        }

        public static string AssignUserRoles(string username, List<string> roles)
        {
            SqlConnection conn = ConnectionHandler.GetSSMSConnection();
            int user_id = SQLHelper.GetUserID(username);

            conn.Open();
            
            for (int i = 0; i < roles.Count; i++)
            {
                string query = "INSERT INTO USER_ROLE VALUES(@id, @role)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("id", user_id);
                cmd.Parameters.AddWithValue("role", SQLHelper.GetRoleID(roles.ElementAt(i)));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            conn.Close();
            return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());        
        }
    }
}