using Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Test
{
    [TestClass]
    public class TestBuilder
    {
        [TestMethod]
        public void TestCompile()
        {
            var builder = new ProjectBuilder(@"D:\GetCHM\tem\123.hhp");
            builder.Compile();
        }
    }
}
