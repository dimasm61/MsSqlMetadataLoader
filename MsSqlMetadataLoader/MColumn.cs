using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MsSqlMetadataLoader
{
    public class MColumn: DbItemCommon
    {
        public string TableKey;

        /// <summary>String,  Catalog of the table.</summary>
        public string table_catalog;
        /// <summary>String,  Schema that contains the table.</summary>
        public string table_schema;
        /// <summary>String,  Table name.</summary>
        public string table_name;
        /// <summary>String,  Column name.</summary>
        public string column_name;
        /// <summary>Int16,   Column identification number.</summary>
        public int ordinal_position;
        /// <summary>String,  Default value of the column</summary>
        public string column_default;
        /// <summary>String, Nullability of the column.If this column allows NULL, this column returns YES.Otherwise, NO is returned.</summary>
        public string is_nullable;
        /// <summary>String,  System - supplied data type.</summary>
        public string data_type;
        /// <summary>Int32,   Maximum length, in characters, for binary data, character data, or text and image data.Otherwise, NULL is returned.</summary>
        public Int32? character_maximum_length;
        /// <summary>Int32,   Maximum length, in bytes, for binary data, character data, or text and image data.Otherwise, NULL is returned.</summary>
        public Int32? character_octet_length;
        /// <summary>Unsigned Byte,   Precision of approximate numeric data, exact numeric data, integer data, or monetary data.Otherwise, NULL is returned.</summary>
        public byte? numeric_precision;
        /// <summary>Int16,   Precision radix of approximate numeric data, exact numeric data, integer data, or monetary data.Otherwise, NULL is returned.</summary>
        public Int16? numeric_precision_radix;
        /// <summary>Int32,   Scale of approximate numeric data, exact numeric data, integer data, or monetary data.Otherwise, NULL is returned.</summary>
        public Int32? numeric_scale;
        /// <summary>Int16,   Subtype code for datetime and SQL - 92 interval data types.For other data types, NULL is returned.</summary>
        public Int16? datetime_precision;
        /// <summary>String,  Returns master, indicating the database in which the character set is located, if the column is character data or text data type. Otherwise, NULL is returned.</summary>
        public string character_set_catalog;
        /// <summary>String,  Always returns NULL.</summary>
        public string character_set_schema;
        /// <summary>String,  Returns the unique name for the character set if this column is character data or text data type. Otherwise, NULL is returned.</summary>
        public string character_set_name;
        /// <summary>String,  Returns master, indicating the database in which the collation is defined, if the column is character data or text data type. Otherwise, this column is NULL.</summary>
        public string collation_catalog;
        /// <summary>String,  YES if the column has FILESTREAM attribute.</summary>
        public bool is_filestream;

        public bool IsPrimaryKey;
        public string Comment;
        public string PropType;
        public DbType DbType;
        public SqlDbType SqlDbType;


        public override string ToString()
        {
            var s = IsPrimaryKey ? " PK" : "";
            return $"{table_schema}.{table_name}.{column_name}{s}";
        }

        public MColumn()
        {

        }

        public MColumn(MColumn source)
        {
            CopyFrom(source);
        }

        public MColumn(System.Data.DataRow dataRow)
        {
            ToStr(dataRow, "table_catalog            ", ref table_catalog); //  String  
            ToStr(dataRow, "table_schema             ", ref table_schema); //  String  
            ToStr(dataRow, "table_name               ", ref table_name); //  String  
            ToStr(dataRow, "column_name              ", ref column_name); //  String  
            ToInt(dataRow, "ordinal_position         ", ref ordinal_position); //  Int16   
            ToStr(dataRow, "column_default           ", ref column_default); //  String  
            ToStr(dataRow, "is_nullable              ", ref is_nullable); //  String N
            ToStr(dataRow, "data_type                ", ref data_type); //  String  
            ToInt(dataRow, "character_maximum_length ", ref character_maximum_length); //  Int32   
            ToInt(dataRow, "character_octet_length   ", ref character_octet_length); //  Int32   
            ToInt(dataRow, "numeric_precision        ", ref numeric_precision); //  Unsigned
            ToInt(dataRow, "numeric_precision_radix  ", ref numeric_precision_radix); //  Int16   
            ToInt(dataRow, "numeric_scale            ", ref numeric_scale); //  Int32   
            ToInt(dataRow, "datetime_precision       ", ref datetime_precision); //  Int16   
            ToStr(dataRow, "character_set_catalog    ", ref character_set_catalog); //  String  
            ToStr(dataRow, "character_set_schema     ", ref character_set_schema); //  String  
            ToStr(dataRow, "character_set_name       ", ref character_set_name); //  String  
            ToStr(dataRow, "collation_catalog        ", ref collation_catalog); //  String  
            ToInt(dataRow, "is_filestream            ", ref is_filestream); //  String  

            switch (data_type)
            {
                case "image"           : PropType = "byte[]"        ; DbType = DbType.Binary; SqlDbType = SqlDbType.Image; break;
                case "text"            : PropType = "string"        ; DbType = DbType.String; SqlDbType = SqlDbType.Text; break;
                case "binary"          : PropType = "byte[]"        ; DbType = DbType.Binary; SqlDbType = SqlDbType.Binary; break;
                case "tinyint"         : PropType = "byte"          ; DbType = DbType.Byte; SqlDbType = SqlDbType.TinyInt; break;
                case "date"            : PropType = "DateTime"      ; DbType = DbType.Date; SqlDbType = SqlDbType.Date; break;
                case "time"            : PropType = "DateTime"      ; DbType = DbType.Time; SqlDbType = SqlDbType.Time; break;
                case "bit"             : PropType = "bool"          ; DbType = DbType.Boolean; SqlDbType = SqlDbType.Bit; break;
                case "smallint"        : PropType = "short"         ; DbType = DbType.Int16; SqlDbType = SqlDbType.SmallInt; break;
                case "decimal"         : PropType = "decimal"       ; DbType = DbType.Decimal; SqlDbType = SqlDbType.Decimal; break;
                case "int"             : PropType = "int"           ; DbType = DbType.Int32; SqlDbType = SqlDbType.Int; break;
                case "smalldatetime"   : PropType = "DateTime"      ; DbType = DbType.DateTime; SqlDbType = SqlDbType.SmallDateTime; break;
                case "real"            : PropType = "float"         ; DbType = DbType.Single; SqlDbType = SqlDbType.Real; break;
                case "money"           : PropType = "decimal"       ; DbType = DbType.Currency; SqlDbType = SqlDbType.Money; break;
                case "datetime"        : PropType = "DateTime"      ; DbType = DbType.DateTime; SqlDbType = SqlDbType.DateTime; break;
                case "float"           : PropType = "double"        ; DbType = DbType.Double; SqlDbType = SqlDbType.Float; break;
                case "numeric"         : PropType = "double"        ; DbType = DbType.Decimal; SqlDbType = SqlDbType.Decimal; break;
                case "smallmoney"      : PropType = "decimal"       ; DbType = DbType.Currency; SqlDbType = SqlDbType.SmallMoney; break;
                case "datetime2"       : PropType = "DateTime"      ; DbType = DbType.DateTime2; SqlDbType = SqlDbType.DateTime2; break;
                case "bigint"          : PropType = "long"          ; DbType = DbType.Int64; SqlDbType = SqlDbType.BigInt; break;
                case "varbinary"       : PropType = "byte[]"        ; DbType = DbType.Binary; SqlDbType = SqlDbType.VarBinary; break;
                case "timestamp"       : PropType = "byte[]"        ; DbType = DbType.Binary; SqlDbType = SqlDbType.Timestamp; break;
                case "sysname"         : PropType = "string"        ; DbType = DbType.String; SqlDbType = SqlDbType.NVarChar; break;
                case "nvarchar"        : PropType = "string"        ; DbType = DbType.String; SqlDbType = SqlDbType.NVarChar; break;
                case "varchar"         : PropType = "string"        ; DbType = DbType.AnsiString; SqlDbType = SqlDbType.VarChar; break;
                case "ntext"           : PropType = "string"        ; DbType = DbType.String; SqlDbType = SqlDbType.NText; break;
                case "uniqueidentifier": PropType = "Guid"          ; DbType = DbType.Binary; SqlDbType = SqlDbType.UniqueIdentifier; break;
                case "datetimeoffset"  : PropType = "DateTimeOffset"; DbType = DbType.DateTimeOffset; SqlDbType = SqlDbType.DateTimeOffset; break;
                case "sql_variant"     : PropType = "object"        ; DbType = DbType.Binary; SqlDbType = SqlDbType.Variant; break;
                case "xml"             : PropType = "string"        ; DbType = DbType.Xml; SqlDbType = SqlDbType.Xml; break;

                case "char":
                    PropType = character_maximum_length == 1 ? "char" : "string";
                    DbType = DbType.AnsiStringFixedLength;
                    SqlDbType = SqlDbType.Char;
                    break;

                case "nchar":
                    PropType = character_maximum_length == 1 ? "char" : "string";
                    DbType = DbType.StringFixedLength;
                    SqlDbType = SqlDbType.NChar;
                    break;

                //hierarchyid
                //geometry
                //geography
                default: PropType = "byte[]"; DbType = DbType.Binary; SqlDbType = SqlDbType.Binary; break;
            }

            //switch (PropType)
            //{
            //    case "string":
            //    case "object":
            //    case "byte[]": c.IsClass = true; break;
            //}

            var typesWithoutQuestionMark = new[] { "byte[]", "string" };

            if ((is_nullable == "YES") && !typesWithoutQuestionMark.Contains(PropType))// PropType != "byte[]"))
                PropType += "?";

            TableKey = $"{table_catalog}.{table_schema}.{table_name}";
        }

        public void CopyFrom(MColumn c)
        {
            table_catalog = c.table_catalog;
            table_schema = c.table_schema;
            table_name = c.table_name;
            column_name = c.column_name;
            ordinal_position = c.ordinal_position;
            column_default = c.column_default;
            is_nullable = c.is_nullable;
            data_type = c.data_type;
            character_maximum_length = c.character_maximum_length;
            character_octet_length = c.character_octet_length;
            numeric_precision = c.numeric_precision;
            numeric_precision_radix = c.numeric_precision_radix;
            numeric_scale = c.numeric_scale;
            datetime_precision = c.datetime_precision;
            character_set_catalog = c.character_set_catalog;
            character_set_schema = c.character_set_schema;
            character_set_name = c.character_set_name;
            collation_catalog = c.collation_catalog;
            is_filestream = c.is_filestream;
            IsPrimaryKey = c.IsPrimaryKey;
            Comment = c.Comment;
            PropType = c.PropType;
            DbType = c.DbType;
            SqlDbType = c.SqlDbType;

            TableKey = $"{table_catalog}.{table_schema}.{table_name}";
        }
    }
}

