using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Data;

namespace GYBGeneralOperations.Extensions
{
    public class DBHelper
    {
        public class DataCollection<T> : List<T>, IEnumerable<SqlDataRecord> where T : class, new()
        {
            IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
            {
                List<SqlMetaData> records = new List<SqlMetaData>();
                var properties = typeof(T).GetProperties();
                foreach (var prop in properties)
                {
                    SqlDbType sdbtyp = GetSqlType(prop.PropertyType);
                    if (sdbtyp == SqlDbType.VarChar)
                    {
                        records.Add(new SqlMetaData(prop.Name, sdbtyp, 8000));
                    }
                    else
                    {
                        records.Add(new SqlMetaData(prop.Name, sdbtyp));
                    }
                }
                SqlDataRecord ret = new SqlDataRecord(records.ToArray());

                foreach (T data in this)
                {
                    for (int i = 0; i < properties.Length; i++)
                    {
                        ret.SetValue(i, properties[i].GetValue(data, null));
                    }
                    yield return ret;
                }
            }

            // change C# types to SqlDbType
            private SqlDbType GetSqlType(Type type)
            {
                SqlDbType val = SqlDbType.VarChar;
                if (type == typeof(Int32) || type == typeof(Nullable<Int32>))
                {
                    val = SqlDbType.Int;
                }
                if (type == typeof(Int64) || type == typeof(Nullable<Int64>))
                {
                    val = SqlDbType.BigInt;
                }
                else if (type == typeof(Byte[]))
                {
                    val = SqlDbType.Binary;
                }
                else if (type == typeof(Boolean) || type == typeof(Nullable<Boolean>))
                {
                    val = SqlDbType.Bit;
                }
                else if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
                {
                    val = SqlDbType.DateTime;
                }
                else if (type == typeof(Decimal))
                {
                    val = SqlDbType.Decimal;
                }
                else if (type == typeof(Double))
                {
                    val = SqlDbType.Float;
                }
                return val;
            }
        }
    }
}
