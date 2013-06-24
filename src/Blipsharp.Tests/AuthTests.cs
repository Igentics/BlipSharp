using BlipSharp.Resources;
using NUnit.Framework;

namespace BlipSharp.Tests
{
    [TestFixture]
    public class AuthTests : TestBase
    {
        [Test]
        public void TestNonce()
        {
            var authentication = new BlipSharp.AuthToken(Api.Context.ApiSecret, Time.Get(Api), "U4354Q");
            authentication.Token = string.Empty;
            var token = Token.Get(Api, "U4354Q", authentication.Nonce, authentication.Timestamp.Timestamp, authentication.Signature);
        }
    }
}