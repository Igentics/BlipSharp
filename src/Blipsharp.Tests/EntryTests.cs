using BlipSharp.Resources;
using NUnit.Framework;

namespace BlipSharp.Tests
{
    [TestFixture]
    public class EntryTests : TestBase
    {
        [Test]
        public void TestEntry()
        {
            var entry = Entry.Get(Api, display_name: "davidd");
            Assert.IsNotNull(entry);
        }

        [Test]
        public void TestEntryAdvanced()
        {
            var entry = Entry.Get(Api, display_name: "davidd", returnActions: true, returnExtended: true, return_comments: true, return_exif: true);
            Assert.IsNotNull(entry);
            Assert.IsNotNull(entry.Exif);
            Assert.IsNullOrEmpty(entry.RawDescription);
            Assert.IsNotNull(entry.Comments);
            Assert.IsNotEmpty(entry.Comments);
        }

    }
}
