using BupaAcibademProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.DataAccessLayer
{
    internal class Connection
    {
        private static readonly StringBuilder StringBuilder = new StringBuilder();
        private SqlConnection _connection;

        public SqlConnection DbConnection
        {
            get
            {
                if (_connection != null)
                {
                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    return _connection;
                }
                else
                {
                    var connectionString = Settings.Current.ConnectionStrings.First();

                    _connection = new SqlConnection(connectionString);

                    if (_connection.State == ConnectionState.Closed)
                    {
                        _connection.Open();
                    }
                    return _connection;
                }
            }
        }
    }
}
