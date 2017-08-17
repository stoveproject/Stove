using System;
using System.Collections.Generic;
using System.Data;

namespace Stove.Dapper.TableValueParameters
{
    internal class TypeToSqlDbTypeMap
    {
        private static readonly Dictionary<Type, SqlDbType> typeMap = new Dictionary<Type, SqlDbType>
        {
            [typeof(string)] = SqlDbType.NVarChar,
            [typeof(char[])] = SqlDbType.NVarChar,
            [typeof(byte)] = SqlDbType.TinyInt,
            [typeof(short)] = SqlDbType.SmallInt,
            [typeof(int)] = SqlDbType.Int,
            [typeof(long)] = SqlDbType.BigInt,
            [typeof(byte[])] = SqlDbType.Image,
            [typeof(bool)] = SqlDbType.Bit,
            [typeof(DateTime)] = SqlDbType.DateTime2,
            [typeof(DateTimeOffset)] = SqlDbType.DateTimeOffset,
            [typeof(decimal)] = SqlDbType.Money,
            [typeof(float)] = SqlDbType.Real,
            [typeof(double)] = SqlDbType.Float,
            [typeof(TimeSpan)] = SqlDbType.Time
        };

        public static SqlDbType GetSqlDbType(Type givenType)
        {
            givenType = Nullable.GetUnderlyingType(givenType) ?? givenType;
            if (typeMap.ContainsKey(givenType))
            {
                return typeMap[givenType];
            }

            throw new ArgumentException($"{givenType.FullName} is not a supported .NET class");
        }

        public static SqlDbType GetSqlDbType<T>()
        {
            return GetSqlDbType(typeof(T));
        }
    }
}
