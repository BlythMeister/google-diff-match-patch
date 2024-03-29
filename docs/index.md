---
title: Home
description: "Evolution of the C# port of the google diff-match-patch implementation."
---

Evolution of the C# port of the google diff-match-patch implementation. - <https://github.com/google/diff-match-patch>

## Usage

Given 2 input strings, for example:

```csharp
var text1 = "start\n"
            "line with some content\n"
            "unchanged content\n"
            "removed line\n"
            "end";

var text2 = "start\n"
            "new line\n"
            "line with some modified content\n"
            "unchanged content\n"
            "end";
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
Diff(Insert,"new·line¶")
Diff(Equal,"line·with·some·")
Diff(Insert,"modified·")
Diff(Equal,"content¶unchanged·content")
Diff(Delete,"¶removed·line")
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

* [diffs.ToHtml()](https://github.com/BlythMeister/google-diff-match-patch/blob/master/src/google-diff-match-patch-tests/OutputTests.CorrectDiffHtmlOutput_TextInput.approved.html)
* [diffs.ToHtmlDocument()](https://github.com/BlythMeister/google-diff-match-patch/blob/master/src/google-diff-match-patch-tests/OutputTests.CorrectDiffHtmlDocOutput_TextInput.approved.html)
* [diffs.ToReadableText()](https://github.com/BlythMeister/google-diff-match-patch/blob/master/src/google-diff-match-patch-tests/OutputTests.CorrectDiffTextOutput_TextInput.approved.txt)
* [diffs.ToText()](https://github.com/BlythMeister/google-diff-match-patch/blob/master/src/google-diff-match-patch-tests/OutputTests.CorrectDiffRawTextOutput_TextInput.approved.txt)
* [patches.ToHtml()](https://github.com/BlythMeister/google-diff-match-patch/blob/master/src/google-diff-match-patch-tests/OutputTests.CorrectPatchHtmlOutput_TextInput.approved.html)
* [patches.ToHtmlDocument()](https://github.com/BlythMeister/google-diff-match-patch/blob/master/src/google-diff-match-patch-tests/OutputTests.CorrectPatchHtmlDocOutput_TextInput.approved.html)
* [patches.ToReadableText()](https://github.com/BlythMeister/google-diff-match-patch/blob/master/src/google-diff-match-patch-tests/OutputTests.CorrectPatchTextOutput_TextInput.approved.txt)
* [patches.ToText()](https://github.com/BlythMeister/google-diff-match-patch/blob/master/src/google-diff-match-patch-tests/OutputTests.CorrectPatchRawTextOutput_TextInput.approved.txt)
