using ApprovalTests;
using ApprovalTests.Reporters;
using DiffMatchPatch;
using NUnit.Framework;

namespace DiffMatchPatchTests
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class ExtensionTests
    {
        private const string expected = "start\n" +
                                        "line with some content\n" +
                                        "unchanged content\n" +
                                        "removed line\n" +
                                        "end";

        private const string actual = "start\n" +
                                      "new line\n" +
                                      "line with some modified content\n" +
                                      "unchanged content\n" +
                                      "end";

        [Test]
        public void CorrectPrettyTextOutput()
        {
            var dmp = new diff_match_patch();

            var diffs = dmp.diff_main(expected, actual);

            dmp.diff_cleanupForPrettyOutput(diffs);

            var (patchCount, prettyOutput) = dmp.diff_toPrettyText(diffs);

            Approvals.Verify(prettyOutput);
            Assert.AreEqual(3, patchCount);
        }

        [Test]
        public void CorrectPrettyHtmlOutput()
        {
            var dmp = new diff_match_patch();

            var diffs = dmp.diff_main(expected, actual);

            dmp.diff_cleanupForPrettyOutput(diffs);

            var prettyOutput = dmp.diff_toPrettyHtml(diffs);

            Approvals.Verify(prettyOutput);
        }
    }
}
