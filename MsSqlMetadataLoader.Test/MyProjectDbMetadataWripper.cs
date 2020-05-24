using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MsSqlMetadataLoader.Test
{
    public class MyProjectDbMetadataWripper
    {
        public static DbMsSqlMetadata Load()
        {
            // connection string specific for current project
            var connStr = @"Server=localhost\EX2017;Database=VTSDB3;Trusted_Connection=True;";

            // load metadata from db
            var meta = DbMsSqlMetadata.Load(connStr);

            // set extended data
            meta.TableList.ForEach(t =>
            {
                t.Tag = new TableAddAttributes();
                ((TableAddAttributes)t.Tag).AddAttrList.Add("[PropGridAttr]");
            });

            return meta;
        }
    }


    /// <summary>
    /// Class example with some extended data
    /// </summary>
    public class TableAddAttributes
    {
        public List<string> AddAttrList = new List<string>();
    }
}
