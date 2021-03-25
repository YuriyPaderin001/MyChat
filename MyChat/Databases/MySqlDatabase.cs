using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;

namespace MyChat.Databases
{
    public class MySqlDatabase : IDatabase
    {
        public string ConnectionString { get; set; }

        public MySqlDatabase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public DataTable ExecuteQuery(string sqlExpression, params DbParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            try
            {
                connection.Open();
                MySqlCommand command = BuildCommand(connection, sqlExpression, parameters);
                
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dataTable);

                return dataTable;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }

            return dataTable;
        }

        public int ExecuteUpdate(string sqlExpression, params DbParameter[] parameters)
        {
            int updatedRowsCount = 0;
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            try
            {
                connection.Open();
                MySqlCommand command = BuildCommand(connection, sqlExpression, parameters);
                updatedRowsCount =  command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }

            return updatedRowsCount;
        }

        public object ExecuteScalar(string sqlExpression, params DbParameter[] parameters)
        {
            object result = null;
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            try
            {
                connection.Open();
                MySqlCommand command = BuildCommand(connection, sqlExpression, parameters);
                result = command.ExecuteScalar();
                System.Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public DbParameter BuildParameter(string parameterName, object parameterValue)
        {
            return new MySqlParameter(parameterName, parameterValue);
        }

        private MySqlCommand BuildCommand(MySqlConnection connection, string sqlExpression, params DbParameter[] parameters)
        {
            MySqlCommand command = new MySqlCommand(sqlExpression, connection);
            command.Parameters.AddRange(parameters);

            return command;
        }
    }
}
