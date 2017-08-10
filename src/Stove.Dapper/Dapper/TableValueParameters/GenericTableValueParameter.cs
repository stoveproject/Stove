using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;

using Castle.Core.Internal;

using Microsoft.SqlServer.Server;

namespace Stove.Dapper.TableValueParameters
{
	internal class GenericTableValueParameter : IEnumerable<SqlDataRecord>
	{
		private readonly IEnumerable<object> _tableValueList;

		public GenericTableValueParameter(IEnumerable<object> tableValueList)
		{
			_tableValueList = tableValueList;
		}

		public IEnumerator<SqlDataRecord> GetEnumerator()
		{
			Type type = _tableValueList.GetType().GetGenericArguments().Single();
			PropertyInfo[] properties = type.GetProperties();
			var metaData = new SqlMetaData[properties.Length];

			for (var i = 0; i < properties.Length; i++)
			{
				PropertyInfo property = properties[i];

				var columnNameAttribute = property.GetAttribute<ColumnAttribute>();
				string name = columnNameAttribute != null ? columnNameAttribute.Name : property.Name;

				SqlDbType dbType = TypeToSqlDbTypeMap.GetSqlDbType(property.PropertyType);
				if (dbType == SqlDbType.NVarChar)
				{
					var length = 255;
					var lengthAttribute = property.GetAttribute<MaxLengthAttribute>();
					if (lengthAttribute != null)
					{
						length = lengthAttribute.Length;
					}
					metaData[i] = new SqlMetaData(name, dbType, length);
				}
				else
				{
					metaData[i] = new SqlMetaData(name, dbType);
				}
			}

			foreach (object item in _tableValueList)
			{
				var sqlDataRecord = new SqlDataRecord(metaData);
				try
				{
					object[] values = properties.Select(x => x.GetValue(item, null)).ToArray();
					sqlDataRecord.SetValues(values);
				}
				catch (Exception exception)
				{
					throw new ArgumentException("An error occured while setting SqlDbValues.", exception);
				}

				yield return sqlDataRecord;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
