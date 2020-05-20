# google-diff-match-patch

[![AppVeyor branch](https://img.shields.io/appveyor/ci/blythmeister/google-diff-match-patch)](https://ci.appveyor.com/project/BlythMeister/google-diff-match-patch)
[![Nuget](https://img.shields.io/nuget/v/google-diff-match-patch)](https://www.nuget.org/packages/google-diff-match-patch/)
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/BlythMeister/google-diff-match-patch)](https://github.com/BlythMeister/google-diff-match-patch/releases/latest)

Evolution of the C# port of the google diff-match-patch implementation. - https://github.com/google/diff-match-patch

## Usages

Given 2 input strings, for example:

```csharp
var text1 = "start\nline with some content\nunchanged content\nremoved line\nend"
var text2 = "start\nnew line\nline with some modified content\nunchanged content\nend"
```

We can calculate the differences by doing

```csharp
var diffs = Diff.Compute(text1, text2);
foreach (var diff in diffs)
{
  Console.WriteLine(diff.ToString().Trim());
}
```

This would give output of:

```text
Diff(Equal,"start¶")
Diff(Insert,"new line¶")
Diff(Equal,"line with some ")
Diff(Insert,"modified ")
Diff(Equal,"content¶unchanged content")
Diff(Delete,"¶removed line")
Diff(Equal,"¶end")
```

From a diff collection we can create a set of patches:

```csharp
var patches = Patch.FromDiffs(diffs);
foreach (var patch in patches)
{
  Console.WriteLine(diff.ToString().Trim());
}
```

This would give output of:

```text
@@ -1,14 +1,23 @@
 start%0a
+new line%0a
 line wit
@@ -23,16 +23,25 @@
 th some 
+modified 
 content%0a
@@ -61,21 +61,8 @@
 tent
-%0aremoved line
 %0aend
```

It's also possible to create the patches without diffs

```csharp
var patches = Patch.Compute(text1, text2);
foreach (var patch in patches)
{
  Console.WriteLine(diff.ToString().Trim());
}
```

And the output here is the same:

```text
@@ -1,14 +1,23 @@
 start%0a
+new line%0a
 line wit
@@ -23,16 +23,25 @@
 th some 
+modified 
 content%0a
@@ -62,20 +62,7 @@
 ent%0a
-removed line%0a
 end
```

Once we have the diff or patch output, there are various 'readable' formats to output in

```csharp
diffs.ToHtml();
diffs.ToHtmlDocument();
diffs.ToReadableText();
diffs.ToText();
patches.ToHtml();
patches.ToHtmlDocument();
patches.ToReadableText();
patches.ToText();
```

These can be displayed on screen or saved to a file within your own code

Example output can be seen in the ApprovalTest output: 

* [diffs.ToHtml()](src/google-diff-match-patch-tests/OutputTests.CorrectDiffHtmlOutput_TextInput.approved.txt)
* [diffs.ToHtmlDocument()](src/google-diff-match-patch-tests/OutputTests.CorrectDiffHtmlDocOutput_TextInput.approved.txt)
* [diffs.ToReadableText()](src/google-diff-match-patch-tests/OutputTests.CorrectDiffTextOutput_TextInput.approved.txt)
* [diffs.ToText()](src/google-diff-match-patch-tests/OutputTests.CorrectDiffRawTextOutput_TextInput.approved.txt)
* [patches.ToHtml()](src/google-diff-match-patch-tests/OutputTests.CorrectPatchHtmlOutput_TextInput.approved.txt)
* [patches.ToHtmlDocument()](src/google-diff-match-patch-tests/OutputTests.CorrectPatchHtmlDocOutput_TextInput.approved.txt)
* [patches.ToReadableText()](src/google-diff-match-patch-tests/OutputTests.CorrectPatchTextOutput_TextInput.approved.txt)
* [patches.ToText()](src/google-diff-match-patch-tests/OutputTests.CorrectPatchRawTextOutput_TextInput.approved.txt)