namespace MsSqlMetadataLoader
{
    public class MForeignKey
    {
        public string TableKey;
        public string constraint_catalog;// String  Catalog the constraint belongs to.
        public string constraint_schema;// String  Schema that contains the constraint.
        public string constraint_name;// String  Name.
        public string table_catalog;// String  Table Name constraint is part of.
        public string table_schema;// String  Schema that that contains the table.
        public string table_name;// String  Table Name
        public string constraint_type;// String  Type of constraint.Only "FOREIGN KEY" is allowed.
        public string is_deferrable;// String  Specifies whether the constraint is deferrable.Returns NO.
        public string initially_deferred;// String  Specifies whether the constraint is initially deferrable. Returns NO.


        public override string ToString()
        {
            return $"{table_schema}.{table_name}.{constraint_name} - {constraint_type}";
        }

        public MForeignKey()
        {

        }

        public MForeignKey(MForeignKey source)
        {
            CopyFrom(source);
        }

        public MForeignKey(System.Data.DataRow dataRow)
        {
            constraint_catalog = dataRow["constraint_catalog"]?.ToString();
            constraint_schema = dataRow["constraint_schema"]?.ToString();
            constraint_name = dataRow["constraint_name"]?.ToString();
            table_catalog = dataRow["table_catalog"]?.ToString();
            table_schema = dataRow["table_schema"]?.ToString();
            table_name = dataRow["table_name"]?.ToString();
            constraint_type = dataRow["constraint_type"]?.ToString();
            is_deferrable = dataRow["is_deferrable"]?.ToString();
            initially_deferred = dataRow["initially_deferred"]?.ToString();

            TableKey = $"{table_catalog}.{table_schema}.{table_name}";
        }

        public void CopyFrom(MForeignKey source)
        {
            constraint_catalog = source.constraint_catalog;
            constraint_schema = source.constraint_schema;
            constraint_name = source.constraint_name;
            table_catalog = source.table_catalog;
            table_schema = source.table_schema;
            table_name = source.table_name;
            constraint_type = source.constraint_type;
            is_deferrable = source.is_deferrable;
            initially_deferred = source.initially_deferred;

            TableKey = $"{table_catalog}.{table_schema}.{table_name}";
        }
    }
}

