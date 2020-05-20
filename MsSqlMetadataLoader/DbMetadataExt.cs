using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsSqlMetadataLoader
{
    public static class DbMetadataExt
    {
        /// <summary>
        /// Фильтрация по схеме
        /// </summary>
        /// <param name="db"></param>
        /// <param name="arg">'+' - оставить только эту схему, '-' - все кроме этой схемы</param>
        /// <param name="schemeArray">перечень схем</param>
        /// <returns></returns>
        public static DbMetadata FilterByScheme(this DbMetadata db, string arg, params string[] schemeArray)
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
        /// Фильтрация по таблице
        /// </summary>
        /// <param name="db"></param>
        /// <param name="arg">'+' - оставить только эту таблицу, '-' - все кроме этой таблицы</param>
        /// <param name="tableArray">перечень таблиц в формате 'app.AppUser'</param>
        /// <returns></returns>
        public static DbMetadata FilterByTable(this DbMetadata db, string arg, params string[] tableArray)
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

        public static DbMetadata FilterByTable(this DbMetadata db, string arg, Microsoft.VisualStudio.TextTemplating.TextTransformation tt, params string[] tableArray)
        {
            if (string.Equals(arg, "+"))
            {
                for (int i = 0; i < db.TableList.Count; i++)
                {
                    MTable t = db.TableList[i];

                    var tName = $"{t.Table_schema}.{t.Table_name}";

                    if (!tableArray.Contains(tName))
                    {
                        tt.WriteLine($"// - remove table {tName}");
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
        /// Указать первичный ключ
        /// </summary>
        /// <param name="db"></param>
        /// <param name="tableName">таблица в формате 'app.AppUser'</param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static DbMetadata SetPk(this DbMetadata db, string tableName, string columnName)
        {
            var t = db.TableList.FirstOrDefault(c => $"{c.Table_schema}.{c.Table_name}" == tableName);

            if (t == null)
                throw new Exception($"Таблица {tableName} не найдена");

            var col = t.ColumnList.FirstOrDefault(c => c.column_name == columnName);

            if (t == null)
                throw new Exception($"Столбец {tableName}.{columnName} не найден");

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
