using ApprovalTests;
using ApprovalTests.Reporters;
using DiffMatchPatch;
using NUnit.Framework;

namespace DiffMatchPatchTests
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class DiffMatchPatchExtensionTests
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

        private const string expectedHtml = "<p>start</p>\n" +
                                            "<p>line with some content</p>\n" +
                                            "<p>unchanged content</p>\n" +
                                            "<p>removed line</p>\n" +
                                            "<p>end</p>";

        private const string actualHtml = "<p>start</p>\n" +
                                          "<p>new line</p>\n" +
                                          "<p>line with some modified content</p>\n" +
                                          "<p>unchanged content</p>\n" +
                                          "<p>end</p>";

        [Test]
        public void CorrectPrettyTextOutput_TextInput()
        {
            var dmp = new diff_match_patch();

            var diffs = dmp.diff_main(expected, actual);

            dmp.diff_cleanupForPrettyOutput(diffs);

            var (patchCount, prettyOutput) = dmp.diff_toPrettyText(diffs);

            Approvals.Verify(prettyOutput);
            Assert.AreEqual(3, patchCount);
        }

        [Test]
        public void CorrectPrettyHtmlOutput_TextInput()
        {
            var dmp = new diff_match_patch();

            var diffs = dmp.diff_main(expected, actual);

            dmp.diff_cleanupForPrettyOutput(diffs);

            var prettyOutput = dmp.diff_toPrettyHtml(diffs);

            Approvals.Verify(prettyOutput);
        }

        [Test]
        public void CorrectPrettyHtmlDocOutput_TextInput()
        {
            var dmp = new diff_match_patch();

            var diffs = dmp.diff_main(expected, actual);

            dmp.diff_cleanupForPrettyOutput(diffs);

            var prettyOutput = dmp.diff_toPrettyHtmlDoc(diffs);

            Approvals.Verify(prettyOutput);
        }

        [Test]
        public void CorrectPrettyTextOutput_HtmlInput()
        {
            var dmp = new diff_match_patch();

            var diffs = dmp.diff_main(expectedHtml, actualHtml);

            dmp.diff_cleanupForPrettyOutput(diffs);

            var (patchCount, prettyOutput) = dmp.diff_toPrettyText(diffs);

            Approvals.Verify(prettyOutput);
            Assert.AreEqual(3, patchCount);
        }

        [Test]
        public void CorrectPrettyHtmlOutput_HtmlInput()
        {
            var dmp = new diff_match_patch();

            var diffs = dmp.diff_main(expectedHtml, actualHtml);

            dmp.diff_cleanupForPrettyOutput(diffs);

            var prettyOutput = dmp.diff_toPrettyHtml(diffs);

            Approvals.Verify(prettyOutput);
        }

        [Test]
        public void CorrectPrettyHtmlDocOutput_HtmlInput()
        {
            var dmp = new diff_match_patch();

            var diffs = dmp.diff_main(expectedHtml, actualHtml);

            dmp.diff_cleanupForPrettyOutput(diffs);

            var prettyOutput = dmp.diff_toPrettyHtmlDoc(diffs);

            Approvals.Verify(prettyOutput);
        }
    }
}
