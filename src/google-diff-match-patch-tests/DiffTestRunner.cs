using NUnit.Framework;

namespace DiffMatchPatchTests
{
    [TestFixture]
    public class DiffTestRunner
    {
        [Test]
        public void diff_commonPrefixTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_commonPrefixTest());
        }

        [Test]
        public void diff_commonSuffixTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_commonSuffixTest());
        }

        [Test]
        public void diff_commonOverlapTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_commonOverlapTest());
        }

        [Test]
        public void diff_halfmatchTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_halfmatchTest());
        }

        [Test]
        public void diff_linesToCharsTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_linesToCharsTest());
        }

        [Test]
        public void diff_charsToLinesTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_charsToLinesTest());
        }

        [Test]
        public void diff_cleanupMergeTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_cleanupMergeTest());
        }

        [Test]
        public void diff_cleanupSemanticLosslessTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_cleanupSemanticLosslessTest());
        }

        [Test]
        public void diff_cleanupSemanticTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_cleanupSemanticTest());
        }

        [Test]
        public void diff_cleanupEfficiencyTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_cleanupEfficiencyTest());
        }

        [Test]
        public void diff_prettyHtmlTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_prettyHtmlTest());
        }

        [Test]
        public void diff_textTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_textTest());
        }

        [Test]
        public void diff_deltaTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_deltaTest());
        }

        [Test]
        public void diff_xIndexTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_xIndexTest());
        }

        [Test]
        public void diff_levenshteinTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_levenshteinTest());
        }

        [Test]
        public void diff_bisectTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_bisectTest());
        }

        [Test]
        public void diff_mainTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.diff_mainTest());
        }
    }
}
