using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MsSqlMetadataLoader
{
    public class DbMsSqlMetadata
    {
        private static List<MTable> _tableList = new List<MTable>();
        private static DateTime _tableListCreateTime = DateTime.MinValue;

        public List<MTable> TableList = new List<MTable>();


        public DataTable Tables;

        private static readonly object _lck = new object();

        /// <summary>
        /// <example> var db1 = DbMetadata.Load().FilterByScheme("+", "nav", "usr").FilterByTable("+","nav.Pass","usr.Watch"); </example>
        /// </summary>
        /// <returns></returns>
        public static DbMsSqlMetadata Load(string connStr, double minTimeOut = 1)
        {
            var result = new DbMsSqlMetadata();
            lock (_lck)
            {
                var sw = Stopwatch.StartNew();

                if (_tableList.Any() && (DateTime.Now - _tableListCreateTime).TotalSeconds < minTimeOut * 60)
                {
                    CopyTableList(ref _tableList, ref result.TableList);
                    return result;
                }

                using (var conn = new SqlConnection(connStr))
                {
                    conn.Open();

                    // tables
                    var tables = conn.GetSchema("Tables", new string[] { null, null, null, null });
                    var tableList = (from DataRow table in tables.Rows select new MTable(table)).ToList();

                    // if(tables.Rows.Count != tableList.Count)
                    // {
                    //     var s = 1;
                    // }

                    result.Tables = tables;

                    // columns
                    var columns = conn.GetSchema("Columns", new string[] { null, null, null });
                    var columnList = (from DataRow column in columns.Rows select new MColumn(column)).ToList();

                    // indexes
                    var idxColumns = conn.GetSchema("IndexColumns", new string[] { null, null, null });
                    var idxList = (from DataRow index in idxColumns.Rows select new MIndex(index)).ToList();

                    // constraints
                    var foreignKeys = conn.GetSchema("ForeignKeys", new string[] { null, null, null, null });
                    var fkList = (from DataRow fKey in foreignKeys.Rows select new MForeignKey(fKey)).ToList();
                    
                    // column descriptions
                    var commentList = GetColumnComments(conn);

                    // set comments into columns
                    foreach (var cm in commentList)
                    {
                        var col = columnList.FirstOrDefault(c => c.TableKey == cm.TableKey && c.column_name == cm.column_name);
                        if (col != null)
                            col.Comment = cm.comment;
                    }
                    var lck = new object();
                    Action<MTable> funcTable = (t) =>
                    {

                        try
                        {
                            lock (lck)
                            {
                                result.TableList.Add(t);
                            }

                            t.ColumnList.AddRange(columnList.Where(c => c.TableKey == t.TableKey));

                            t.IdxList.AddRange(idxList.Where(c => c.TableKey == t.TableKey));

                            t.FkList.AddRange(fkList.Where(c => c.TableKey == t.TableKey));

                            var pkCol = t.ColumnList.FirstOrDefault(c => c.ordinal_position == 1);
                            if (pkCol != null)
                                pkCol.IsPrimaryKey = true;
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceInformation(ex.Message);
                        }
                    };

                    Parallel.ForEach(tableList, funcTable);

                }// using

                result.TableList = result.TableList.OrderBy(c => c.TableKey).ToList();

                CopyTableList(ref result.TableList, ref _tableList);
                _tableListCreateTime = DateTime.Now;
                sw.Stop();
            }
            return result;
        }



        private static List<CommentInfo> GetColumnComments(SqlConnection conn)
        {
            var commentList = new List<CommentInfo>();

            var getCommentSql
                = " select col.table_catalog,col.table_schema,col.table_name,col.column_name, ext.value as comment \r\n"
                + " from information_schema.columns as col                                                         \r\n"
                + " join sys.extended_properties as ext                                                            \r\n"
                + " on object_id(col.table_catalog + '.' + col.table_schema + '.' + col.table_name) = ext.major_id \r\n"
                + " and col.ordinal_position = ext.minor_id                                                        \r\n"
                ;
            var cmd = new SqlCommand(getCommentSql, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                commentList.Add(new CommentInfo(reader));

            return commentList;
        }

        class CommentInfo
        {
            public string TableKey;

            public string table_catalog;//String  Catalog of the table.
            public string table_schema;//String  Schema that contains the table.
            public string table_name;//String  Table name.
            public string column_name;//String  Column name.
            public string comment;

            public CommentInfo(IDataReader dataReader)
            {
                table_catalog = dataReader["table_catalog"]?.ToString();
                table_schema = dataReader["table_schema"]?.ToString();
                table_name = dataReader["table_name"]?.ToString();
                column_name = dataReader["column_name"]?.ToString();
                comment = dataReader["comment"]?.ToString();

                TableKey = $"{table_catalog}.{table_schema}.{table_name}";
            }
        }

        private static void CopyTableList(ref List<MTable> source, ref List<MTable> dest)
        {
            if (dest == null)
                dest = new List<MTable>();
            dest.Clear();
            var list = dest;

            source.ForEach(t => { list.Add(new MTable(t)); });
        }


    }
}
