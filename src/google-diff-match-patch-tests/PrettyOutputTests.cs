using ApprovalTests;
using ApprovalTests.Reporters;
using DiffMatchPatch;
using NUnit.Framework;

namespace DiffMatchPatchTests
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class PrettyOutputTests
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
        public void CorrectPatchRawTextOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(patches.ToText());
            Assert.AreEqual(3, patches.Count);
        }

        [Test]
        public void CorrectPatchTextOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(patches.ToReadableText());
            Assert.AreEqual(3, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(patches.ToHtml());
            Assert.AreEqual(3, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlDocOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(patches.ToHtmlDocument());
            Assert.AreEqual(3, patches.Count);
        }

        [Test]
        public void CorrectDiffRawTextOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            Approvals.Verify(diffs.ToText());
        }

        [Test]
        public void CorrectDiffTextOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            Approvals.Verify(diffs.ToReadableText());
        }

        [Test]
        public void CorrectDiffHtmlOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            Approvals.Verify(diffs.ToHtml());
        }

        [Test]
        public void CorrectDiffHtmlDocOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            Approvals.Verify(diffs.ToHtmlDocument());
        }

        [Test]
        public void CorrectPatchRawTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(patches.ToText());
            Assert.AreEqual(3, patches.Count);
        }

        [Test]
        public void CorrectPatchTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(patches.ToReadableText());
            Assert.AreEqual(3, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlEncodedTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(patches.ToReadableText(true));
            Assert.AreEqual(3, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(patches.ToHtml());
            Assert.AreEqual(3, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlDocOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(patches.ToHtmlDocument());
            Assert.AreEqual(3, patches.Count);
        }

        [Test]
        public void CorrectDiffRawTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(diffs.ToText());
        }

        [Test]
        public void CorrectDiffTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(diffs.ToReadableText());
        }

        [Test]
        public void CorrectDiffHtmlEncodedTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(diffs.ToReadableText(true));
        }

        [Test]
        public void CorrectDiffHtmlOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(diffs.ToHtml());
        }

        [Test]
        public void CorrectDiffHtmlDocOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(diffs.ToHtmlDocument());
        }
    }
}
