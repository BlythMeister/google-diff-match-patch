using NUnit.Framework;

namespace DiffMatchPatchTests
{
    [TestFixture]
    public class MatchTestRunner
    {
        [Test]
        public void match_alphabetTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.match_alphabetTest());
        }

        [Test]
        public void match_bitapTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.match_bitapTest());
        }

        [Test]
        public void match_mainTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.match_mainTest());
        }
    }
}
