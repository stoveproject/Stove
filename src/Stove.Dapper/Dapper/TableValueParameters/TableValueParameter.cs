using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

using Dapper;

using Microsoft.SqlServer.Server;

using Stove.Extensions;

namespace Stove.Dapper.TableValueParameters
{
    public class TableValueParameter : SqlMapper.IDynamicParameters
    {
        private readonly string _parameterName;
        private readonly IEnumerable<SqlDataRecord> _rows;
        private readonly string _typeName;

        public TableValueParameter(string parameterName, string typeName, IEnumerable<object> rows)
        {
            _parameterName = parameterName;
            _typeName = typeName;

            var genericTvp = new GenericTableValueParameter(rows);
            _rows = genericTvp;
        }

        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            SqlCommand sqlCommand = null;
            if (typeof(SqlCommand).GetTypeInfo().IsInstanceOfType(command))
            {
                sqlCommand = command.As<SqlCommand>();
            }
            if (sqlCommand == null)
            {
                throw new ArgumentException("Could not convert to a SqlCommand.", $"{typeof(SqlCommand).Name}");
            }

            SqlParameter sqlParameter = sqlCommand.Parameters.Add(_parameterName, SqlDbType.Structured);

            sqlParameter.Direction = ParameterDirection.Input;
            sqlParameter.TypeName = _typeName;
            sqlParameter.Value = _rows;
        }
    }
}
