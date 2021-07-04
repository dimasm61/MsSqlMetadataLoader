using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsSqlMetadataLoader
{
    public class DbItemCommon
    {
        /// <summary>
        /// Field for extend of storage
        /// </summary>
        public object Tag;

        public T GetTag<T>() => (T)Tag;

        protected void Parse(DataRow reader, string colName, ref string outResult)
        {
            if (reader[colName.Trim()] == DBNull.Value)
                outResult = null;
            else
                outResult = (string)reader[colName.Trim()];
        }

        protected void Parse(DataRow reader, string colName, ref int outResult)
        {
            outResult = (int)reader[colName.Trim()];
        }

        protected void Parse(DataRow reader, string colName, ref int? outResult)
        {
            if (reader[colName.Trim()] == DBNull.Value)
                outResult = null;
            else
                outResult = (int?)reader[colName.Trim()];
        }

        protected void Parse(DataRow reader, string colName, ref Int16? outResult)
        {
            if (reader[colName.Trim()] == DBNull.Value)
                outResult = null;
            else
                outResult = (Int16?)reader[colName.Trim()];
        }

        protected void Parse(DataRow reader, string colName, ref byte? outResult)
        {
            if (reader[colName.Trim()] == DBNull.Value)
                outResult = null;
            else
                outResult = (byte?)reader[colName.Trim()];
        }

        protected void Parse(DataRow reader, string colName, ref bool outResult)
        {
            if (reader[colName.Trim()] == DBNull.Value)
                outResult = false;
            else
                outResult = (bool)reader[colName.Trim()];
        }

        protected void Parse(DataRow reader, string colName, ref DateTime? outResult)
        {
            if (reader[colName.Trim()] == DBNull.Value)
                outResult = null;
            else
                outResult = (DateTime?)reader[colName.Trim()];
        }

        protected void Parse(DataRow reader, string colName, ref DateTime outResult)
        {
            if (reader[colName.Trim()] == DBNull.Value)
                outResult = DateTime.MinValue;
            else
                outResult = (DateTime)reader[colName.Trim()];
        }
    }
}
