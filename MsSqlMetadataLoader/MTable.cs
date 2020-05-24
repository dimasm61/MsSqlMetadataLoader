using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsSqlMetadataLoader
{
    public class MTable
    {
        /// <summary>
        /// Field for extend of storage
        /// </summary>
        public object Tag;

        public string TableKey => $"{Table_catalog}.{Table_schema}.{Table_name}";

        public string Table_catalog;// String	Catalog of the table.
        public string Table_schema;// String	Schema that contains the table.
        public string Table_name;// String	Table name.
        public string Table_type;// String	Type of table. Can be VIEW or BASE TABLE.

        public readonly List<MColumn> ColumnList = new List<MColumn>();
        public readonly List<MForeignKey> FkList = new List<MForeignKey>();
        public readonly List<MIndex> IdxList = new List<MIndex>();

        public string Comment;

        public override string ToString() => $"{Table_schema}.{Table_name}";

        public MTable()
        {

        }

        public MTable(MTable source)
        {
            CopyFrom(source);
        }

        public MTable(System.Data.DataRow dataRow)
        {
            Table_catalog = dataRow["table_catalog"]?.ToString();
            Table_schema = dataRow["table_schema"]?.ToString();
            Table_name = dataRow["table_name"]?.ToString();
            Table_type = dataRow["table_type"]?.ToString();
        }

        public void CopyFrom(MTable t)
        {
            Table_catalog = t.Table_catalog;
            Table_schema = t.Table_schema;
            Table_name = t.Table_name;
            Table_type = t.Table_type;

            ColumnList.Clear();
            FkList.Clear();
            IdxList.Clear();

            ColumnList.AddRange(t.ColumnList.Select(c => new MColumn(c)));

            FkList.AddRange(t.FkList.Select(c => new MForeignKey(c)));

            IdxList.AddRange(t.IdxList.Select(c => new MIndex(c)));

        }
    }
}

