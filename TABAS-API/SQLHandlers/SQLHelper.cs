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
            SqlConnection conn = ConnectionHandler.GetSSMSConnection();

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
            SqlConnection sconn = ConnectionHandler.GetSSMSConnection();

            sconn.Open();
            pconn.Open();

            string color_id_qry = "SELECT color_id FROM COLOR WHERE color_name = @color";

            NpgsqlCommand color_cmd = new NpgsqlCommand(color_id_qry, pconn);

            color_cmd.Parameters.AddWithValue("color", bag_dto.color);

            int color_id = -1;

            using (NpgsqlDataReader reader = color_cmd.ExecuteReader())
            {
                if (reader.Read()) color_id = reader.GetInt32(0);
            }

            int user_id = GetUserID(bag_dto.username);

            color_cmd.Dispose();

            pconn.Close();
            sconn.Close();

            return new Tuple<int, int>(user_id, color_id);
        }

        /// <summary>
        /// Obtiene el id de una marca de bagcart.
        /// </summary>
        /// <param name="brand">El nombre de la marca.</param>
        /// <returns>El id de la marca.</returns>
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

        /// <summary>
        /// Obtiene el id de un modelo de avión.
        /// </summary>
        /// <param name="brand">El nombre del modelo.</param>
        /// <returns>El id del modelo.</returns>
        public static int GetPlaneID(string model)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();

            string query = "SELECT plane_id FROM AIRPLANE WHERE model = @model";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("model", model);

            int plane_id = -1;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read()) plane_id = reader.GetInt32(0);
            }
            return plane_id;
        }

        /// <summary>
        /// Obtiene la id de un usuario asociada a una maleta.
        /// </summary>
        /// <param name="suit_id">El id de la maleta.</param>
        /// <returns>El id del usuario.</returns>
        public static int GetUserIdBySuitcase(int suit_id)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();

            string query = "SELECT user_id FROM SUITCASE WHERE suitcase_id = @id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("id", suit_id);

            int user_id = -1;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read()) user_id = reader.GetInt32(0);
            }
            return user_id;
        }

        /// <summary>
        /// Obtiene el id del avión asociado a un vuelo.
        /// </summary>
        /// <param name="flid">El id del vuelo.</param>
        /// <returns>El id del avión.</returns>
        public static int GetPlaneIDByFlight(int flid)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();

            string query = "SELECT plane_id FROM FLIGHT WHERE flight_id = @id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("id", flid);

            int pl_id = -1;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read()) pl_id = reader.GetInt32(0);
            }
            return pl_id;
        }

        /// <summary>
        /// Obtiene la capacidad máxima de un bagcart.
        /// </summary>
        /// <param name="brand">La marca del bargcart.</param>
        /// <returns>La capacidad el bagcart.</returns>
        public static int GetBagcartCapacity(string brand)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();

            string query = "SELECT capacity FROM BAGCART WHERE brand_if = @id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("id", GetBrandID(brand));

            int capacity = -1;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read()) capacity = reader.GetInt32(0);
            }
            return capacity;
        }

        /// <summary>
        /// Obtiene el peso de una maleta.
        /// </summary>
        /// <param name="suit_id">El id de la maleta.</param>
        /// <returns>El peso de la maleta.</returns>
        public static double GetBaggageWeight(int suit_id)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();

            string query = "SELECT weight FROM SUITCASE WHERE suitcase_id = @id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("id", suit_id);

            double weight = -1;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read()) weight = reader.GetDouble(0);
            }
            return weight;
        }

        /// <summary>
        /// Determina si una sección está llena o no.
        /// </summary>
        /// <param name="sec_id">El id de la sección.</param>
        /// <returns>Si está lleno o no.</returns>
        public static bool IsSectionFull(int sec_id)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT suitcase_id FROM BAG_TO_SECTION WHERE section_id = @id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("id", sec_id);

            double curr_weight = 0;
            double max_w = GetSectionMaxWeight(sec_id);
            bool is_full = true;

            List<int> suits_id = new List<int>();

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) suits_id.Add(reader.GetInt32(0));
                }
            }

            for (int i = 0; i < suits_id.Count; i++) curr_weight += GetBaggageWeight(suits_id.ElementAt(i));

            if (curr_weight <= max_w) is_full = false;

            conn.Close();
            cmd.Dispose();
            return is_full;
        }

        /// <summary>
        /// Obtiene el peso máximo de una sección de un avión.
        /// </summary>
        /// <param name="sec_id">El id de la sección.</param>
        /// <returns>El peso máximo de la sección.</returns>
        private static double GetSectionMaxWeight(int sec_id)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();

            string query = "SELECT weight FROM AIRPLANE_SECTION WHERE section_id = @id";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("id", sec_id);

            double weight = 0;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    weight = reader.GetDouble(0);
                }
            }
            return weight;
        }

        /// <summary>
        /// Indica si ya existe un bagcart con las mismas especificaciones.
        /// </summary>
        /// <param name="bagcart">Las especificaciones el bagcart.</param>
        /// <returns>Si ya existe el bagcart o no.</returns>
        public static bool BagcartExists(BagCart bagcart)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT brand_id, year, capacity FROM BAGCART WHERE brand_id = @id and year = @year and capacity = @cap";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("id", SQLHelper.GetBrandID(bagcart.brand));
            cmd.Parameters.AddWithValue("year", bagcart.model);
            cmd.Parameters.AddWithValue("cap", bagcart.capacity);

            bool exists = false;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows) exists = true;               
            }
            cmd.Dispose();
            conn.Close();
            return exists;
        }

        /// <summary>
        /// Indica si ya existe una marca de bagcart con el mismo nombre.
        /// </summary>
        /// <param name="brand">El nombre de la marca.</param>
        /// <returns>Si la marca existe o no.</returns>
        public static bool BrandExists(string brand)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT brand FROM BAGCART_BRAND WHERE brand = @brand";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("brand", brand);

            bool exists = false;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows) exists = true;
            }
            cmd.Dispose();
            conn.Close();
            return exists;
        }

        /// <summary>
        /// Obtiene el nombre completo de un usuario.
        /// </summary>
        /// <param name="user_id">El id del usuario.</param>
        /// <returns>El nombre completo del usuario.</returns>
        public static string GetUserFullName(int user_id)
        {
            SqlConnection conn = ConnectionHandler.GetSSMSConnection();
            string query = "SELECT full_name FROM [USER] WHERE user_id = @id";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("id", user_id);

            string result = "Unknown";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    if(reader.Read()) result = reader.GetString(0);
                }
            }

            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Obtiene el módelo del avión asignado a un vuelo.
        /// </summary>
        /// <param name="flight">el id del vuelo.</param>
        /// <returns>String con el modelo del avión.</returns>
        public static string GetPlaneModel(int flight)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT model FROM AIRPLANE WHERE(plane_id = (SELECT plane_id FROM FLIGHT WHERE(flight_id = @flight)))";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("flight", flight);

            string model = "";

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        model = reader.GetString(0);
                    }
                }
            }
            cmd.Dispose();
            conn.Close();
            return model;
        }

        /// <summary>
        /// Obtiene el peso máximo de un avión.
        /// </summary>
        /// <param name="flight">El identificador del vuelo.</param>
        /// <returns>El peso máximo del avión.</returns>
        public static double GetMaxWeight(int flight)
        {
            NpgsqlConnection conn = ConnectionHandler.GetPGConnection();
            conn.Open();

            string query = "SELECT weight FROM AIRPLANE_SECTION WHERE plane_id = @plane";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("plane", SQLHelper.GetPlaneIDByFlight(flight));

            double weight = 0;

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {

                while (reader.Read())
                {
                    weight += reader.GetDouble(0);
                }
            }
            cmd.Dispose();
            conn.Close();
            return weight;
        }
    }
}