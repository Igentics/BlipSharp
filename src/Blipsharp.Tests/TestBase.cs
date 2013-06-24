using System.Configuration;
using NUnit.Framework;

namespace BlipSharp.Tests
{
    public class TestBase
    {
        [SetUp]
        public void Setup()
        {
            Api = new Api(new ApiContext {
                ApiKey = ConfigurationManager.AppSettings["BlipSharp.ApiKey"],
                ApiSecret = ConfigurationManager.AppSettings["BlipSharp.ApiSecret"],
                PermissionsURL = ConfigurationManager.AppSettings["BlipSharp.AuthenticationUrl"]
            });
        }

        [TearDown]
        public void TearDown()
        {
            Api = null;
            Assert.IsNull(Api);
        }

        public Api Api { get; set; }
    }
}