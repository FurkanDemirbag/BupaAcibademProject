using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.DataAccessLayer
{
    public interface IDAL
    {
        public void AddInputParameter(params SqlParameter[] sqlParameters);
        public void AddOutputParameter(string parameterName, object parameterValue);
        public object GetParameterValues(string parameterName);
        public bool ExecuteQuery(string query, CommandType queryType);
        public bool ExecuteInsertQuery(string query, CommandType queryType);
        public int ExecuteUpdateQuery(string query, CommandType queryType);
        public bool ExecuteDeleteQuery(string query, CommandType queryType);
        public object ExecuteScalarQuery(string query, CommandType queryType);
        public SqlDataReader ExecuteDrSelectQuery(string query, CommandType queryType);
        public DataTable ExecuteDtSelectQuery(string query, CommandType queryType);
    }
}
