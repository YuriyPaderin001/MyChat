using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using MyChat.Databases;

namespace MyChat.Repositories
{
    public abstract class AbstractRepository<T> where T : class
    {
        public IDatabase Database { get; set; }

        public AbstractRepository(IDatabase database)
        {
            Database = database;
        }

        public List<T> GetList(string sqlExpression, params DbParameter[] parameters)
        {
            DataTable resultTable = Database.ExecuteQuery(sqlExpression, parameters);

            List<T> result = new List<T>();
            foreach (DataRow row in resultTable.Rows)
            {
                T item = ParseDataRow(row);
                result.Add(item);
            }

            return result;
        }

        public T GetFirst(string sqlExpression, params DbParameter[] parameters)
        {
            T result = null;
            DataTable resultTable = Database.ExecuteQuery(sqlExpression, parameters);
            if (resultTable.Rows.Count > 0)
            {
                DataRow row = resultTable.Rows[0];
                result = ParseDataRow(row);
            }

            return result;
        }

        public abstract int Add(T value);
        public abstract int Update(T value);
        public abstract int Remove(T value);

        protected abstract T ParseDataRow(DataRow row);

        protected int ExecuteInsert(string sqlExpression, params DbParameter[] parameters)
        {
            sqlExpression += "\nSELECT LAST_INSERT_ID();";
            object result = Database.ExecuteScalar(sqlExpression, parameters);
            return Convert.ToInt32(result);
        }

        protected int ExecuteUpdate(string sqlExpression, params DbParameter[] parameters)
        {
            return Database.ExecuteUpdate(sqlExpression, parameters);
        }
        
        protected DbParameter BuildParameter(string parameterName, object parameterValue)
        {
            return Database.BuildParameter(parameterName, parameterValue);
        }
    }
}
