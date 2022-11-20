using System.Text;

namespace DiffMatchPatch
{
    public static class StringBuilderExtensions
    {
        public static string ToStringWithoutTrailingLine(this StringBuilder sb)
        {
            var stringOutput = sb.ToString();
            if (stringOutput.EndsWith("\r\n"))
            {
                return stringOutput.Substring(0, stringOutput.Length - 2);
            }

            if (stringOutput.EndsWith("\n"))
            {
                return stringOutput.Substring(0, stringOutput.Length - 1);
            }

            return stringOutput;
        }
        public static StringBuilder AppendNewline(this StringBuilder sb, string append) 
        { 
            return sb.Append(append).Append('\n');
        }
    }
}
