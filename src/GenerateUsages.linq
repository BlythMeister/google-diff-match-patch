<Query Kind="Program">
  <NuGetReference>google-diff-match-patch</NuGetReference>
  <Namespace>DiffMatchPatch</Namespace>
</Query>

void Main()
{
	var text1 = "start\n" +
				"line with some content\n" +
				"unchanged content\n" +
				"removed line\n" +
				"end";

	var text2 = "start\n" +
				"new line\n" +
				"line with some modified content\n" +
				"unchanged content\n" +
				"end";
	
	Console.WriteLine("## Usages");				 
	Console.WriteLine();
	Console.WriteLine("Given 2 input strings, for example:");
	Console.WriteLine();
	Console.WriteLine("```csharp");
	Console.WriteLine($"var text1 = \"{text1.Replace("\n","\\n")}\"");
	Console.WriteLine($"var text2 = \"{text2.Replace("\n","\\n")}\"");
	Console.WriteLine("```");
	Console.WriteLine();
	Console.WriteLine("We can calculate the differences by doing");
	Console.WriteLine();
	Console.WriteLine("```csharp");
	Console.WriteLine("var diffs = Diff.Compute(text1, text2);");
	Console.WriteLine("foreach (var diff in diffs)");
	Console.WriteLine("{");
	Console.WriteLine("	Console.WriteLine(diff.ToString().Trim());");
	Console.WriteLine("}");
	Console.WriteLine("```");
	Console.WriteLine();
	Console.WriteLine("This would give output of:");
	Console.WriteLine();
	Console.WriteLine("```text");
	var diffs = Diff.Compute(text1, text2);
	foreach (var diff in diffs)
	{
		Console.WriteLine(diff.ToString().Trim());
	}
	Console.WriteLine("```");
	Console.WriteLine();
	Console.WriteLine("From a diff collection we can create a set of patches:");
	Console.WriteLine();
	Console.WriteLine("```csharp");
	Console.WriteLine("var patches = Patch.FromDiffs(diffs);");
	Console.WriteLine("foreach (var patch in patches)");
	Console.WriteLine("{");
	Console.WriteLine("	Console.WriteLine(diff.ToString().Trim());");
	Console.WriteLine("}");
	Console.WriteLine("```");
	Console.WriteLine();
	Console.WriteLine("This would give output of:");
	Console.WriteLine();
	Console.WriteLine("```text");
	var patches = Patch.FromDiffs(diffs);
	foreach (var patch in patches)
	{
		Console.WriteLine(patch.ToString().Trim());
	}
	Console.WriteLine("```");
	Console.WriteLine();
	Console.WriteLine("It's also possible to create the patches without diffs");
	Console.WriteLine();
	Console.WriteLine("```csharp");
	Console.WriteLine("var patches = Patch.Compute(text1, text2);");
	Console.WriteLine("foreach (var patch in patches)");
	Console.WriteLine("{");
	Console.WriteLine("	Console.WriteLine(diff.ToString().Trim());");
	Console.WriteLine("}");
	Console.WriteLine("```");
	Console.WriteLine();
	Console.WriteLine("And the output here is the same:");
	Console.WriteLine();
	Console.WriteLine("```text");
	var patches2 = Patch.Compute(text1, text2);
	foreach (var patch in patches2)
	{
		Console.WriteLine(patch.ToString().Trim());
	}
	Console.WriteLine("```");
	Console.WriteLine();
	Console.WriteLine("Once we have the diff or patch output, there are various 'readable' formats to output in");
	Console.WriteLine();
	Console.WriteLine("```csharp");
	Console.WriteLine("diffs.ToHtml();");
	Console.WriteLine("diffs.ToHtmlDocument();");
	Console.WriteLine("diffs.ToReadableText();");
	Console.WriteLine("diffs.ToText();");
	Console.WriteLine("patches.ToHtml();");
	Console.WriteLine("patches.ToHtmlDocument();");
	Console.WriteLine("patches.ToReadableText();");
	Console.WriteLine("patches.ToText();");
	Console.WriteLine("```");
	Console.WriteLine();
	Console.WriteLine("These can be displayed on screen or saved to a file within your own code");
	Console.WriteLine();
	Console.WriteLine("Example output can be seen in the ApprovalTest output: ");
	Console.WriteLine();
	Console.WriteLine("* [diffs.ToHtml()](src/google-diff-match-patch-tests/OutputTests.CorrectDiffHtmlOutput_TextInput.approved.txt)");
	Console.WriteLine("* [diffs.ToHtmlDocument()](src/google-diff-match-patch-tests/OutputTests.CorrectDiffHtmlDocOutput_TextInput.approved.txt)");
	Console.WriteLine("* [diffs.ToReadableText()](src/google-diff-match-patch-tests/OutputTests.CorrectDiffTextOutput_TextInput.approved.txt)");
	Console.WriteLine("* [diffs.ToText()](src/google-diff-match-patch-tests/OutputTests.CorrectDiffRawTextOutput_TextInput.approved.txt)");
	Console.WriteLine("* [patches.ToHtml()](src/google-diff-match-patch-tests/OutputTests.CorrectPatchHtmlOutput_TextInput.approved.txt)");
	Console.WriteLine("* [patches.ToHtmlDocument()](src/google-diff-match-patch-tests/OutputTests.CorrectPatchHtmlDocOutput_TextInput.approved.txt)");
	Console.WriteLine("* [patches.ToReadableText()](src/google-diff-match-patch-tests/OutputTests.CorrectPatchTextOutput_TextInput.approved.txt)");
	Console.WriteLine("* [patches.ToText()](src/google-diff-match-patch-tests/OutputTests.CorrectPatchRawTextOutput_TextInput.approved.txt)");
}

// Define other methods and classes here
