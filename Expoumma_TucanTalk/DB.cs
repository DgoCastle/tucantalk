using MySql.Data.MySqlClient;
using System;
using System.Data.SqlClient;

namespace Expoumma_TucanTalk
{
    public static class DB
    {
        private static string connectionString = "server=localhost;database=expoumma_db;uid=root;pwd=;";

        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
            }
            return conn;
        }
    }
}
