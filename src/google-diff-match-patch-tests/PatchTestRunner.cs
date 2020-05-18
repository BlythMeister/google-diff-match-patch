using NUnit.Framework;

namespace DiffMatchPatchTests
{
    [TestFixture]
    public class PatchTestRunner
    {
        [Test]
        public void patch_patchObjTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.patch_patchObjTest());
        }

        [Test]
        public void patch_fromTextTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.patch_fromTextTest());
        }

        [Test]
        public void patch_toTextTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.patch_toTextTest());
        }

        [Test]
        public void patch_addContextTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.patch_addContextTest());
        }

        [Test]
        public void patch_makeTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.patch_makeTest());
        }

        [Test]
        public void patch_splitMaxTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.patch_splitMaxTest());
        }

        [Test]
        public void patch_addPaddingTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.patch_addPaddingTest());
        }

        [Test]
        public void patch_applyTest()
        {
            var dmp = new diff_match_patchTest();

            Assert.DoesNotThrow(() => dmp.patch_applyTest());
        }
    }
}
