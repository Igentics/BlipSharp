using BlipSharp.Resources;
using NUnit.Framework;

namespace BlipSharp.Tests
{
    [TestFixture]
    public class TimeTests : TestBase
    {
        [Test]
        public void TestTime()
        {
            var time = Api.Get<Time>();
            Assert.IsNotNull(time, "Time was null");
        }
    }
}