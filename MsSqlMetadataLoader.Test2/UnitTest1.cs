using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsSqlMetadataLoader.Test2
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void LoadMetadataTest()
        {
            var dbMeta = DbMsSqlMetadata.Load("Server=localhost\\EX2017;Database=VTSDB3;Trusted_Connection=True;", 0.1);
            Assert.AreEqual(dbMeta.TableList.Any(), true);
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
