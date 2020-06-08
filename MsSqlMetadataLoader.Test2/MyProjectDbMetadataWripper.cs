using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace MsSqlMetadataLoader.Test2
{
    /// <summary>
    /// The same class you are able to create in your project.
    /// It cam be useful if you have several T4 scripts that have to use one connection string.
    /// </summary>
    public class MyProjectDbMetadataWripper
    {
        public static DbMsSqlMetadata Load()
        {
            // easy way - to set a connection string in code

            // connection string specific for current project
            var connStr = @"Server=localhost\EX2017;Database=VTSDB3;Trusted_Connection=True;";

            // load metadata from db
            var meta = DbMsSqlMetadata.Load(connStr);

            // set extended data
            meta.TableList.ForEach(t =>
            {
                //TableAddAttributes - custom class
                t.Tag = new TableAddAttributes();
                t.GetTag<TableAddAttributes>().AddAttrList.Add("[PropGridAttr]");

                t.ColumnList.ForEach(c => {
                    c.Tag = new ColumnAddAttributes();
                    c.GetTag<ColumnAddAttributes>().AddAttrList.Add("[ColGridAttr]");
                });
            });

            return meta;
        }

        public static DbMsSqlMetadata LoadWithJsonConfig()
        {
            // in some cases, you want to store the connection string in one place
            // for example, for T4 scripts and for test
            
            // in this example I use json file stored in a root path of solution

            // find roor path
            var rootPath = GetSolutionPath("MsSqlMetadataLoader");
            var configFile = $"{rootPath}\\GenerationConfig.json";

            // load joson
            var jsonString = File.ReadAllText(configFile);

            // deserialize to config class
            var testConfig = JsonSerializer.Deserialize<GenerationConfig>(jsonString);

            // convert time out from sec to min
            var timeOutMin = testConfig.ReloadMetadataTimeoutSec / 60.0;

            // load metadata from db
            var meta = DbMsSqlMetadata.Load(testConfig.ConnectionString, timeOutMin);

            return meta;
        }

        public static string GetSolutionPath(string solutionRootFolderName)
        {
            var path = Environment.CurrentDirectory;

            for (var i = path.Length - 1; i > 0; i--)
            {
                if (path.ToLower().EndsWith(solutionRootFolderName.ToLower()))
                    break;

                if (path[i] == '\\')
                {
                    path = path.Substring(0, i);
                    i = path.Length - 1;
                }
            }
            return path;
        }
    }


    /// <summary>
    /// Class example with some extended data
    /// </summary>
    public class TableAddAttributes
    {
        public List<string> AddAttrList = new List<string>();
    }

    public class ColumnAddAttributes
    {
        public List<string> AddAttrList = new List<string>();
    }

    public class GenerationConfig
    {
        public string ConnectionString { get; set; }
        public int ReloadMetadataTimeoutSec { get; set; }
    }
}
