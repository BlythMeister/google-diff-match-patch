using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DiffMatchPatch
{
    public static class DiffList
    {
        /// <summary>
        /// Compute and return the source text (all equalities and deletions).
        /// </summary>
        /// <param name="diffs"></param>
        /// <returns></returns>
        public static string Text1(this IEnumerable<Diff> diffs)
            => diffs
            .Where(d => d.Operation != Operation.Insert)
            .Aggregate(new StringBuilder(), (sb, diff) => sb.Append(diff.Text))
            .ToString();

        /// <summary>
        /// Compute and return the destination text (all equalities and insertions).
        /// </summary>
        /// <param name="diffs"></param>
        /// <returns></returns>
        public static string Text2(this IEnumerable<Diff> diffs)
            => diffs
            .Where(d => d.Operation != Operation.Delete)
            .Aggregate(new StringBuilder(), (sb, diff) => sb.Append(diff.Text))
            .ToString();

        /// <summary>
        /// Compute the Levenshtein distance; the number of inserted, deleted or substituted characters.
        /// </summary>
        /// <param name="diffs"></param>
        /// <returns></returns>
        public static int Levenshtein(this IEnumerable<Diff> diffs)
        {
            var levenshtein = 0;
            var insertions = 0;
            var deletions = 0;
            foreach (var aDiff in diffs)
            {
                switch (aDiff.Operation)
                {
                    case Operation.Insert:
                        insertions += aDiff.Text.Length;
                        break;

                    case Operation.Delete:
                        deletions += aDiff.Text.Length;
                        break;

                    case Operation.Equal:
                        // A deletion and an insertion is one substitution.
                        levenshtein += Math.Max(insertions, deletions);
                        insertions = 0;
                        deletions = 0;
                        break;
                }
            }
            levenshtein += Math.Max(insertions, deletions);
            return levenshtein;
        }

        /// <summary>
        /// Take a list of Diff and return the textual representation.
        /// </summary>
        /// <param name="diffs"></param>
        /// <returns></returns>
        public static string ToText(this List<Diff> diffs) => diffs.Aggregate(new StringBuilder(), (sb, diff) => sb.Append(diff)).ToString();

        /// <summary>
        /// Take a list of Diff and return a readable textual representation.
        /// </summary>
        /// <param name="diffs"></param>
        /// <param name="htmlEncodeContent"></param>
        /// <returns></returns>
        public static string ToReadableText(this List<Diff> diffs, bool htmlEncodeContent = false)
        {
            var deleteMessage = new StringBuilder();
            var insertMessage = new StringBuilder();

            foreach (var aDiff in diffs)
            {
                var text = htmlEncodeContent ? HttpUtility.HtmlEncode(aDiff.FormattedText) : aDiff.FormattedText;

                switch (aDiff.Operation)
                {
                    case Operation.Equal:
                        deleteMessage.Append(text);
                        insertMessage.Append(text);
                        break;

                    case Operation.Delete:
                        deleteMessage.Append($"{text}");
                        break;

                    case Operation.Insert:
                        insertMessage.Append($"{text}");
                        break;

                    default: throw new ArgumentException($"Invalid Operation: {aDiff.Operation}");
                }
            }

            var textBuilder = new StringBuilder();
            textBuilder.AppendLine(">> ---");
            textBuilder.AppendLine(deleteMessage.ToString());
            textBuilder.AppendLine("".PadLeft(20, '~'));
            textBuilder.AppendLine(">> +++");
            textBuilder.AppendLine(insertMessage.ToString());
            return textBuilder.ToStringWithoutTrailingLine();
        }

        /// <summary>
        /// Convert a Diff list into a pretty HTML snippet.
        /// </summary>
        /// <param name="diffs"></param>
        /// <param name="includeStyle"></param>
        /// <returns></returns>
        public static string ToHtml(this IEnumerable<Diff> diffs, bool includeStyle = true)
        {
            var textBuilder = new StringBuilder();
            if (includeStyle)
            {
                textBuilder.AddDiffStyle();
            }
            textBuilder.AppendLine("<!-- START OF DIFFS -->");
            textBuilder.AppendLine("<div class=\"diffContainer\">");

            var diffText = new StringBuilder();
            foreach (var aDiff in diffs)
            {
                var text = HttpUtility.HtmlEncode(aDiff.FormattedText).Replace("\n", "<br />\n");

                if (text.EndsWith("\n"))
                {
                    text = text.Substring(0, text.Length - 1);
                }

                switch (aDiff.Operation)
                {
                    case Operation.Insert:
                        diffText.Append($"<ins class=\"diffInsert\">{text}</ins>");
                        break;

                    case Operation.Delete:
                        diffText.Append($"<del class=\"diffDelete\">{text}</del>");
                        break;

                    case Operation.Equal:
                        diffText.Append($"<span class=\"diffUnchanged\">{text}</span>");
                        break;

                    default: throw new ArgumentException($"Invalid Operation: {aDiff.Operation}");
                }
            }

            textBuilder.AppendLine(diffText.ToStringWithoutTrailingLine());
            textBuilder.AppendLine("</div>");
            textBuilder.AppendLine("<!-- END OF DIFFS -->");
            return textBuilder.ToStringWithoutTrailingLine();
        }

        /// <summary>
        /// Convert a Diff list into a pretty HTML report.
        /// </summary>
        /// <param name="diffs"></param>
        /// <returns></returns>
        public static string ToHtmlDocument(this IEnumerable<Diff> diffs)
        {
            var textBuilder = new StringBuilder();
            textBuilder.AppendLine("<html>");
            textBuilder.AppendLine("<head>");
            textBuilder.AddDiffStyle();
            textBuilder.AppendLine("</head>");
            textBuilder.AppendLine("<body>");
            textBuilder.AppendLine();
            textBuilder.AppendLine(diffs.ToHtml(false));
            textBuilder.AppendLine();
            textBuilder.AppendLine("</body>");
            textBuilder.AppendLine("</html>");
            return textBuilder.ToStringWithoutTrailingLine();
        }

        internal static void AddDiffStyle(this StringBuilder textBuilder)
        {
            textBuilder.AppendLine("<!-- START OF DIFF STYLE -->");
            textBuilder.AppendLine("<style type=\"text/css\">");
            textBuilder.AppendLine(".diffContainer {");
            textBuilder.AppendLine("	font-family: 'Courier New', Courier, monospace;");
            textBuilder.AppendLine("}");
            textBuilder.AppendLine();
            textBuilder.AppendLine(".diffInsert {");
            textBuilder.AppendLine("	background:#e6ffe6;");
            textBuilder.AppendLine("}");
            textBuilder.AppendLine();
            textBuilder.AppendLine(".diffDelete {");
            textBuilder.AppendLine("	background:#ffe6e6;");
            textBuilder.AppendLine("}");
            textBuilder.AppendLine("</style>");
            textBuilder.AppendLine("<!-- END OF DIFF STYLE -->");
        }

        private static char ToDelta(this Operation o)
        {
            switch (o)
            {
                case Operation.Delete: return '-';
                case Operation.Insert: return '+';
                case Operation.Equal: return '=';
                default: throw new ArgumentException($"Unknown Operation: {o}");
            }
        }

        private static Operation FromDelta(char c)
        {
            switch (c)
            {
                case '-': return Operation.Delete;
                case '+': return Operation.Insert;
                case '=': return Operation.Equal;
                default: throw new ArgumentException($"Invalid Delta Token: {c}");
            }
        }

        /// <summary>
        /// Crush the diff into an encoded string which describes the operations
        /// required to transform text1 into text2.
        /// E.g. =3\t-2\t+ing  -> Keep 3 chars, delete 2 chars, insert 'ing'.
        /// Operations are tab-separated.  Inserted text is escaped using %xx
        /// notation.
        /// </summary>
        /// <param name="diffs"></param>
        /// <returns></returns>
        public static string ToDelta(this IEnumerable<Diff> diffs)
        {
            var s =
                from aDiff in diffs
                let sign = aDiff.Operation.ToDelta()
                let textToAppend = aDiff.Operation == Operation.Insert
                    ? aDiff.Text.UrlEncoded()
                    : aDiff.Text.Length.ToString()
                select string.Concat(sign, textToAppend);

            var delta = string.Join("\t", s);
            return delta;
        }

        /// <summary>
        /// Given the original text1, and an encoded string which describes the
        /// operations required to transform text1 into text2, compute the full diff.
        /// </summary>
        /// <param name="text1">Source string for the diff.</param>
        /// <param name="delta">Delta text.</param>
        /// <returns></returns>
        public static IEnumerable<Diff> FromDelta(string text1, string delta)
        {
            var pointer = 0;  // Cursor in text1

            var tokens = delta.Split(new[] { "\t" }, StringSplitOptions.None);

            foreach (var token in tokens)
            {
                if (token.Length == 0)
                {
                    // Blank tokens are ok (from a trailing \t).
                    continue;
                }
                // Each token begins with a one character parameter which specifies the
                // operation of this token (delete, insert, equality).
                var param = token.Substring(1);
                var operation = FromDelta(token[0]);
                string text;
                switch (operation)
                {
                    case Operation.Insert:
                        // decode would change all "+" to " "
                        text = param.Replace("+", "%2b").UrlDecoded();
                        break;

                    case Operation.Delete:
                    case Operation.Equal:
                        if (!int.TryParse(param, out var n))
                        {
                            throw new ArgumentException($"Invalid number in Diff.FromDelta: {param}");
                        }
                        if (pointer < 0 || n < 0 || pointer > text1.Length - n)
                        {
                            throw new ArgumentException($"Delta length ({pointer}) larger than source text length ({text1.Length}).");
                        }

                        text = text1.Substring(pointer, n);
                        pointer += n;

                        break;

                    default: throw new ArgumentException($"Unknown Operation: {operation}");
                }
                yield return Diff.Create(operation, text);
            }
            if (pointer != text1.Length)
            {
                throw new ArgumentException($"Delta length ({pointer}) smaller than source text length ({text1.Length}).");
            }
        }

        /// <summary>
        /// Reorder and merge like edit sections.  Merge equalities.
        /// Any edit section can move as long as it doesn't cross an equality.
        /// </summary>
        /// <param name="diffs">list of Diffs</param>
        internal static void CleanupMerge(this List<Diff> diffs)
        {
            // Add a dummy entry at the end.
            diffs.Add(Diff.Equal(string.Empty));
            var nofdiffs = 0;
            var sbDelete = new StringBuilder();
            var sbInsert = new StringBuilder();
            var pointer = 0;
            while (pointer < diffs.Count)
            {
                switch (diffs[pointer].Operation)
                {
                    case Operation.Insert:
                        nofdiffs++;
                        sbInsert.Append(diffs[pointer].Text);
                        pointer++;
                        break;

                    case Operation.Delete:
                        nofdiffs++;
                        sbDelete.Append(diffs[pointer].Text);
                        pointer++;
                        break;

                    case Operation.Equal:
                        // Upon reaching an equality, check for prior redundancies.
                        if (nofdiffs > 1)
                        {
                            if (sbDelete.Length > 0 && sbInsert.Length > 0)
                            {
                                // Factor out any common prefixies.
                                var commonlength = TextUtil.CommonPrefix(sbInsert, sbDelete);
                                if (commonlength != 0)
                                {
                                    var commonprefix = sbInsert.ToString(0, commonlength);
                                    sbInsert.Remove(0, commonlength);
                                    sbDelete.Remove(0, commonlength);
                                    var index = pointer - nofdiffs - 1;
                                    if (index >= 0 && diffs[index].Operation == Operation.Equal)
                                    {
                                        diffs[index] = diffs[index].Replace(diffs[index].Text + commonprefix);
                                    }
                                    else
                                    {
                                        diffs.Insert(0, Diff.Equal(commonprefix));
                                        pointer++;
                                    }
                                }
                                // Factor out any common suffixies.
                                commonlength = TextUtil.CommonSuffix(sbInsert, sbDelete);
                                if (commonlength != 0)
                                {
                                    var commonsuffix = sbInsert.ToString(sbInsert.Length - commonlength, commonlength);
                                    sbInsert.Remove(sbInsert.Length - commonlength, commonlength);
                                    sbDelete.Remove(sbDelete.Length - commonlength, commonlength);
                                    diffs[pointer] = diffs[pointer].Replace(commonsuffix + diffs[pointer].Text);
                                }
                            }

                            // Delete the offending records and add the merged ones.
                            IEnumerable<Diff> Replacements()
                            {
                                if (sbDelete.Length > 0) yield return Diff.Delete(sbDelete.ToString());
                                if (sbInsert.Length > 0) yield return Diff.Insert(sbInsert.ToString());
                            }

                            var replacements = Replacements().ToList();
                            diffs.Splice(pointer - nofdiffs, nofdiffs, replacements);

                            pointer = pointer - nofdiffs + replacements.Count + 1;
                        }
                        else if (pointer > 0 && diffs[pointer - 1].Operation == Operation.Equal)
                        {
                            // Merge this equality with the previous one.
                            diffs[pointer - 1] = diffs[pointer - 1].Replace(diffs[pointer - 1].Text + diffs[pointer].Text);
                            diffs.RemoveAt(pointer);
                        }
                        else
                        {
                            pointer++;
                        }
                        nofdiffs = 0;
                        sbDelete.Clear();
                        sbInsert.Clear();
                        break;
                }
            }
            if (diffs.Last().Text.Length == 0)
            {
                diffs.RemoveAt(diffs.Count - 1);  // Remove the dummy entry at the end.
            }

            // Second pass: look for single edits surrounded on both sides by
            // equalities which can be shifted sideways to eliminate an equality.
            // e.g: A<ins>BA</ins>C -> <ins>AB</ins>AC
            var changes = false;
            // Intentionally ignore the first and last element (don't need checking).
            for (var i = 1; i < diffs.Count - 1; i++)
            {
                var previous = diffs[i - 1];
                var current = diffs[i];
                var next = diffs[i + 1];
                if (previous.Operation == Operation.Equal && next.Operation == Operation.Equal)
                {
                    // This is a single edit surrounded by equalities.
                    if (current.Text.EndsWith(previous.Text, StringComparison.Ordinal))
                    {
                        // Shift the edit over the previous equality.
                        var text = previous.Text + current.Text.Substring(0, current.Text.Length - previous.Text.Length);
                        diffs[i] = current.Replace(text);
                        diffs[i + 1] = next.Replace(previous.Text + next.Text);
                        diffs.Splice(i - 1, 1);
                        changes = true;
                    }
                    else if (current.Text.StartsWith(next.Text, StringComparison.Ordinal))
                    {
                        // Shift the edit over the next equality.
                        diffs[i - 1] = previous.Replace(previous.Text + next.Text);
                        diffs[i] = current.Replace(current.Text.Substring(next.Text.Length) + next.Text);
                        diffs.Splice(i + 1, 1);
                        changes = true;
                    }
                }
            }
            // If shifts were made, the diff needs reordering and another shift sweep.
            if (changes)
            {
                diffs.CleanupMerge();
            }
        }

        /// <summary>
        /// Look for single edits surrounded on both sides by equalities
        /// which can be shifted sideways to align the edit to a word boundary.
        /// e.g: The c<ins>at c</ins>ame. -> The <ins>cat </ins>came.
        /// </summary>
        /// <param name="diffs"></param>
        internal static void CleanupSemanticLossless(this List<Diff> diffs)
        {
            var pointer = 1;
            // Intentionally ignore the first and last element (don't need checking).
            while (pointer < diffs.Count - 1)
            {
                var previous = diffs[pointer - 1];
                var current = diffs[pointer];
                var next = diffs[pointer + 1];

                if (previous.Operation == Operation.Equal && next.Operation == Operation.Equal)
                {
                    // This is a single edit surrounded by equalities.
                    var equality1 = previous.Text;
                    var edit = current.Text;
                    var equality2 = next.Text;

                    // First, shift the edit as far left as possible.
                    var commonOffset = TextUtil.CommonSuffix(equality1, edit);
                    if (commonOffset > 0)
                    {
                        var commonString = edit.Substring(edit.Length - commonOffset);
                        equality1 = equality1.Substring(0, equality1.Length - commonOffset);
                        edit = commonString + edit.Substring(0, edit.Length - commonOffset);
                        equality2 = commonString + equality2;
                    }

                    // Second, step character by character right,
                    // looking for the best fit.
                    var bestEquality1 = equality1;
                    var bestEdit = edit;
                    var bestEquality2 = equality2;
                    var bestScore = DiffCleanupSemanticScore(equality1, edit) + DiffCleanupSemanticScore(edit, equality2);
                    while (edit.Length != 0 && equality2.Length != 0 && edit[0] == equality2[0])
                    {
                        equality1 += edit[0];
                        edit = edit.Substring(1) + equality2[0];
                        equality2 = equality2.Substring(1);
                        var score = DiffCleanupSemanticScore(equality1, edit) + DiffCleanupSemanticScore(edit, equality2);
                        // The >= encourages trailing rather than leading whitespace on
                        // edits.
                        if (score >= bestScore)
                        {
                            bestScore = score;
                            bestEquality1 = equality1;
                            bestEdit = edit;
                            bestEquality2 = equality2;
                        }
                    }

                    if (previous.Text != bestEquality1)
                    {
                        // We have an improvement, save it back to the diff.

                        var newDiffs = new[]
                        {
                            Diff.Equal(bestEquality1),
                            current.Replace(bestEdit),
                            Diff.Equal(bestEquality2)
                        }.Where(d => !string.IsNullOrEmpty(d.Text))
                            .ToArray();

                        diffs.Splice(pointer - 1, 3, newDiffs);
                        pointer = pointer - (3 - newDiffs.Length);
                    }
                }
                pointer++;
            }
        }

        /// <summary>
        /// Given two strings, compute a score representing whether the internal
        /// boundary falls on logical boundaries.
        /// Scores range from 6 (best) to 0 (worst).
        ///  </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns>score</returns>
        private static int DiffCleanupSemanticScore(string one, string two)
        {
            if (one.Length == 0 || two.Length == 0)
            {
                // Edges are the best.
                return 6;
            }

            // Each port of this function behaves slightly differently due to
            // subtle differences in each language's definition of things like
            // 'whitespace'.  Since this function's purpose is largely cosmetic,
            // the choice has been made to use each language's native features
            // rather than force total conformity.
            var char1 = one[one.Length - 1];
            var char2 = two[0];
            var nonAlphaNumeric1 = !char.IsLetterOrDigit(char1);
            var nonAlphaNumeric2 = !char.IsLetterOrDigit(char2);
            var whitespace1 = nonAlphaNumeric1 && char.IsWhiteSpace(char1);
            var whitespace2 = nonAlphaNumeric2 && char.IsWhiteSpace(char2);
            var lineBreak1 = whitespace1 && char.IsControl(char1);
            var lineBreak2 = whitespace2 && char.IsControl(char2);
            var blankLine1 = lineBreak1 && BlankLineEnd.IsMatch(one);
            var blankLine2 = lineBreak2 && BlankLineStart.IsMatch(two);

            if (blankLine1 || blankLine2)
            {
                // Five points for blank lines.
                return 5;
            }
            if (lineBreak1 || lineBreak2)
            {
                // Four points for line breaks.
                return 4;
            }
            if (nonAlphaNumeric1 && !whitespace1 && whitespace2)
            {
                // Three points for end of sentences.
                return 3;
            }
            if (whitespace1 || whitespace2)
            {
                // Two points for whitespace.
                return 2;
            }
            if (nonAlphaNumeric1 || nonAlphaNumeric2)
            {
                // One point for non-alphanumeric.
                return 1;
            }
            return 0;
        }

        // Define some regex patterns for matching boundaries.
        private static readonly Regex BlankLineEnd = new Regex("\\n\\r?\\n\\Z", RegexOptions.Compiled);

        private static readonly Regex BlankLineStart = new Regex("\\A\\r?\\n\\r?\\n", RegexOptions.Compiled);

        /// <summary>
        /// Reduce the number of edits by eliminating operationally & semantically trivial equalities.
        /// </summary>
        /// <param name="diffs"></param>
        /// <param name="diffEditCost"></param>
        public static void Cleanup(this List<Diff> diffs, short diffEditCost = 4)
        {
            diffs.CleanupEfficiency(diffEditCost);
            diffs.CleanupSemantic();
        }

        /// <summary>
        /// Reduce the number of edits by eliminating operationally trivial equalities.
        /// </summary>
        /// <param name="diffs"></param>
        /// <param name="diffEditCost"></param>
        public static void CleanupEfficiency(this List<Diff> diffs, short diffEditCost = 4)
        {
            var changes = false;
            // Stack of indices where equalities are found.
            var equalities = new Stack<int>();
            // Always equal to equalities[equalitiesLength-1][1]
            var lastEquality = string.Empty;
            // Is there an insertion operation before the last equality.
            var preIns = false;
            // Is there a deletion operation before the last equality.
            var preDel = false;
            // Is there an insertion operation after the last equality.
            var postIns = false;
            // Is there a deletion operation after the last equality.
            var postDel = false;

            for (var i = 0; i < diffs.Count; i++)
            {
                if (diffs[i].Operation == Operation.Equal)
                {  // Equality found.
                    if (diffs[i].Text.Length < diffEditCost && (postIns || postDel))
                    {
                        // Candidate found.
                        equalities.Push(i);
                        preIns = postIns;
                        preDel = postDel;
                        lastEquality = diffs[i].Text;
                    }
                    else
                    {
                        // Not a candidate, and can never become one.
                        equalities.Clear();
                        lastEquality = string.Empty;
                    }
                    postIns = postDel = false;
                }
                else
                {  // An insertion or deletion.
                    if (diffs[i].Operation == Operation.Delete)
                    {
                        postDel = true;
                    }
                    else
                    {
                        postIns = true;
                    }
                    /*
                     * Five types to be split:
                     * <ins>A</ins><del>B</del>XY<ins>C</ins><del>D</del>
                     * <ins>A</ins>X<ins>C</ins><del>D</del>
                     * <ins>A</ins><del>B</del>X<ins>C</ins>
                     * <ins>A</del>X<ins>C</ins><del>D</del>
                     * <ins>A</ins><del>B</del>X<del>C</del>
                     */
                    if ((lastEquality.Length != 0)
                        && ((preIns && preDel && postIns && postDel)
                            || ((lastEquality.Length < diffEditCost / 2)
                                && (preIns ? 1 : 0) + (preDel ? 1 : 0) + (postIns ? 1 : 0)
                                + (postDel ? 1 : 0) == 3)))
                    {
                        diffs.Splice(equalities.Peek(), 1, Diff.Delete(lastEquality), Diff.Insert(lastEquality));
                        equalities.Pop();  // Throw away the equality we just deleted.
                        lastEquality = string.Empty;
                        if (preIns && preDel)
                        {
                            // No changes made which could affect previous entry, keep going.
                            postIns = postDel = true;
                            equalities.Clear();
                        }
                        else
                        {
                            if (equalities.Count > 0)
                            {
                                equalities.Pop();
                            }

                            i = equalities.Count > 0 ? equalities.Peek() : -1;
                            postIns = postDel = false;
                        }
                        changes = true;
                    }
                }
            }

            if (changes)
            {
                diffs.CleanupMerge();
            }
        }

        /// <summary>
        /// Reduce the number of edits by eliminating semantically trivial equalities.
        /// </summary>
        /// <param name="diffs"></param>
        public static void CleanupSemantic(this List<Diff> diffs)
        {
            // Stack of indices where equalities are found.
            var equalities = new Stack<int>();
            // Always equal to equalities[equalitiesLength-1][1]
            string lastEquality = null;
            var pointer = 0;  // Index of current position.
            // Number of characters that changed prior to the equality.
            var lengthInsertions1 = 0;
            var lengthDeletions1 = 0;
            // Number of characters that changed after the equality.
            var lengthInsertions2 = 0;
            var lengthDeletions2 = 0;
            while (pointer < diffs.Count)
            {
                if (diffs[pointer].Operation == Operation.Equal)
                {  // Equality found.
                    equalities.Push(pointer);
                    lengthInsertions1 = lengthInsertions2;
                    lengthDeletions1 = lengthDeletions2;
                    lengthInsertions2 = 0;
                    lengthDeletions2 = 0;
                    lastEquality = diffs[pointer].Text;
                }
                else
                {  // an insertion or deletion
                    if (diffs[pointer].Operation == Operation.Insert)
                    {
                        lengthInsertions2 += diffs[pointer].Text.Length;
                    }
                    else
                    {
                        lengthDeletions2 += diffs[pointer].Text.Length;
                    }
                    // Eliminate an equality that is smaller or equal to the edits on both
                    // sides of it.
                    if (lastEquality != null && (lastEquality.Length
                                                 <= Math.Max(lengthInsertions1, lengthDeletions1))
                        && (lastEquality.Length
                            <= Math.Max(lengthInsertions2, lengthDeletions2)))
                    {
                        // Duplicate record.

                        diffs.Splice(equalities.Peek(), 1, Diff.Delete(lastEquality), Diff.Insert(lastEquality));

                        // Throw away the equality we just deleted.
                        equalities.Pop();
                        if (equalities.Count > 0)
                        {
                            equalities.Pop();
                        }
                        pointer = equalities.Count > 0 ? equalities.Peek() : -1;
                        lengthInsertions1 = 0;  // Reset the counters.
                        lengthDeletions1 = 0;
                        lengthInsertions2 = 0;
                        lengthDeletions2 = 0;
                        lastEquality = null;
                    }
                }
                pointer++;
            }

            diffs.CleanupMerge();
            diffs.CleanupSemanticLossless();

            // Find any overlaps between deletions and insertions.
            // e.g: <del>abcxxx</del><ins>xxxdef</ins>
            //   -> <del>abc</del>xxx<ins>def</ins>
            // e.g: <del>xxxabc</del><ins>defxxx</ins>
            //   -> <ins>def</ins>xxx<del>abc</del>
            // Only extract an overlap if it is as big as the edit ahead or behind it.
            pointer = 1;
            while (pointer < diffs.Count)
            {
                if (diffs[pointer - 1].Operation == Operation.Delete &&
                    diffs[pointer].Operation == Operation.Insert)
                {
                    var deletion = diffs[pointer - 1].Text;
                    var insertion = diffs[pointer].Text;
                    var overlapLength1 = TextUtil.CommonOverlap(deletion, insertion);
                    var overlapLength2 = TextUtil.CommonOverlap(insertion, deletion);
                    if (overlapLength1 >= overlapLength2)
                    {
                        if (overlapLength1 >= deletion.Length / 2.0 ||
                            overlapLength1 >= insertion.Length / 2.0)
                        {
                            // Overlap found.
                            // Insert an equality and trim the surrounding edits.
                            var newDiffs = new[]
                            {
                                Diff.Delete(deletion.Substring(0, deletion.Length - overlapLength1)),
                                Diff.Equal(insertion.Substring(0, overlapLength1)),
                                Diff.Insert(insertion.Substring(overlapLength1))
                            };

                            diffs.Splice(pointer - 1, 2, newDiffs);
                            pointer++;
                        }
                    }
                    else
                    {
                        if (overlapLength2 >= deletion.Length / 2.0 ||
                            overlapLength2 >= insertion.Length / 2.0)
                        {
                            // Reverse overlap found.
                            // Insert an equality and swap and trim the surrounding edits.

                            diffs.Splice(pointer - 1, 2,
                                Diff.Insert(insertion.Substring(0, insertion.Length - overlapLength2)),
                                Diff.Equal(deletion.Substring(0, overlapLength2)),
                                Diff.Delete(deletion.Substring(overlapLength2)
                                    ));
                            pointer++;
                        }
                    }
                    pointer++;
                }
                pointer++;
            }
        }

        /// <summary>
        /// Rehydrate the text in a diff from a string of line hashes to real lines of text.
        /// </summary>
        /// <param name="diffs"></param>
        /// <param name="lineArray">list of unique strings</param>
        /// <returns></returns>
        internal static IEnumerable<Diff> CharsToLines(this ICollection<Diff> diffs, IList<string> lineArray)
        {
            foreach (var diff in diffs)
            {
                var text = new StringBuilder();
                foreach (var c in diff.Text)
                {
                    text.Append(lineArray[c]);
                }
                yield return diff.Replace(text.ToString());
            }
        }

        /// <summary>
        /// Compute and return equivalent location in target text.
        /// </summary>
        /// <param name="diffs">list of diffs</param>
        /// <param name="location1">location in source</param>
        /// <returns>location in target</returns>
        internal static int FindEquivalentLocation2(this IEnumerable<Diff> diffs, int location1)
        {
            var chars1 = 0;
            var chars2 = 0;
            var lastChars1 = 0;
            var lastChars2 = 0;
            var lastDiff = Diff.Create(Operation.Equal, string.Empty);
            foreach (var aDiff in diffs)
            {
                if (aDiff.Operation != Operation.Insert)
                {
                    // Equality or deletion.
                    chars1 += aDiff.Text.Length;
                }
                if (aDiff.Operation != Operation.Delete)
                {
                    // Equality or insertion.
                    chars2 += aDiff.Text.Length;
                }
                if (chars1 > location1)
                {
                    // Overshot the location.
                    lastDiff = aDiff;
                    break;
                }
                lastChars1 = chars1;
                lastChars2 = chars2;
            }
            if (lastDiff.Operation == Operation.Delete)
            {
                // The location was deleted.
                return lastChars2;
            }
            // Add the remaining character length.
            return lastChars2 + (location1 - lastChars1);
        }
    }
}
