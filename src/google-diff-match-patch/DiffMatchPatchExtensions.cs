using System.Collections.Generic;
using System.Text;
using System.Web;

namespace DiffMatchPatch
{
    public static class DiffMatchPatchExtensions
    {
        #region Settings

        public static void dmp_perfectionSettings(this diff_match_patch dmp)
        {
            dmp.diff_Settings(30.0f, 6);
            dmp.match_Settings(0f, 500);
            dmp.patch_Settings(0f, 6);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dmp"></param>
        /// <param name="timeout">Number of seconds to map a diff before giving up (0 for infinity).</param>
        /// <param name="editCost">Cost of an empty edit operation in terms of edit characters.</param>
        public static void diff_Settings(this diff_match_patch dmp, float timeout = 1.0f, short editCost = 4)
        {
            dmp.Diff_Timeout = timeout;
            dmp.Diff_EditCost = editCost;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dmp"></param>
        /// <param name="threshold">At what point is no match declared (0.0 = perfection, 1.0 = very loose).</param>
        /// <param name="distance">How far to search for a match (0 = exact location, 1000+ = broad match).
        /// A match this many characters away from the expected location will add
        /// 1.0 to the score (0.0 is a perfect match).</param>
        public static void match_Settings(this diff_match_patch dmp, float threshold = 0.5f, int distance = 1000)
        {
            dmp.Match_Threshold = threshold;
            dmp.Match_Distance = distance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dmp"></param>
        /// <param name="threshold">When deleting a large block of text (over ~64 characters), how close
        /// do the contents have to be to match the expected contents. (0.0 =
        /// perfection, 1.0 = very loose).  Note that Match_Threshold controls
        /// how closely the end points of a delete need to match.</param>
        /// <param name="margin">Chunk size for context length.</param>
        public static void patch_Settings(this diff_match_patch dmp, float threshold = 0.5f, short margin = 4)
        {
            dmp.Patch_DeleteThreshold = threshold;
            dmp.Patch_Margin = margin;
        }

        #endregion Settings

        #region Clean Up

        public static void diff_cleanupForPrettyOutput(this diff_match_patch dmp, List<Diff> diffs)
        {
            dmp.diff_cleanupSemantic(diffs);
            dmp.diff_cleanupEfficiency(diffs);
        }

        #endregion Clean Up

        #region Diff Printing

        public static string diff_toPrettyHtmlDoc(this diff_match_patch dmp, List<Diff> diffs, bool htmlEncodeContent)
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine("<html>");
            textBuilder.AppendLine("<head>");
            textBuilder.AppendLine("</head>");
            textBuilder.AppendLine("<body style=\"font-family: 'Lucida Console', Courier, monospace;\">");
            textBuilder.AppendLine();
            textBuilder.AppendLine("<!-- START OF DIFFS -->");
            textBuilder.AppendLine(dmp.diff_toPrettyHtml(diffs, htmlEncodeContent));
            textBuilder.AppendLine("<!-- END OF DIFFS -->");
            textBuilder.AppendLine();
            textBuilder.AppendLine("</body>");
            textBuilder.AppendLine("</html>");
            return textBuilder.ToStringWithoutTrailingLine();
        }

        public static string diff_toPrettyHtml(this diff_match_patch dmp, List<Diff> diffs, bool htmlEncodeContent)
        {
            var textBuilder = new StringBuilder();

            foreach (var aDiff in diffs)
            {
                var text = htmlEncodeContent ? HttpUtility.HtmlEncode(aDiff.text) : aDiff.text;
                text = text.Replace("\n", "<br />\n");

                switch (aDiff.operation)
                {
                    case Operation.INSERT:
                        textBuilder.AppendLine($"<ins style=\"background:#e6ffe6;\">\n{text}\n</ins>");
                        break;

                    case Operation.DELETE:
                        textBuilder.AppendLine($"<del style=\"background:#ffe6e6;\">\n{text}\n</del>");
                        break;

                    case Operation.EQUAL:
                        textBuilder.AppendLine($"<span>\n{text}\n</span>");
                        break;
                }
            }

            return textBuilder.ToStringWithoutTrailingLine();
        }

        public static string diff_toPrettyText(this diff_match_patch dmp, List<Diff> diffs, bool htmlEncodeContent)
        {
            var shownDif = false;
            var expectedMsg = new StringBuilder();
            var actualMsg = new StringBuilder();

            foreach (var aDiff in diffs)
            {
                var text = htmlEncodeContent ? HttpUtility.HtmlEncode(aDiff.text) : aDiff.text;

                switch (aDiff.operation)
                {
                    case Operation.EQUAL:
                        expectedMsg.Append(text);
                        actualMsg.Append(text);

                        break;

                    case Operation.DELETE:
                        expectedMsg.Append($"{text}");
                        shownDif = true;
                        break;

                    case Operation.INSERT:
                        actualMsg.Append($"{text}");
                        shownDif = true;
                        break;
                }
            }

            var textBuilder = new StringBuilder();
            textBuilder.AppendLine(">> ---");
            textBuilder.AppendLine(expectedMsg.ToString());
            textBuilder.AppendLine("".PadLeft(20, '~'));
            textBuilder.AppendLine(">> +++");
            textBuilder.AppendLine(actualMsg.ToString());
            return textBuilder.ToStringWithoutTrailingLine();
        }

        #endregion Diff Printing

        #region Patch Printing From Diffs

        public static (int numberOfPatches, string patches) diff_toPrettyPatchText(this diff_match_patch dmp, List<Diff> diffs, bool htmlEncodeContent)
        {
            var patches = dmp.patch_make(diffs);
            var prettyText = dmp.patch_toPrettyText(patches, htmlEncodeContent);
            return (patches.Count, prettyText);
        }

        public static (int numberOfPatches, string patches) diff_toPrettyPatchHtmlDoc(this diff_match_patch dmp, List<Diff> diffs, bool htmlEncodeContent)
        {
            var patches = dmp.patch_make(diffs);
            var prettyText = dmp.patch_toPrettyHtmlDoc(patches, htmlEncodeContent);
            return (patches.Count, prettyText);
        }

        public static (int numberOfPatches, string patchesText, string patchesHtml) diff_toPrettyPatchHtmlDocAndText(this diff_match_patch dmp, List<Diff> diffs, bool htmlEncodeContent)
        {
            var patches = dmp.patch_make(diffs);
            var prettyText = dmp.patch_toPrettyText(patches, htmlEncodeContent);
            var prettyHtml = dmp.patch_toPrettyHtmlDoc(patches, htmlEncodeContent);
            return (patches.Count, prettyText, prettyHtml);
        }

        #endregion Patch Printing From Diffs

        #region Patch Printing

        public static string patch_toPrettyHtmlDoc(this diff_match_patch dmp, List<Patch> patches, bool htmlEncodeContent)
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine("<html>");
            textBuilder.AppendLine("<head>");
            textBuilder.AppendLine("</head>");
            textBuilder.AppendLine("<body style=\"font-family: 'Lucida Console', Courier, monospace;\">");
            textBuilder.AppendLine();
            textBuilder.AppendLine("<!-- START OF PATCHES -->");
            textBuilder.AppendLine(dmp.patch_toPrettyHtml(patches, htmlEncodeContent));
            textBuilder.AppendLine("<!-- END OF PATCHES -->");
            textBuilder.AppendLine();
            textBuilder.AppendLine("</body>");
            textBuilder.AppendLine("</html>");
            return textBuilder.ToStringWithoutTrailingLine();
        }

        public static string patch_toPrettyHtml(this diff_match_patch dmp, List<Patch> patches, bool htmlEncodeContent)
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine();

            var counter = 0;
            foreach (var patch in patches)
            {
                counter++;
                textBuilder.AppendLine("<div style=\"border: 1px solid black; padding-bottom: 2px;\">");
                textBuilder.AppendLine($"<div>Patch Number: {counter}<div>");
                textBuilder.AppendLine($"<div>Removal Position: {patch.start1 + 1},{patch.start1}<div>");
                textBuilder.AppendLine($"<div>Addition Position: {patch.start2 + 1},{patch.start2}<div>");
                textBuilder.AppendLine("<div>");
                textBuilder.AppendLine(dmp.diff_toPrettyHtml(patch.diffs, htmlEncodeContent));
                textBuilder.AppendLine("</div>");
                textBuilder.AppendLine("</div>");
            }

            return textBuilder.ToStringWithoutTrailingLine();
        }

        public static string patch_toPrettyText(this diff_match_patch dmp, List<Patch> patches, bool htmlEncodeContent)
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine();

            var counter = 0;
            foreach (var patch in patches)
            {
                counter++;
                textBuilder.AppendLine($">> {counter} ".PadRight(40, '_'));
                textBuilder.AppendLine($"@@ -{patch.start1 + 1},{patch.start1} +{patch.start2 + 1},{patch.start2} @@");
                textBuilder.AppendLine(dmp.diff_toPrettyText(patch.diffs, htmlEncodeContent));
                textBuilder.AppendLine($" {counter} <<".PadLeft(40, '_'));
                textBuilder.AppendLine();
            }

            return textBuilder.ToStringWithoutTrailingLine();
        }

        #endregion Patch Printing
    }
}
