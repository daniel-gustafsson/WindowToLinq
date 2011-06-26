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
    public sealed class TestExamples
    {
        [Test]
        public void Example1()
        {
            var result = Enumerable.Range(1, 10)
                .Window(
                    i => i >= -1    // Window start 1 element before current
                    , i => i <= 1   // Window ends 1 element after current
                    , (source, window) => Tuple.Create(source, window.Count(), window.Sum()));

            // When iterated on, the result sequence should be the same as
            var expected = new List<Tuple<int, int, int>> {
                    Tuple.Create(1, 2, 3)
                    , Tuple.Create(2, 3, 6)
                    , Tuple.Create(3, 3, 9)
                    , Tuple.Create(4, 3, 12)
                    , Tuple.Create(5, 3, 15)
                    , Tuple.Create(6, 3, 18)
                    , Tuple.Create(7, 3, 21)
                    , Tuple.Create(8, 3, 24)
                    , Tuple.Create(9, 3, 27)
                    , Tuple.Create(10, 2, 19)
                };
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void Example2()
        {
            var source = from s in Enumerable.Range(1, 3)
                         from v in Enumerable.Range(s, 2)
                         select Tuple.Create(s, v);

            var result = source.Window(
                    s => s.Item1    // Partition on first value
                    , i => true     // No preceding bound
                    , i => true     // No following bound
                    , (src, window) => window.Sum(w => w.Item2));

            var expected = new int[] { 3, 3, 5, 5, 7, 7 };
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void Example3()
        {
            var result = Enumerable.Range(1, 10)
                .Window(i => i >= -1, i => i <= 1)
                .Count()
                .Sum()
                .Select((src, count, sum) => Tuple.Create(src, count, sum));

            var expected = new List<Tuple<int, int, int>> {
                    Tuple.Create(1, 2, 3)
                    , Tuple.Create(2, 3, 6)
                    , Tuple.Create(3, 3, 9)
                    , Tuple.Create(4, 3, 12)
                    , Tuple.Create(5, 3, 15)
                    , Tuple.Create(6, 3, 18)
                    , Tuple.Create(7, 3, 21)
                    , Tuple.Create(8, 3, 24)
                    , Tuple.Create(9, 3, 27)
                    , Tuple.Create(10, 2, 19)
                };
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void Example4()
        {
            var result = Enumerable.Range(1, 10000000)
                .WindowUnboundedPreceding(i => i <= 0)
                .Sum()
                .Select((src, sum) => sum)
                .Skip(1000)
                .Take(5);

            var expected = new int[] { 501501, 502503, 503506, 504510, 505515 };
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void Example5()
        {
            var result = Enumerable.Range(1, 10)
                .WindowUnboundedPreceding(i => i <= 0)
                .Count().Where(s => s % 2 == 0).Where(s => s % 3 == 0) // Accumulated count of every sixth element
                .Select((src, sum) => sum);

            var expected = new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void Example6()
        {
            var result = Enumerable.Range(1, 10)
                .WindowUnboundedPreceding(i => i <= 0)
                .Count()
                .Sum()
                .Select((src, count, sum) => Tuple.Create(count, sum))
                .Last();

            var expected = Tuple.Create(10, 55);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Example7()
        {
            var result = Enumerable.Range(1, 10)
                .BeginAggregate()
                .Count()
                .Sum()
                .Compute();

            var expected = Tuple.Create(10, 55);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Example8()
        {
            var source = new SinglePassSequence<int>(Enumerable.Range(1, 100));
            
            var result = source
                .Zip(source.Skip(1), (l, r) => Tuple.Create(l, r))
                .Zip(source.Skip(2), (l, r) => Tuple.Create(l.Item1, l.Item2, r))
                .Skip(10)
                .Take(2);

            var expected = new List<Tuple<int, int, int>> {
                    Tuple.Create(11, 12, 13)
                    , Tuple.Create(12, 13, 14)
                };
            Assert.That(result.SequenceEqual(expected));
        }
    }
}
