using System.Data;
using System.Data.Common;

namespace MyChat.Databases
{
    public interface IDatabase
    {
        public DataTable ExecuteQuery(string sqlExpression, params DbParameter[] parameters);
        public int ExecuteUpdate(string sqlExpression, params DbParameter[] parameters);
        public object ExecuteScalar(string sqlExpression, params DbParameter[] parameters);

        public DbParameter BuildParameter(string parameterName, object parameterValue);
    }
}
