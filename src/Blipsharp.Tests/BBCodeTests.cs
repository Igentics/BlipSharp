using System;
using BlipSharp.Utilities;
using NUnit.Framework;

namespace BlipSharp.Tests
{
    [TestFixture]
    public class QueryStringTests : TestBase
    {
        [Test]
        public void TestQueryStringBooleanValue()
        {
            Assert.AreEqual("1", QueryStringUtilities.AsBooleanValue(true));
            Assert.AreEqual("0", QueryStringUtilities.AsBooleanValue(false));
            Assert.AreNotEqual("1", QueryStringUtilities.AsBooleanValue(false));
        }

        [Test]
        public void TestQuerystringEncoding()
        {
            Assert.AreEqual("%20", QueryStringUtilities.Encode(" "));
        }
    }
}
