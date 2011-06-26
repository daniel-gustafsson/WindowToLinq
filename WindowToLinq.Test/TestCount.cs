using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WindowToLinq;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WindowToLinq.Test
{
    [TestFixture]
    public sealed class TestCount
    {
        public IEnumerable<int> CountRange
        {
            get
            {
                foreach (int c in Enumerable.Range(0, 5))
                    yield return c;
                foreach (int c in Enumerable.Range(1, 5))
                    yield return c * 10;
                foreach (int c in Enumerable.Range(2, 3))
                    yield return c * 50;
            }
        }

        [Test]
        public void Count(
            [ValueSource("CountRange")] int count)
        {
            var result = Enumerable.Range(1, count)
                .WindowUnboundedPreceding(i => i <= 0)
                .Count()
                .Select((s, c) => c);
            foreach (var v in result)
                Console.WriteLine(v);
            Assert.That(result.SequenceEqual(Enumerable.Range(1, count)));
        }

        [Test]
        public void LongCount(
            [ValueSource("CountRange")] int count)
        {
            var result = Enumerable.Repeat(1, count)
                .WindowUnboundedPreceding(i => i <= 0)
                .LongCount()
                .Select((s, c) => c);
            Assert.That(result.SequenceEqual(Enumerable.Range(1, count).Select(r => (long)r)));
        }

        [Test]
        public void CountPredicate(
            [ValueSource("CountRange")] int count)
        {
            var source = Enumerable.Range(1, count)
                .WindowUnboundedPreceding(i => i <= 0)
                .Count(x => x % 2 == 0)
                .Select((s, c) => c);
            var expected = Enumerable.Range(1, count).Select(x => x / 2);
            Assert.That(source.SequenceEqual(expected));
        }

        [Test]
        public void LongCountPredicate(
            [ValueSource("CountRange")] int count)
        {
            var source = Enumerable.Range(1, count)
                .WindowUnboundedPreceding(i => i <= 0)
                .LongCount(x => x % 2 == 0)
                .Select((s, c) => c);
            var expected = Enumerable.Range(1, count).Select(x => (long)x / 2);
            Assert.That(source.SequenceEqual(expected));
        }
    }
}
