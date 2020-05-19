using DiffMatchPatch;
using NUnit.Framework;
using System.Collections.Generic;

namespace DiffMatchPatchTests
{
    [TestFixture]
    public class DiffList_CleanupSemanticTests
    {
        [Test]
        public void EmptyList()
        {
            var diffs = new List<Diff>();
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>(), diffs);
        }

        [Test]
        public void NoEliminiation1()
        {
            var diffs = new List<Diff>
            {
                Diff.Delete("ab"),
                Diff.Insert("cd"),
                Diff.Equal("12"),
                Diff.Delete("e")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Delete("ab"),
                Diff.Insert("cd"),
                Diff.Equal("12"),
                Diff.Delete("e")
            }, diffs);
        }

        [Test]
        public void NoElimination2()
        {
            var diffs = new List<Diff>
            {
                Diff.Delete("abc"),
                Diff.Insert("ABC"),
                Diff.Equal("1234"),
                Diff.Delete("wxyz")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Delete("abc"),
                Diff.Insert("ABC"),
                Diff.Equal("1234"),
                Diff.Delete("wxyz")
            }, diffs);
        }

        [Test]
        public void SimpleElimination()
        {
            var diffs = new List<Diff>
            {
                Diff.Delete("a"),
                Diff.Equal("b"),
                Diff.Delete("c")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Delete("abc"),
                Diff.Insert("b")
            }, diffs);
        }

        [Test]
        public void BackpassElimination()
        {
            var diffs = new List<Diff>
            {
                Diff.Delete("ab"),
                Diff.Equal("cd"),
                Diff.Delete("e"),
                Diff.Equal("f"),
                Diff.Insert("g")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Delete("abcdef"),
                Diff.Insert("cdfg")
            }, diffs);
        }

        [Test]
        public void MultipleEliminations()
        {
            var diffs = new List<Diff>
            {
                Diff.Insert("1"),
                Diff.Equal("A"),
                Diff.Delete("B"),
                Diff.Insert("2"),
                Diff.Equal("_"),
                Diff.Insert("1"),
                Diff.Equal("A"),
                Diff.Delete("B"),
                Diff.Insert("2")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Delete("AB_AB"),
                Diff.Insert("1A2_1A2")
            }, diffs);
        }

        [Test]
        public void WordBoundaries()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal("The c"),
                Diff.Delete("ow and the c"),
                Diff.Equal("at.")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Equal("The "),
                Diff.Delete("cow and the "),
                Diff.Equal("cat.")
            }, diffs);
        }

        [Test]
        public void NoOverlapElimination()
        {
            var diffs = new List<Diff>
            {
                Diff.Delete("abcxx"),
                Diff.Insert("xxdef")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Delete("abcxx"),
                Diff.Insert("xxdef")
            }, diffs);
        }

        [Test]
        public void OverlapElimination()
        {
            var diffs = new List<Diff>
            {
                Diff.Delete("abcxxx"),
                Diff.Insert("xxxdef")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Delete("abc"),
                Diff.Equal("xxx"),
                Diff.Insert("def")
            }, diffs);
        }

        [Test]
        public void ReverseOverlapElimination()
        {
            var diffs = new List<Diff>
            {
                Diff.Delete("xxxabc"),
                Diff.Insert("defxxx")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Insert("def"),
                Diff.Equal("xxx"),
                Diff.Delete("abc")
            }, diffs);
        }

        [Test]
        public void TwoOverlapEliminations()
        {
            var diffs = new List<Diff>
            {
                Diff.Delete("abcd1212"),
                Diff.Insert("1212efghi"),
                Diff.Equal("----"),
                Diff.Delete("A3"),
                Diff.Insert("3BC")
            };
            diffs.CleanupSemantic();
            CollectionAssert.AreEqual(new List<Diff>
            {
                Diff.Delete("abcd"),
                Diff.Equal("1212"),
                Diff.Insert("efghi"),
                Diff.Equal("----"),
                Diff.Delete("A"),
                Diff.Equal("3"),
                Diff.Insert("BC")
            }, diffs);
        }
    }
}
