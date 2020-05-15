using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiffMatchPatch
{
    public static class DiffMatchPatchExtensions
    {
        public static string diff_prettyHtmlNoPara(this diff_match_patch dmp, List<Diff> diffs)
        {
            return dmp.diff_prettyHtml(diffs).Replace("&para;<br>", "<br />");
        }

        public static string diff_prettyText(this diff_match_patch dmp, List<Diff> diffs)
        {
            var patches = dmp.patch_make(diffs);
            var stringB = new StringBuilder();
            stringB.AppendLine();

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
                                expectedMsg.Append($"{patchDif.text}[...]");
                                actualMsg.Append($"{patchDif.text}[...]");
                            }
                            else
                            {
                                expectedMsg.Append($"[...]{patchDif.text}");
                                actualMsg.Append($"[...]{patchDif.text}");
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

                stringB.AppendLine($">> {counter} ".PadRight(40, '_'));
                stringB.AppendLine($"@@ -{patch.start1 + 1},{patch.start1} +{patch.start2 + 1},{patch.start2} @@");
                stringB.AppendLine(">> ---");
                stringB.AppendLine(expectedMsg.ToString());
                stringB.AppendLine("".PadLeft(20, '~'));
                stringB.AppendLine(">> +++");
                stringB.AppendLine(actualMsg.ToString());
                stringB.AppendLine($" {counter} <<".PadLeft(40, '_'));
                stringB.AppendLine();
            }

            return stringB.ToString();
        }
    }
}
