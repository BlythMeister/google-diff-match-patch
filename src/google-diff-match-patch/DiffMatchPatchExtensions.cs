using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DiffMatchPatch
{
    public static class DiffMatchPatchExtensions
    {
        public static void diff_cleanupForPrettyOutput(this diff_match_patch dmp, List<Diff> diffs)
        {
            dmp.diff_cleanupSemantic(diffs);
            dmp.diff_cleanupEfficiency(diffs);
        }

        public static string diff_toPrettyHtmlDoc(this diff_match_patch dmp, List<Diff> diffs)
        {
            var html = new StringBuilder();
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("</head>");
            html.AppendLine("<body style=\"font-family: 'Lucida Console', Courier, monospace;\">");
            html.AppendLine();
            html.AppendLine("<!-- START OF DIFFS -->");
            html.AppendLine();
            html.AppendLine(dmp.diff_toPrettyHtml(diffs));
            html.AppendLine();
            html.AppendLine("<!-- END OF DIFFS -->");
            html.AppendLine();
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            return html.ToString();
        }

        public static string diff_toPrettyHtml(this diff_match_patch dmp, List<Diff> diffs)
        {
            var html = new StringBuilder();
            foreach (var aDiff in diffs)
            {
                var text = HttpUtility.HtmlEncode(aDiff.text).Replace("\n", "<br />\n");

                switch (aDiff.operation)
                {
                    case Operation.INSERT:
                        html.Append("<ins style=\"background:#e6ffe6;\">").Append(text).Append("</ins>");
                        break;

                    case Operation.DELETE:
                        html.Append("<del style=\"background:#ffe6e6;\">").Append(text).Append("</del>");
                        break;

                    case Operation.EQUAL:
                        html.Append("<span>").Append(text).Append("</span>");
                        break;
                }
            }
            return html.ToString();
        }

        public static (int numberOfPatches, string patches) diff_toPrettyText(this diff_match_patch dmp, List<Diff> diffs)
        {
            var patches = dmp.patch_make(diffs);
            var prettyText = dmp.patch_toPrettyText(patches);
            return (patches.Count, prettyText);
        }

        public static string patch_toPrettyText(this diff_match_patch dmp, List<Patch> patches)
        {
            var text = new StringBuilder();
            text.AppendLine();

            var counter = 0;
            foreach (var patch in patches)
            {
                counter++;
                var shownDif = false;
                var expectedMsg = new StringBuilder();
                var actualMsg = new StringBuilder();

                if (patch.diffs.First().operation != Operation.EQUAL)
                {
                    expectedMsg.Append("[>>>]");
                    actualMsg.Append("[>>>]");
                }

                foreach (var patchDif in patch.diffs)
                {
                    switch (patchDif.operation)
                    {
                        case Operation.EQUAL:
                            if (shownDif)
                            {
                                expectedMsg.Append($"{patchDif.text.TrimEnd()}[...]");
                                actualMsg.Append($"{patchDif.text.TrimEnd()}[...]");
                            }
                            else
                            {
                                expectedMsg.Append($"[...]{patchDif.text.TrimStart()}");
                                actualMsg.Append($"[...]{patchDif.text.TrimStart()}");
                            }
                            break;

                        case Operation.DELETE:
                            expectedMsg.Append($"{patchDif.text}");
                            shownDif = true;
                            break;

                        case Operation.INSERT:
                            actualMsg.Append($"{patchDif.text}");
                            shownDif = true;
                            break;
                    }
                }

                if (patch.diffs.Last().operation != Operation.EQUAL)
                {
                    expectedMsg.Append("[<<<]");
                    actualMsg.Append("[<<<]");
                }

                text.AppendLine($">> {counter} ".PadRight(40, '_'));
                text.AppendLine($"@@ -{patch.start1 + 1},{patch.start1} +{patch.start2 + 1},{patch.start2} @@");
                text.AppendLine(">> ---");
                text.AppendLine(expectedMsg.ToString());
                text.AppendLine("".PadLeft(20, '~'));
                text.AppendLine(">> +++");
                text.AppendLine(actualMsg.ToString());
                text.AppendLine($" {counter} <<".PadLeft(40, '_'));
                text.AppendLine();
            }

            return text.ToString();
        }
    }
}
