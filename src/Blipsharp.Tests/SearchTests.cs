using BlipSharp.Resources;
using NUnit.Framework;

namespace BlipSharp.Tests
{
    [TestFixture]
    public class SearchTests : TestBase
    {
        [Test]
        public void TestSearch()
        {
            var s = Api.Get<Search[]>("query=" + "davidd").Result;
            Assert.IsTrue(s.Length > 0);

        }
    }
}