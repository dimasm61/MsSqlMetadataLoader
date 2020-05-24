using NUnit.Framework;

namespace MsSqlMetadataLoader.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LoadMetadataTest()
        {
            var dbMeta = DbMsSqlMetadata.Load("Server=localhost\\EX2017;Database=VTSDB3;Trusted_Connection=True;", 0.1);
 
            Assert.Pass();
        }

        [Test]
        public void LoadWithWripper()
        {
            var dbMeta = MyProjectDbMetadataWripper.Load();
        }
    }
}