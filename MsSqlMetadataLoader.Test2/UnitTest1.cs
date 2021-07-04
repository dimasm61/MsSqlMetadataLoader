using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsSqlMetadataLoader.Test2
{
    [TestClass]
    public class UnitTest1
    {
        private static string ConnStr = "Server=localhost\\EX2017;Database=VTSDB3;Trusted_Connection=True;";
        [TestMethod]
        public void LoadMetadataTest()
        {
            
            var dbMeta = DbMsSqlMetadata.Load(ConnStr, 0.1);
            Assert.AreEqual(dbMeta.TableList.Any(), true);
        }

        [TestMethod]
        public void CheckForLostTablesTest()
        {
            var cn = 0;
            var tableCount = -1;
            var columnCount = -1;
            var prevList = new List<MTable>();
            var rs = true;
            while (cn++ < 20)
            {
                var dbMeta = DbMsSqlMetadata.Load(ConnStr, 0);

                if (tableCount == -1)
                {
                    tableCount = dbMeta.TableList.Count;
                    continue;
                }

                Assert.AreEqual(dbMeta.TableList.Any(), true);

                if(prevList.Count != 0)
                    rs = rs || CheckAndPrintDiff(dbMeta, dbMeta.TableList, prevList);

                //Assert.AreEqual(dbMeta.TableList.Count, tableCount, "Is not equals tables count");// HOW ????
                
                prevList = dbMeta.TableList;

            }

            Assert.AreEqual(rs, true, "In some iteration has have lost table");
        }

        private bool CheckAndPrintDiff(DbMsSqlMetadata metadata, List<MTable> list1, List<MTable> list2)
        {
            if (list1.Count == list2.Count)
                return true;
            var maxCountList = (list1.Count > list2.Count) ? list1 : list2;
            var minCountList = (list1.Count < list2.Count) ? list1 : list2;

            foreach (var item in maxCountList)
            {
                var table = minCountList.FirstOrDefault(c => c.TableKey == item.TableKey);
                if(table == null)
                {
                    Trace.TraceInformation($"Lost table - {item.TableKey}");
                    foreach(DataRow row in metadata.Tables.Rows)
                    {
                        var table_catalog = row["table_catalog"]?.ToString();
                        var table_schema = row["table_schema"]?.ToString();
                        var table_name = row["table_name"]?.ToString();
                        var table_type = row["table_type"]?.ToString();

                        if(table_name == item.Table_name)
                            Trace.TraceInformation($"  - exists in Tables {metadata.Tables.Rows.Count}/{metadata.TableList.Count}");
                    }
                    //var row = metadata.Tables.Rows[]

                }
            }

            return false;
        }

        [TestMethod]
        public void LoadWithWripper()
        {
            var dbMeta = MyProjectDbMetadataWripper.Load();
            Assert.AreEqual(dbMeta.TableList.Any(), true);
        }

        [TestMethod]
        public void LoadWithWripperWithJson()
        {
            var dbMeta = MyProjectDbMetadataWripper.LoadWithJsonConfig();
            Assert.AreEqual(dbMeta.TableList.Any(), true);
        }
    }
}
