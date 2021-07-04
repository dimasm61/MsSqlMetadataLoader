namespace MsSqlMetadataLoader
{
    public class MIndex: DbItemCommon
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
            Parse(dataRow, "constraint_catalog", ref constraint_catalog);
            Parse(dataRow, "constraint_schema ", ref constraint_schema );
            Parse(dataRow, "constraint_name   ", ref constraint_name   );
            Parse(dataRow, "table_catalog     ", ref table_catalog     );
            Parse(dataRow, "table_schema      ", ref table_schema      );
            Parse(dataRow, "table_name        ", ref table_name        );
            Parse(dataRow, "column_name       ", ref column_name       );

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

