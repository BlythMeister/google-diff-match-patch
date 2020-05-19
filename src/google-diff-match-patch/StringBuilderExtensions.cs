using System.Text;

namespace DiffMatchPatch
{
    public static class StringBuilderExtensions
    {
        public static string ToStringWithoutTrailingLine(this StringBuilder sb)
        {
            var stringOutput = sb.ToString();
            return stringOutput.Substring(0, stringOutput.Length - 2);
        }
    }
}
