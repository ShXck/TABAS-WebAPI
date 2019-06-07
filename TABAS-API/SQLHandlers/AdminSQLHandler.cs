using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using TABAS_API.DataObjects;
using TABAS_API.SQLHandlers;

namespace TABAS_API.Objects
{
    public class AdminSQLHandler
    {
        private static Random random = new Random();
        /// <summary>
        /// Inserta un nuevo administrador en la base de datos.
        /// </summary>
        /// <param name="admin">Los datos del admin.</param>
        /// <returns>El mensaje de resultado de la acción.</returns>
        public static string AdminSignUp(Admin admin)
        {
            if (!SQLHelper.UserExists(admin.username, admin.email, admin.phone_number))
            {
                SqlConnection conn = new SqlConnection(ConnectionHandler.GetSSMSString());
                
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

                    if (query_result == 1)
                    {
                        conn.Close();
                        return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
                    }
                }
                return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
                
            }
            return JSONHandler.BuildMsg(0, MessageHandler.UserExistsMSG());
        }

        public static string GetAllRoles()
        {
            SqlConnection conn = new SqlConnection(ConnectionHandler.GetSSMSString());
            conn.Open();

            string query = "SELECT role from ROLE";
            SqlCommand cmd = new SqlCommand(query, conn);

            List<string> roles = new List<string>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("roles"));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) roles.Add(reader.GetString(0));
                    result = JSONHandler.BuildListStrResult("roles", roles);
                }
            }

            cmd.Dispose();
            conn.Close();
            return result;
        } 

        /// <summary>
        /// Asigna roles a un usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario.</param>
        /// <param name="roles">La lista de roles de un usuario.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string AssignUserRoles(string username, List<string> roles)
        {
            SqlConnection conn = new SqlConnection(ConnectionHandler.GetSSMSString());

            conn.Open();
            
            for (int i = 0; i < roles.Count; i++)
            {
                string query = "INSERT INTO USER_ROLE VALUES(@id, @role)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("id", SQLHelper.GetUserID(username));
                cmd.Parameters.AddWithValue("role", SQLHelper.GetRoleID(roles.ElementAt(i)));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            conn.Close();
            return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());        
        }

        /// <summary>
        /// Verifica los credenciales de un usuario.
        /// </summary>
        /// <param name="admin">Los credenciales el usuario.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string UserLogin(LoginDTO admin)
        {
            SqlConnection conn = new SqlConnection(ConnectionHandler.GetSSMSString());
            conn.Open();

            string query = "SELECT username, password, role FROM [USER] JOIN ([USER_ROLE] JOIN [ROLE] ON [USER_ROLE].role_id = [ROLE].role_id) ON [USER].user_id = [USER_ROLE].user_id WHERE username = @user AND password = @password AND role = @role";
            SqlCommand cmd = new SqlCommand(query, conn);

            string encr_pass = Cipher.Encrypt(admin.password);

            cmd.Parameters.Add(new SqlParameter("user", admin.username));
            cmd.Parameters.Add(new SqlParameter("password", encr_pass));
            cmd.Parameters.Add(new SqlParameter("role", admin.role));

            string result = JSONHandler.BuildMsg(0, MessageHandler.LoginFailedMSG());

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows) result = JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Obtiene los colores disponibles para describir una maleta.
        /// </summary>
        /// <returns>La lista de colores en la base de datos.</returns>
        public static string GetColors()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "SELECT color_name FROM color";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            List<string> colors = new List<string>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("colors"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) colors.Add(reader.GetString(0));
                    result = JSONHandler.BuildListStrResult("colors", colors);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Crea una nueva maleta y la asocia a un cliente.
        /// </summary>
        /// <param name="bagg_dto">Los datos de la maleta.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string InsertNewBaggage(BaggageDTO bagg_dto)
        {
            if (SQLHelper.UserExists(bagg_dto.username, "x", "x"))
            {
                Tuple<int, int> ids = SQLHelper.GetBaggageIDs(bagg_dto);

                BaggageDAO bagg_dao = new BaggageDAO(ids.Item1, ids.Item2, bagg_dto.weight, (Decimal)bagg_dto.weight * (Decimal)605.0);

                NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString());
                conn.Open();
                string query = "INSERT INTO SUITCASE (weight, color_id, user_id, cost) VALUES (@weight, @color, @user, @cost)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("weight", bagg_dao.weight);
                cmd.Parameters.AddWithValue("color", bagg_dao.color_id);
                cmd.Parameters.AddWithValue("user", bagg_dao.user_id);
                cmd.Parameters.AddWithValue("cost", bagg_dao.cost);

                int result = cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();

                if (result == 1) return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());

                return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
            }
            return JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("user"));
        }

        /// <summary>
        /// Crea un nuevo bag cart.
        /// </summary>
        /// <param name="cart">Los datos del bagcart.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string CreateBagCart(BagCart cart)
        {
            if (!SQLHelper.BagcartExists(cart))
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
                conn.Open();

                string query = "INSERT INTO BAGCART (brand_id, year, capacity) VALUES(@id, @year, @cap)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("id", SQLHelper.GetBrandID(cart.brand));
                cmd.Parameters.AddWithValue("year", cart.model);
                cmd.Parameters.AddWithValue("cap", cart.capacity);

                int result = cmd.ExecuteNonQuery();

                cmd.Dispose();
                conn.Close();

                if (result == 1) return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());

                return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
            }
            return JSONHandler.BuildMsg(0, MessageHandler.ResourceAlreadyExists("Bagcart"));
        }


        /// <summary>
        /// Inserta una nueva marca de carritos.
        /// </summary>
        /// <param name="cart">El nombre del carro.</param>
        /// <returns>El resultado de la acción.</returns>
        public static string InsertNewCartBrand(BagCart cart)
        {
            if (!SQLHelper.BrandExists(cart.brand))
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
                conn.Open();

                string query = "INSERT INTO BAGCART_BRAND (brand) VALUES(@brand)";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("brand", cart.brand);

                int result = cmd.ExecuteNonQuery();

                if (result == 1) return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());

                conn.Close();
                cmd.Dispose();

                return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
            }
            return JSONHandler.BuildMsg(0, MessageHandler.ResourceAlreadyExists("Brand"));
        }

        /// <summary>
        /// Obtiene las marcas de bagcarts de la base de datos y las junta en una lista.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        public static string GetAllBagCartBrands()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "SELECT brand FROM BAGCART_BRAND";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            List<string> brands = new List<string>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("brands"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) brands.Add(reader.GetString(0));
                    result = JSONHandler.BuildListStrResult("brands", brands);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Crea un nuevo vuelo en la base de datos.
        /// </summary>
        /// <param name="flight"></param>
        /// <returns></returns>
        public static string CreateNewFlight(FlightDTO flight)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "INSERT INTO FLIGHT (plane_id) VALUES(@plane)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("plane", SQLHelper.GetPlaneID(flight.model));

            int result = cmd.ExecuteNonQuery();

            if (result == 1) return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());

            return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
        }

        /// <summary>
        /// Obtiene los modelos de aviones de la base de datos y las junta en una lista.
        /// </summary>
        /// <returns>El resultado de la acción.</returns>
        public static string GetAllPlanes()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "SELECT model FROM AIRPLANE";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            List<string> brands = new List<string>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("airplanes"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) brands.Add(reader.GetString(0));
                    result = JSONHandler.BuildListStrResult("airplanes", brands);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Obtiene una lista de los id de los vuelos.
        /// </summary>
        /// <returns>La lista de los vuelos.</returns>
        public static string GetUnassignedFlights()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "SELECT flight_id FROM FLIGHT WHERE  NOT EXISTS (SELECT FROM BAGCART_TO_FLIGHT WHERE  flight_id = FLIGHT.flight_id)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            List<int> flights = new List<int>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("Unassigned Flights"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) flights.Add(reader.GetInt32(0));
                    result = JSONHandler.BuildListIntResult("flights", flights);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Inserta un 
        /// </summary>
        /// <param name="fl_bagcart"></param>
        /// <returns></returns>
        public static string AssignBagcart(FlightBagCartDTO fl_bagcart)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "INSERT INTO BAGCART_TO_FLIGHT VALUES (@flid, @bgid, @seal)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("flid", fl_bagcart.flight_id);
            cmd.Parameters.AddWithValue("bgid", fl_bagcart.bagcart_id);
            cmd.Parameters.AddWithValue("seal", "X");

            int result = cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Close();

            if (result == 1) return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
            else return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
        }

        /// <summary>
        /// Cierra el bagcart y genera el código de seguridad.
        /// </summary>
        /// <param name="flight">El id del vuelo a el que se le cierra el bagcart.</param>
        /// <returns>El resultado, con el código de seguridad.</returns>
        public static string CloseBagcart(int flight)
        {
            string sec_seal = GetNewSecuritySeal();

            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "UPDATE BAGCART_TO_FLIGHT SET seal = @seal WHERE flight_id = @flight";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("seal", sec_seal);
            cmd.Parameters.AddWithValue("flight", flight);

            int result = cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Close();

            if (result == 1) return JSONHandler.BuildSeal(sec_seal);
            else return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
        }

        /// <summary>
        /// Obtiene una lista de aquellos vuelos que no hayan sido cerrados.
        /// </summary>
        /// <returns>La lista de vuelos activos.</returns>
        public static string GetAllActiveFlights()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "SELECT flight_id FROM BAGCART_TO_FLIGHT WHERE LENGTH(seal) = @len";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("len", 1);

            List<int> flights = new List<int>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("Active Flights"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) flights.Add(reader.GetInt32(0));
                    result = JSONHandler.BuildListIntResult("flights", flights);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Devuelve una lista de ids de los bagcarts existentes.
        /// </summary>
        /// <returns>La lista de bagcarts.</returns>
        public static string GetBagcarts()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "SELECT bagcart_id FROM BAGCART";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            List<int> bagcarts = new List<int>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("Bagcarts"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) bagcarts.Add(reader.GetInt32(0));
                    result = JSONHandler.BuildListIntResult("bagcarts", bagcarts);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result; 
        }

        /// <summary>
        /// Obtiene una lista de maletas que han sido chequeadas pero no han sido asignadas a ningún vuelo.
        /// </summary>
        /// <returns>La lista de maletas sin asignar</returns>
        public static string GetUnassignedBaggage()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "SELECT suitcase_id FROM SUITCASE_CHECK WHERE NOT EXISTS (SELECT FROM BAG_TO_SECTION WHERE suitcase_id = SUITCASE_CHECK.suitcase_id)";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            List<int> flights = new List<int>();

            string result = JSONHandler.BuildMsg(0, MessageHandler.ResourceNotFound("Unassigned Baggage"));

            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read()) flights.Add(reader.GetInt32(0));
                    result = JSONHandler.BuildListIntResult("suitcases", flights);
                }
            }
            cmd.Dispose();
            conn.Close();
            return result;
        }

        /// <summary>
        /// Asigna un avión a un vuelo existente.
        /// </summary>
        /// <param name="plane_dto">Los datos del avión y el vuelo.</param>
        /// <returns>El resultado de la actualización.</returns>
        public static string AssignPlaneToFlight(AssignPlaneDTO plane_dto)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConnectionHandler.GetPGString()); 
            conn.Open();

            string query = "UPDATE FLIGHT SET plane_id = @pid WHERE flight_id = @fid";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("pid", SQLHelper.GetPlaneID(plane_dto.model));
            cmd.Parameters.AddWithValue("fid", plane_dto.flight);

            int result = cmd.ExecuteNonQuery();


            System.Diagnostics.Debug.WriteLine(result);

            cmd.Dispose();
            conn.Close();

            if (result == 1) return JSONHandler.BuildMsg(1, MessageHandler.SuccessMSG());
            else return JSONHandler.BuildMsg(0, MessageHandler.ErrorMSG());
        }

        /// <summary>
        /// Genera un string alfanumérico aleatorio.
        /// </summary>
        /// <returns>El string aleatorio.</returns>
        private static string GetNewSecuritySeal()
        {
            const int len = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, len)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}