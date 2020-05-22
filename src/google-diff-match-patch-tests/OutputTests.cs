using ApprovalTests;
using ApprovalTests.Reporters;
using DiffMatchPatch;
using NUnit.Framework;

namespace DiffMatchPatchTests
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter))]
    public class OutputTests
    {
        private const string expected = "start\n" +
                                        "line with some content\n" +
                                        "line with some more content\n" +
                                        "line with some \twhitespace\t content\n" +
                                        "unchanged content\n" +
                                        "removed line\n" +
                                        "end";

        private const string actual = "start\n" +
                                      "new line\n" +
                                      "line with some modified content\n" +
                                      "line with some more content\tthat has a tab added\n" +
                                      "line with some whitespace content\n" +
                                      "unchanged content\n" +
                                      "end";

        private const string expectedHtml = "<p>start</p>\n" +
                                            "<p>line with some content</p>\n" +
                                            "<p>line with some more content</p>\n" +
                                            "<p>line with some \twhitespace\t content</p>\n" +
                                            "<p>unchanged content</p>\n" +
                                            "<p>removed line</p>\n" +
                                            "<p>end</p>";

        private const string actualHtml = "<p>start</p>\n" +
                                          "<p>new line</p>\n" +
                                          "<p>line with some modified content</p>\n" +
                                          "<p>line with some more content\tthat has a tab added</p>\n" +
                                          "<p>line with some whitespace content</p>\n" +
                                          "<p>unchanged content</p>\n" +
                                          "<p>end</p>";

        [Test]
        public void CorrectPatchRawTextOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(new ApprovalTextWriter(patches.ToText()));
            Assert.AreEqual(6, patches.Count);
        }

        [Test]
        public void CorrectPatchTextOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(new ApprovalTextWriter(patches.ToReadableText()));
            Assert.AreEqual(6, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(new ApprovalTextWriter(patches.ToHtml(), "html"));
            Assert.AreEqual(6, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlDocOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(new ApprovalTextWriter(patches.ToHtmlDocument(), "html"));
            Assert.AreEqual(6, patches.Count);
        }

        [Test]
        public void CorrectDiffRawTextOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            Approvals.Verify(new ApprovalTextWriter(diffs.ToText()));
        }

        [Test]
        public void CorrectDiffTextOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            Approvals.Verify(new ApprovalTextWriter(diffs.ToReadableText()));
        }

        [Test]
        public void CorrectDiffHtmlOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            Approvals.Verify(new ApprovalTextWriter(diffs.ToHtml(), "html"));
        }

        [Test]
        public void CorrectDiffHtmlDocOutput_TextInput()
        {
            var diffs = Diff.Compute(expected, actual);
            diffs.Cleanup();

            Approvals.Verify(new ApprovalTextWriter(diffs.ToHtmlDocument(), "html"));
        }

        [Test]
        public void CorrectPatchRawTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(new ApprovalTextWriter(patches.ToText()));
            Assert.AreEqual(6, patches.Count);
        }

        [Test]
        public void CorrectPatchTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(new ApprovalTextWriter(patches.ToReadableText()));
            Assert.AreEqual(6, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlEncodedTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(new ApprovalTextWriter(patches.ToReadableText(true)));
            Assert.AreEqual(6, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(new ApprovalTextWriter(patches.ToHtml(), "html"));
            Assert.AreEqual(6, patches.Count);
        }

        [Test]
        public void CorrectPatchHtmlDocOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            var patches = Patch.FromDiffs(diffs);

            Approvals.Verify(new ApprovalTextWriter(patches.ToHtmlDocument(), "html"));
            Assert.AreEqual(6, patches.Count);
        }

        [Test]
        public void CorrectDiffRawTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(new ApprovalTextWriter(diffs.ToText()));
        }

        [Test]
        public void CorrectDiffTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(new ApprovalTextWriter(diffs.ToReadableText()));
        }

        [Test]
        public void CorrectDiffHtmlEncodedTextOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(new ApprovalTextWriter(diffs.ToReadableText(true)));
        }

        [Test]
        public void CorrectDiffHtmlOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(new ApprovalTextWriter(diffs.ToHtml(), "html"));
        }

        [Test]
        public void CorrectDiffHtmlDocOutput_HtmlInput()
        {
            var diffs = Diff.Compute(expectedHtml, actualHtml);
            diffs.Cleanup();

            Approvals.Verify(new ApprovalTextWriter(diffs.ToHtmlDocument(), "html"));
        }
    }
}
