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
    [TestFixture(typeof(int))]
    [TestFixture(typeof(int?))]
    [TestFixture(typeof(long))]
    [TestFixture(typeof(long?))]
    [TestFixture(typeof(float))]
    [TestFixture(typeof(float?))]
    [TestFixture(typeof(double))]
    [TestFixture(typeof(double?))]
    [TestFixture(typeof(decimal))]
    [TestFixture(typeof(decimal?))]
    public sealed class TestSum<T>
    {
        Func<int?, T> cast = Lambda.Cast<int?, T>();

        IEnumerable<T> CastSource(IEnumerable<int?> source)
        {
            return source.Select(cast);
        }


        [Test]
        public void RollingSum()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new int?[] { 1, 3, 6, 10, 15 };
            dynamic query = CastSource(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Sum(query);
            query = WindowExtension.Select(query, (Func<T, T, T>)((d, sum) => sum));

            IEnumerable<T> result = query;
            Assert.That(result.SequenceEqual(CastSource(expected)));
        }

        [Test]
        public void RollingSumFiltered()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new int?[] { 0, 2, 5, 9, 9 };
            dynamic query = CastSource(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Sum(query);
            query = WindowExtension.Where(query, Lambda.Between<T>(cast(2), cast(4)));
            query = WindowExtension.Select(query, (Func<T, T, T>)((d, sum) => sum));

            IEnumerable<T> result = query;
            Assert.That(result.SequenceEqual(CastSource(expected)));
        }

        [Test]
        public void RollingSumFilteredTwice()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new int?[] { 0, 2, 5, 5, 5 };
            dynamic query = CastSource(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Sum(query);
            query = WindowExtension.Where(query, Lambda.LessThan<T>(cast(4)));
            query = WindowExtension.Where(query, Lambda.GreaterThan<T>(cast(1)));
            query = WindowExtension.Select(query, (Func<T, T, T>)((d, sum) => sum));

            IEnumerable<T> result = query;
            Assert.That(result.SequenceEqual(CastSource(expected)));
        }

        [Test]
        public void RollingSumSelector()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new int?[] { 1, 3, 6, 10, 15 };
            dynamic query = Enumerable.Zip(CastSource(source), CastSource(source), (Func<T, T, Tuple<T, T>>)((l, r) => Tuple.Create(l, r)));
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Sum(query, (Func<Tuple<T, T>, T>)(a => a.Item1));
            query = WindowExtension.Select(query, (Func<Tuple<T, T>, T, T>)((d, sum) => sum));

            IEnumerable<T> result = query;
            Assert.That(result.SequenceEqual(CastSource(expected)));
        }

        [Test]
        public void RollingSumFilteredSelector()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new int?[] { 0, 2, 5, 9, 9 };
            dynamic query = Enumerable.Zip(CastSource(source), CastSource(source), (Func<T, T, Tuple<T, T>>)((l, r) => Tuple.Create(l, r)));
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Sum(query, (Func<Tuple<T, T>, T>)(a => a.Item1));
            query = WindowExtension.Where(query, Lambda.Between<Tuple<T, T>, T>(cast(2), cast(4), "Item1"));
            query = WindowExtension.Select(query, (Func<Tuple<T, T>, T, T>)((d, sum) => sum));

            IEnumerable<T> result = query;
            Assert.That(result.SequenceEqual(CastSource(expected)));
        }

        [Test]
        public void RollingSumFilteredTwiceSelector()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new int?[] { 0, 2, 5, 5, 5 };
            dynamic query = Enumerable.Zip(CastSource(source), CastSource(source), (Func<T, T, Tuple<T, T>>)((l, r) => Tuple.Create(l, r)));
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Sum(query, (Func<Tuple<T, T>, T>)(a => a.Item1));
            query = WindowExtension.Where(query, Lambda.LessThan<Tuple<T, T>, T>(cast(4), "Item1"));
            query = WindowExtension.Where(query, Lambda.GreaterThan<Tuple<T, T>, T>(cast(1), "Item1"));
            query = WindowExtension.Select(query, (Func<Tuple<T, T>, T, T>)((d, sum) => sum));

            IEnumerable<T> result = query;
            Assert.That(result.SequenceEqual(CastSource(expected)));
        }
    }
}
