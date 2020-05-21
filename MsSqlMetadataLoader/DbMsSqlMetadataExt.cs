using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsSqlMetadataLoader
{
    public static class DbMsSqlMetadataExt
    {
        /// <summary>
        /// Filter by database schema
        /// </summary>
        /// <param name="db"></param>
        /// <param name="arg">'+' - include schema, '-' - exclude schema</param>
        /// <param name="schemeArray">перечень схем</param>
        /// <returns></returns>
        public static DbMsSqlMetadata FilterByScheme(this DbMsSqlMetadata db, string arg, params string[] schemeArray)
        {
            if (string.Equals(arg, "+"))
            {
                for (int i = 0; i < db.TableList.Count; i++)
                {
                    MTable t = db.TableList[i];

                    if (!schemeArray.Contains(t.Table_schema))
                    {
                        db.TableList.RemoveAt(i);
                        i--;
                    }

                } // for

            }

            if (string.Equals(arg, "-"))
            {
                for (int i = 0; i < db.TableList.Count; i++)
                {
                    MTable t = db.TableList[i];

                    if (schemeArray.Contains(t.Table_schema))
                    {
                        db.TableList.RemoveAt(i);
                        i--;
                    }

                } // for

            }

            return db;
        }

        /// <summary>
        /// Filter by table
        /// </summary>
        /// <param name="db"></param>
        /// <param name="arg">'+' - include table, '-' - exclude table</param>
        /// <param name="tableArray">array of tables 'schema.table' ('app.AppUser')</param>
        /// <returns></returns>
        public static DbMsSqlMetadata FilterByTable(this DbMsSqlMetadata db, string arg, params string[] tableArray)
        {
            if (string.Equals(arg, "+"))
            {
                for (int i = 0; i < db.TableList.Count; i++)
                {
                    MTable t = db.TableList[i];

                    var tName = $"{t.Table_schema}.{t.Table_name}";

                    if (!tableArray.Contains(tName))
                    {
                        db.TableList.RemoveAt(i);
                        i--;
                    }

                } // for
            }

            if (string.Equals(arg, "-"))
            {
                for (int i = 0; i < db.TableList.Count; i++)
                {
                    MTable t = db.TableList[i];

                    var tName = $"{t.Table_schema}.{t.Table_name}";

                    if (tableArray.Contains(tName))
                    {
                        db.TableList.RemoveAt(i);
                        i--;
                    }

                } // for

            }

            return db;
        }

        public static DbMsSqlMetadata FilterByTable(this DbMsSqlMetadata db, string arg, Action<string> outputWriteFunc, params string[] tableArray)
        {
            if (string.Equals(arg, "+"))
            {
                for (int i = 0; i < db.TableList.Count; i++)
                {
                    MTable t = db.TableList[i];

                    var tName = $"{t.Table_schema}.{t.Table_name}";

                    if (!tableArray.Contains(tName))
                    {
                        outputWriteFunc?.Invoke($"// - remove table {tName}");
                        db.TableList.RemoveAt(i);
                        i--;
                    }

                } // for

            }

            if (string.Equals(arg, "-"))
            {
                for (int i = 0; i < db.TableList.Count; i++)
                {
                    MTable t = db.TableList[i];

                    var tName = $"{t.Table_schema}.{t.Table_name}";

                    if (tableArray.Contains(tName))
                    {
                        db.TableList.RemoveAt(i);
                        i--;
                    }

                } // for

            }

            return db;
        }

        /// <summary>
        /// Set primary key. Some time it need for VIEW.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName">table or view 'app.AppUser'</param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DbMsSqlMetadata SetPk(this DbMsSqlMetadata db, string tableName, string columnName)
        {
            var t = db.TableList.FirstOrDefault(c => $"{c.Table_schema}.{c.Table_name}" == tableName);

            if (t == null)
                throw new Exception($"Table {tableName} not found");

            var col = t.ColumnList.FirstOrDefault(c => c.column_name == columnName);

            if (t == null)
                throw new Exception($"Column {tableName}.{columnName} not found");

            foreach (var c in t.ColumnList)
                c.IsPrimaryKey = string.Equals(c.column_name, columnName);

            //col.IsPrimaryKey = true;

            return db;
        }

        public static MColumn FindColumn(this List<MTable> list, string schema, string table, string column)
        {
            return list
                .FirstOrDefault(c => c.Table_schema == schema && c.Table_name == table)
                ?.ColumnList.FirstOrDefault(c => c.column_name == column);
        }
    }
}
