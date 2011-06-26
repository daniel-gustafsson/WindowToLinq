using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WindowToLinq;

namespace WindowToLinq.Test
{
    [TestFixture]
    public sealed class TestSinglePassBuffer
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
        public void OneIteration(
            [ValueSource("CountRange")] int count)
        {
            var source = new SinglePassSequence<int>(Enumerable.Range(1, count));
            var expected = Enumerable.Range(1, count);
            Assert.That(source.SequenceEqual(expected));
        }

        [Test]
        public void MultipleIterations(
            [Random(Int32.MinValue, Int32.MaxValue, 1)] int seed
            , [Range(5, 20, 15)] int iterators
            , [Range(20, 2000, 1980)] int range)
        {
            Random rnd = new Random(seed);
            var source = new SinglePassSequence<int>(Enumerable.Range(1, range));
            var enumerators = Enumerable.Range(1, iterators).Select(x => source.GetEnumerator()).ToList();
            var resultBuffers = Enumerable.Range(1, iterators).Select(x => new List<int>(range)).ToList();

            while (enumerators.Count > 0)
            {
                int e = rnd.Next(enumerators.Count);
                if (enumerators[e].MoveNext())
                {
                    resultBuffers[e].Add(enumerators[e].Current);
                }
                else
                {
                    Assert.That(Enumerable.Range(1, range).SequenceEqual(resultBuffers[e]));
                    resultBuffers.RemoveAt(e);
                    enumerators[e].Dispose();
                    enumerators.RemoveAt(e);
                }
            }
        }
    }
}
