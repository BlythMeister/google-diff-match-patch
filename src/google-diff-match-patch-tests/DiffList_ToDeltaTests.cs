using DiffMatchPatch;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiffMatchPatchTests
{
    [TestFixture]
    public class DiffList_ToDeltaTests
    {
        [Test]
        public void ToDelta_GeneratesExpectedOutput()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal("jump"),
                Diff.Delete("s"),
                Diff.Insert("ed"),
                Diff.Equal(" over "),
                Diff.Delete("the"),
                Diff.Insert("a"),
                Diff.Equal(" lazy"),
                Diff.Insert("old dog")
            };
            var delta = diffs.ToDelta();
            Assert.AreEqual("=4\t-1\t+ed\t=6\t-3\t+a\t=5\t+old dog", delta);
        }

        [Test]
        public void FromDelta_GeneratesExpectedDiffs()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal("jump"),
                Diff.Delete("s"),
                Diff.Insert("ed"),
                Diff.Equal(" over "),
                Diff.Delete("the"),
                Diff.Insert("a"),
                Diff.Equal(" lazy"),
                Diff.Insert("old dog")
            };
            var delta = diffs.ToDelta();
            var result = DiffList.FromDelta(diffs.Text1(), delta);
            CollectionAssert.AreEqual(diffs, result.ToList());
        }

        [Test]
        public void ToDelta_InputTooLong_Throws()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal("jump"),
                Diff.Delete("s"),
                Diff.Insert("ed"),
                Diff.Equal(" over "),
                Diff.Delete("the"),
                Diff.Insert("a"),
                Diff.Equal(" lazy"),
                Diff.Insert("old dog")
            };
            var delta = diffs.ToDelta();
            var text1 = diffs.Text1() + "x";
            Assert.Throws<ArgumentException>(() => DiffList.FromDelta(text1, delta).ToList());
        }

        [Test]
        public void ToDelta_InputTooShort_Throws()
        {
            var diffs = new List<Diff>
            {
                Diff.Equal("jump"),
                Diff.Delete("s"),
                Diff.Insert("ed"),
                Diff.Equal(" over "),
                Diff.Delete("the"),
                Diff.Insert("a"),
                Diff.Equal(" lazy"),
                Diff.Insert("old dog")
            };
            var delta = diffs.ToDelta();
            var text1 = diffs.Text1().Substring(1);
            Assert.Throws<ArgumentException>(() => DiffList.FromDelta(text1, delta).ToList());
        }

        [Test]
        public void Delta_SpecialCharacters_Works()
        {
            var zero = (char)0;
            var one = (char)1;
            var two = (char)2;
            var diffs2 = new List<Diff>
            {
                Diff.Equal("\u0680 " + zero + " \t %"),
                Diff.Delete("\u0681 " + one + " \n ^"),
                Diff.Insert("\u0682 " + two + " \\ |")
            };
            var text1 = diffs2.Text1();
            Assert.AreEqual("\u0680 " + zero + " \t %\u0681 " + one + " \n ^", text1);

            var delta = diffs2.ToDelta();
            // Lowercase, due to UrlEncode uses lower.
            Assert.AreEqual("=7\t-7\t+%da%82 %02 %5c %7c", delta, "diff_toDelta: Unicode.");

            CollectionAssert.AreEqual(diffs2, DiffList.FromDelta(text1, delta).ToList(), "diff_fromDelta: Unicode.");
        }

        [Test]
        public void Delta_FromUnchangedCharacters_Succeeds()
        {
            // Verify pool of unchanged characters.
            var expected = new List<Diff>
            {
                Diff.Insert("A-Z a-z 0-9 - _ . ! ~ * ' ( ) ; / ? : @ & = + $ , # ")
            };
            var text2 = expected.Text2();
            Assert.AreEqual("A-Z a-z 0-9 - _ . ! ~ * \' ( ) ; / ? : @ & = + $ , # ", text2);

            var delta = expected.ToDelta();
            Assert.AreEqual("+A-Z a-z 0-9 - _ . ! ~ * \' ( ) ; / ? : @ & = + $ , # ", delta);

            // Convert delta string into a diff.
            var actual = DiffList.FromDelta("", delta);
            CollectionAssert.AreEqual(expected, actual.ToList(), "diff_fromDelta: Unchanged characters.");
        }

        [Test]
        public void Delta_LargeString()
        {
            // 160 kb string.
            var a = "abcdefghij";
            for (var i = 0; i < 14; i++)
            {
                a += a;
            }
            var diffs2 = new List<Diff> { Diff.Insert(a) };
            var delta = diffs2.ToDelta();
            Assert.AreEqual("+" + a, delta);

            // Convert delta string into a diff.
            CollectionAssert.AreEqual(diffs2, DiffList.FromDelta("", delta).ToList(), "diff_fromDelta: 160kb string.");
        }
    }
}
