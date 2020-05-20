namespace MsSqlMetadataLoader
{
    public class MIndex
    {
        public string TableKey;

        public string constraint_catalog;// String	Catalog that index belongs to.
        public string constraint_schema;// String	Schema that contains the index.
        public string constraint_name;// String	Name of the index.
        public string table_catalog;// String	Table name the index is associated with.
        public string table_schema;// String	Schema that contains the table the index is associated with.
        public string table_name;// String	Table Name.
        public string column_name;

        public override string ToString()
        {
            return $"{table_schema}.{table_name}.{constraint_name}";
        }

        public MIndex()
        {

        }
        public MIndex(MIndex source)
        {
            CopyFrom(source);
        }

        public MIndex(System.Data.DataRow dataRow)
        {
            constraint_catalog = dataRow["constraint_catalog"]?.ToString();
            constraint_schema = dataRow["constraint_schema"]?.ToString();
            constraint_name = dataRow["constraint_name"]?.ToString();
            table_catalog = dataRow["table_catalog"]?.ToString();
            table_schema = dataRow["table_schema"]?.ToString();
            table_name = dataRow["table_name"]?.ToString();
            column_name = dataRow["column_name"]?.ToString();

            TableKey = $"{table_catalog}.{table_schema}.{table_name}";
        }

        public void CopyFrom(MIndex source)
        {
            constraint_catalog = source.constraint_catalog;
            constraint_schema = source.constraint_schema;
            constraint_name = source.constraint_name;
            table_catalog = source.table_catalog;
            table_schema = source.table_schema;
            table_name = source.table_name;
            column_name = source.column_name;

            TableKey = $"{table_catalog}.{table_schema}.{table_name}";
        }
    }
}

