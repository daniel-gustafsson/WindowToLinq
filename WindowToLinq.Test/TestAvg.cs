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
    [TestFixture(typeof(int), typeof(double))]
    [TestFixture(typeof(int?), typeof(double?))]
    [TestFixture(typeof(long), typeof(double))]
    [TestFixture(typeof(long?), typeof(double?))]
    [TestFixture(typeof(float), typeof(float))]
    [TestFixture(typeof(float?), typeof(float?))]
    [TestFixture(typeof(double), typeof(double))]
    [TestFixture(typeof(double?), typeof(double?))]
    [TestFixture(typeof(decimal), typeof(decimal))]
    [TestFixture(typeof(decimal?), typeof(decimal?))]
    public sealed class TestAvg<T, TAvg>
    {
        Func<int?, T> cast_int = Lambda.Cast<int?, T>();
        Func<double?, TAvg> cast_double = Lambda.Cast<double?, TAvg>();

        IEnumerable<T> CastSource(IEnumerable<int?> source)
        {
            return source.Select(cast_int);
        }

        IEnumerable<TAvg> CastExpected(IEnumerable<double?> expected)
        {
            return expected.Select(cast_double);
        }

        [Test]
        public void RollingAvg()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new double?[] { 1, 1.5, 2, 2.5, 3 };
            dynamic query = CastSource(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Average(query);
            query = WindowExtension.Select(query, (Func<T, TAvg, TAvg>)((d, avg) => avg));

            IEnumerable<TAvg> result = query;
            Assert.That(result.SequenceEqual(CastExpected(expected)));
        }

        [Test]
        public void RollingAvgFiltered()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new double?[] { null, 2, 2.5, 3, 3 };
            dynamic query = CastSource(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Average(query);
            query = WindowExtension.Where(query, Lambda.Between<T>(cast_int(2), cast_int(4)));
            query = WindowExtension.Select(query, (Func<T, TAvg, TAvg>)((d, avg) => avg));

            IEnumerable<TAvg> result = query;
            Assert.That(result.SequenceEqual(CastExpected(expected)));
        }

        [Test]
        public void RollingAvgFilteredTwice()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new double?[] { null, 2, 2.5, 2.5, 2.5 };
            dynamic query = CastSource(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Average(query);
            query = WindowExtension.Where(query, Lambda.LessThan<T>(cast_int(4)));
            query = WindowExtension.Where(query, Lambda.GreaterThan<T>(cast_int(1)));
            query = WindowExtension.Select(query, (Func<T, TAvg, TAvg>)((d, avg) => avg));

            IEnumerable<TAvg> result = query;
            Assert.That(result.SequenceEqual(CastExpected(expected)));
        }

        [Test]
        public void RollingAvgFilteredTwiceSelector()
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new double?[] { null, 2, 2.5, 2.5, 2.5 };
            dynamic query = Enumerable.Zip(CastSource(source), CastSource(source), (Func<T, T, Tuple<T, T>>)((l, r) => Tuple.Create(l, r)));
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Average(query, (Func<Tuple<T, T>, T>)(a => a.Item1));
            query = WindowExtension.Where(query, Lambda.LessThan<Tuple<T, T>, T>(cast_int(4), "Item1"));
            query = WindowExtension.Where(query, Lambda.GreaterThan<Tuple<T, T>, T>(cast_int(1), "Item1"));
            query = WindowExtension.Select(query, (Func<Tuple<T, T>, TAvg, TAvg>)((d, sum) => sum));

            IEnumerable<TAvg> result = query;
            Assert.That(result.SequenceEqual(CastExpected(expected)));
        }
    }
}
