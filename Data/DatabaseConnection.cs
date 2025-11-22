using System;
using System.Data.SqlClient;

namespace TiendaVirtual.Data
{
    /// <summary>
    /// Clase para manejar la conexión a la base de datos
    /// </summary>
    public static class DatabaseConnection
    {
        // Configuración de la base de datos
        private static string serverName = "(localdb)\\MSSQLLocalDB";
        private static string databaseName = "Tienda";
        private static bool useWindowsAuth = true;

        /// <summary>
        /// Obtiene la cadena de conexión configurada
        /// </summary>
        public static string GetConnectionString()
        {
            if (useWindowsAuth)
            {
                return $"Server={serverName};Database={databaseName};Integrated Security=True;TrustServerCertificate=True;";
            }
            else
            {
                // Por si se necesita autenticación SQL en el futuro
                return $"Server={serverName};Database={databaseName};User Id=usuario;Password=password;TrustServerCertificate=True;";
            }
        }

        /// <summary>
        /// Crea y retorna una nueva conexión SqlConnection
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        /// <summary>
        /// Prueba la conexión a la base de datos
        /// </summary>
        /// <returns>True si la conexión es exitosa, False en caso contrario</returns>
        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Ejecuta un comando SQL sin retornar resultados (INSERT, UPDATE, DELETE)
        /// </summary>
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar comando: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Ejecuta un comando SQL y retorna un valor escalar (SELECT COUNT, MAX, etc.)
        /// </summary>
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar consulta escalar: {ex.Message}", ex);
            }
        }
    }
}