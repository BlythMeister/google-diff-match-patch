using DiffMatchPatch;
using NUnit.Framework;
using System.Collections.Generic;

namespace DiffMatchPatchTests
{
    [TestFixture]
    public class DiffList_CleanupMergeTests
    {
        [Test]
        public void CleanupMerge_EmptyDiffList_ReturnsEmptyDiffList()
        {
            // Cleanup a messy diff.
            // Null case.
            var result = new List<Diff>();
            result.CleanupMerge();
            var expected = new List<Diff>();

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void CleanupMerge_AlreadyCleaned_ReturnsSameList()
        {
            var result = new List<Diff> { Diff.Equal("a"), Diff.Delete("b"), Diff.Insert("c") };
            result.CleanupMerge();

            var expected = new[] { Diff.Equal("a"), Diff.Delete("b"), Diff.Insert("c") };

            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void SubsequentEqualitiesAreMerged()
        {
            var diffs = new List<Diff> { Diff.Equal("a"), Diff.Equal("b"), Diff.Equal("c") };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Equal("abc") }, diffs);
        }

        [Test]
        public void SubsequentDeletesAreMerged()
        {
            var diffs = new List<Diff> { Diff.Delete("a"), Diff.Delete("b"), Diff.Delete("c") };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Delete("abc") }, diffs);
        }

        [Test]
        public void SubsequentInsertsAreMerged()
        {
            var diffs = new List<Diff> { Diff.Insert("a"), Diff.Insert("b"), Diff.Insert("c") };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Insert("abc") }, diffs);
        }

        [Test]
        public void InterweavedInsertDeletesAreMerged()
        {
            // Merge interweave.
            var diffs = new List<Diff>
            {
                Diff.Delete("a"),
                Diff.Insert("b"),
                Diff.Delete("c"),
                Diff.Insert("d"),
                Diff.Equal("e"),
                Diff.Equal("f")
            };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Delete("ac"), Diff.Insert("bd"), Diff.Equal("ef") }, diffs);
        }

        [Test]
        public void PrefixSuffixDetection()
        {
            // Prefix and suffix detection.
            var diffs = new List<Diff> { Diff.Delete("a"), Diff.Insert("abc"), Diff.Delete("dc") };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(
                new List<Diff> { Diff.Equal("a"), Diff.Delete("d"), Diff.Insert("b"), Diff.Equal("c") }, diffs);
        }

        [Test]
        public void PrefixSuffixDetectionWithEqualities()
        {
            // Prefix and suffix detection.
            var diffs = new List<Diff>
            {
                Diff.Equal("x"),
                Diff.Delete("a"),
                Diff.Insert("abc"),
                Diff.Delete("dc"),
                Diff.Equal("y")
            };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(
                new List<Diff> { Diff.Equal("xa"), Diff.Delete("d"), Diff.Insert("b"), Diff.Equal("cy") }, diffs);
        }

        [Test]
        public void SlideEditLeft()
        {
            // Slide edit left.
            var diffs = new List<Diff> { Diff.Equal("a"), Diff.Insert("ba"), Diff.Equal("c") };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Insert("ab"), Diff.Equal("ac") }, diffs);
        }

        [Test]
        public void SlideEditRight()
        {
            // Slide edit right.
            var diffs = new List<Diff> { Diff.Equal("c"), Diff.Insert("ab"), Diff.Equal("a") };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Equal("ca"), Diff.Insert("ba") }, diffs);
        }

        [Test]
        public void SlideEditLeftRecursive()
        {
            // Slide edit left recursive.
            var diffs = new List<Diff>
            {
                Diff.Equal("a"),
                Diff.Delete("b"),
                Diff.Equal("c"),
                Diff.Delete("ac"),
                Diff.Equal("x")
            };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Delete("abc"), Diff.Equal("acx") }, diffs);
        }

        [Test]
        public void SlideEditRightRecursive()
        {
            // Slide edit right recursive.
            var diffs = new List<Diff>
            {
                Diff.Equal("x"),
                Diff.Delete("ca"),
                Diff.Equal("c"),
                Diff.Delete("b"),
                Diff.Equal("a")
            };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Equal("xca"), Diff.Delete("cba") }, diffs);
        }

        [Test]
        public void EmptyMerge()
        {
            var diffs = new List<Diff>
            {
                Diff.Delete("b"),
                Diff.Insert("ab"),
                Diff.Equal("c")
            };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Insert("a"), Diff.Equal("bc") }, diffs);
        }

        [Test]
        public void EmptyEquality()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal(""),
                Diff.Insert("a"),
                Diff.Equal("b")
            };
            diffs.CleanupMerge();
            CollectionAssert.AreEqual(new List<Diff> { Diff.Insert("a"), Diff.Equal("b") }, diffs);
        }
    }
}
