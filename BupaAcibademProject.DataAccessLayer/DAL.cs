using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.DataAccessLayer
{
    public class DAL : IDAL
    {
        readonly List<SqlParameter> _parameters = new List<SqlParameter>();

        private SqlCommand WriteQuery(string query, CommandType queryType)
        {
            var connection = new Connection();
            var cmd = connection.DbConnection.CreateCommand();
            cmd.CommandText = query;
            cmd.CommandType = queryType;
            return cmd;
        }

        public void AddInputParameter(params SqlParameter[] sqlParameters)
        {
            _parameters.Clear();
            foreach (var item in sqlParameters)
            {
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = item.ParameterName;
                parameter.Value = item.Value;
                _parameters.Add(parameter);
            }
        }
        public void AddOutputParameter(string parameterName, object parameterValue)
        {
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = parameterValue;
            parameter.Direction = ParameterDirection.Output;
            _parameters.Add(parameter);
        }
        private void AddParametersToQuery(SqlCommand command)
        {
            command.Parameters.AddRange(_parameters.ToArray());
            _parameters.Clear();
        }
        public object GetParameterValues(string parameterName)
        {
            foreach (var item in _parameters)
            {
                if (item.ParameterName == parameterName)
                {
                    return item.Value.ToString();
                }
            }
            return null;
        }
        public bool ExecuteQuery(string query, CommandType queryType)
        {
            try
            {
                var cmd = WriteQuery(query, queryType);
                AddParametersToQuery(cmd);
                var res = cmd.ExecuteNonQuery();
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                cmd.Connection.Dispose();
                cmd.Dispose();
                if (res > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool ExecuteInsertQuery(string query, CommandType queryType)
        {
            try
            {
                var cmd = WriteQuery(query, queryType);
                AddParametersToQuery(cmd);
                var res = cmd.ExecuteNonQuery();
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                cmd.Connection.Dispose();
                cmd.Dispose();
                if (res > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int ExecuteUpdateQuery(string query, CommandType queryType)
        {
            try
            {
                var cmd = WriteQuery(query, queryType);
                AddParametersToQuery(cmd);
                var res = cmd.ExecuteNonQuery();
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                cmd.Connection.Dispose();
                cmd.Dispose();

                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ExecuteDeleteQuery(string query, CommandType queryType)
        {
            try
            {
                var cmd = WriteQuery(query, queryType);
                AddParametersToQuery(cmd);
                var res = cmd.ExecuteNonQuery();
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                cmd.Connection.Dispose();
                cmd.Dispose();

                if (res > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public object ExecuteScalarQuery(string query, CommandType queryType)
        {
            try
            {
                var cmd = WriteQuery(query, queryType);
                AddParametersToQuery(cmd);
                var res = cmd.ExecuteScalar();
                if (cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
                cmd.Connection.Dispose();
                cmd.Dispose();

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public SqlDataReader ExecuteDrSelectQuery(string query, CommandType queryType)
        {
            try
            {
                var cmd = WriteQuery(query, queryType);
                AddParametersToQuery(cmd);
                var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return dr;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public DataTable ExecuteDtSelectQuery(string query, CommandType queryType)
        {
            try
            {
                var dr = ExecuteDrSelectQuery(query, queryType);
                var dt = new DataTable();
                dt.Load(dr);

                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
