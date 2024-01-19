using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace z2
{
    internal class Database
    {
        NpgsqlConnection npgsqlConnection = new NpgsqlConnection("Server = localhost; Port = 5432; Database = db_uchet; User Id = postgres; Password = assaq123");

        public void InitializeConnection(string connectionString)
        {
            npgsqlConnection = new NpgsqlConnection(connectionString);
        }
        public void OpenConnection()
        {
            if (npgsqlConnection.State == System.Data.ConnectionState.Closed)
            {
                npgsqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (npgsqlConnection.State == System.Data.ConnectionState.Open)
            {
                npgsqlConnection.Close();
            }
        }

        public NpgsqlConnection GetConnection()
        {
            return npgsqlConnection;
        }
    }
}
