using DiffMatchPatch;
using NUnit.Framework;
using System.Collections.Generic;

namespace DiffMatchPatchTests
{
    [TestFixture]
    public class DiffList_CleanupSemanticLosslessTests
    {
        [Test]
        public void EmptyList_WhenCleaned_RemainsEmptyList()
        {
            // Slide Diffs to match logical boundaries.
            // Null case.
            var diffs = new List<Diff>();
            diffs.CleanupSemanticLossless();
            CollectionAssert.AreEqual(new List<Diff>(), diffs);
        }

        [Test]
        public void BlankLines()
        {
            // Blank lines.
            var diffs = new List<Diff>
            {
                Diff.Equal("AAA\r\n\r\nBBB"),
                Diff.Insert("\r\nDDD\r\n\r\nBBB"),
                Diff.Equal("\r\nEEE")
            };
            diffs.CleanupSemanticLossless();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Equal("AAA\r\n\r\n"),
                Diff.Insert("BBB\r\nDDD\r\n\r\n"),
                Diff.Equal("BBB\r\nEEE")
            }, diffs);
        }

        [Test]
        public void NoCleanup()
        {
            // Line boundaries.
            var diffs = new List<Diff>
            {
                Diff.Equal("AAA\r\n"),
                Diff.Insert("BBB DDD\r\n"),
                Diff.Equal("BBB EEE\r\n"),
                Diff.Insert("FFF GGG\r\n"),
                Diff.Equal("HHH III"),
            };
            diffs.CleanupSemanticLossless();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Equal("AAA\r\n"),
                Diff.Insert("BBB DDD\r\n"),
                Diff.Equal("BBB EEE\r\n"),
                Diff.Insert("FFF GGG\r\n"),
                Diff.Equal("HHH III"),
            }, diffs);
        }

        [Test]
        public void LineBoundaries()
        {
            // Line boundaries.
            var diffs = new List<Diff>
            {
                Diff.Equal("AAA\r\nBBB"),
                Diff.Insert(" DDD\r\nBBB"),
                Diff.Equal(" EEE")
            };
            diffs.CleanupSemanticLossless();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Equal("AAA\r\n"),
                Diff.Insert("BBB DDD\r\n"),
                Diff.Equal("BBB EEE")
            }, diffs);
        }

        [Test]
        public void WordBoundaries()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal("The c"),
                Diff.Insert("ow and the c"),
                Diff.Equal("at.")
            };
            diffs.CleanupSemanticLossless();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Equal("The "),
                Diff.Insert("cow and the "),
                Diff.Equal("cat.")
            }, diffs);
        }

        [Test]
        public void AlphaNumericBoundaries()
        {
            // Alphanumeric boundaries.
            var diffs = new List<Diff>
            {
                Diff.Equal("The-c"),
                Diff.Insert("ow-and-the-c"),
                Diff.Equal("at.")
            };
            diffs.CleanupSemanticLossless();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Equal("The-"),
                Diff.Insert("cow-and-the-"),
                Diff.Equal("cat.")
            }, diffs);
        }

        [Test]
        public void HittingTheStart()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal("a"),
                Diff.Delete("a"),
                Diff.Equal("ax")
            };
            diffs.CleanupSemanticLossless();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Delete("a"),
                Diff.Equal("aax")
            }, diffs);
        }

        [Test]
        public void HittingTheEnd()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal("xa"),
                Diff.Delete("a"),
                Diff.Equal("a")
            };
            diffs.CleanupSemanticLossless();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Equal("xaa"),
                Diff.Delete("a")
            }, diffs);
        }

        [Test]
        public void SentenceBoundaries()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal("The xxx. The "),
                Diff.Insert("zzz. The "),
                Diff.Equal("yyy.")
            };
            diffs.CleanupSemanticLossless();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Equal("The xxx."),
                Diff.Insert(" The zzz."),
                Diff.Equal(" The yyy.")
            }, diffs);
        }
    }
}
