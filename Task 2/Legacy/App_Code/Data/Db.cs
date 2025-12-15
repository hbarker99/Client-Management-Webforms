using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ClientManagementWebforms.Data
{
    public static class Db
    {
        private const int CommandTimeoutSeconds = 30;

        private static string ConnectionString
        {
            get
            {
                var connection = ConfigurationManager.ConnectionStrings["ClientManagementDemo"];
                if (connection == null || string.IsNullOrEmpty(connection.ConnectionString))
                {
                    throw new InvalidOperationException("Connection string 'ClientManagementDemo' is not configured.");
                }

                return connection.ConnectionString;
            }
        }

        public static DataTable ReadTable(string storedProcedureName, params SqlParameter[] parameters)
        {
            var dataSet = ReadDataSet(storedProcedureName, parameters);
            if (dataSet.Tables.Count == 0)
            {
                return new DataTable();
            }

            return dataSet.Tables[0];
        }

        public static DataSet ReadDataSet(string storedProcedureName, params SqlParameter[] parameters)
        {
            var dataSet = new DataSet();

            using (var connection = new SqlConnection(ConnectionString))
            using (var command = CreateCommand(connection, storedProcedureName, parameters))
            using (var adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(dataSet);
            }

            return dataSet;
        }

        public static object Scalar(string storedProcedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = CreateCommand(connection, storedProcedureName, parameters))
            {
                connection.Open();
                return command.ExecuteScalar();
            }
        }

        public static int Exec(string storedProcedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = CreateCommand(connection, storedProcedureName, parameters))
            {
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        private static SqlCommand CreateCommand(SqlConnection connection, string storedProcedureName, params SqlParameter[] parameters)
        {
            var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = CommandTimeoutSeconds
            };

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter != null)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
            }

            return command;
        }
    }
}

