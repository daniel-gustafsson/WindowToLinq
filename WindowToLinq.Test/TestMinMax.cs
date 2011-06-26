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
    [TestFixture(typeof(TestValueType))]
    [TestFixture(typeof(TestValueType?))]
    [TestFixture(typeof(string))]
    public sealed class TestMinMax<T>
    {
        Func<int?, T> cast = Lambda.Cast<int?, T>();

        IEnumerable<T> CastSequence(IEnumerable<int?> source)
        {
            return source.Select(cast);
        }


        [Test]
        public void MinMax()
        {
            var source = new int?[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new List<Tuple<T, T>>();
            for (int i = 1; i <= source.Length; ++i)
                expected.Add(Tuple.Create(CastSequence(source).Take(i).Min(), CastSequence(source).Take(i).Max()));

            dynamic query = CastSequence(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Min(query);
            query = WindowExtension.Max(query);
            query = WindowExtension.Select(query, (Func<T, T, T, Tuple<T, T>>)((d, min, max) => Tuple.Create(min, max)));

            IEnumerable<Tuple<T, T>> result = query;
            foreach (var v in expected.Zip(result, (l, r) => Tuple.Create(l, r))) ;
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void MinMaxNegative()
        {
            var source = new int?[] { -1, -2, -3, -4, -5, -6, -7, -8, -9 };
            var expected = new List<Tuple<T, T>>();
            for (int i = 1; i <= source.Length; ++i)
                expected.Add(Tuple.Create(CastSequence(source).Take(i).Min(), CastSequence(source).Take(i).Max()));

            dynamic query = CastSequence(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Min(query);
            query = WindowExtension.Max(query);
            query = WindowExtension.Select(query, (Func<T, T, T, Tuple<T, T>>)((d, min, max) => Tuple.Create(min, max)));

            IEnumerable<Tuple<T, T>> result = query;
            foreach (var v in expected.Zip(result, (l, r) => Tuple.Create(l, r))) ;
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void MinMaxNull()
        {
            var source = new int?[] { null, -2, 3, null, 5, -6, 7, null, 9 };
            var expected = new List<Tuple<T, T>>();
            for (int i = 1; i <= source.Length; ++i)
                expected.Add(Tuple.Create(CastSequence(source).Take(i).Min(), CastSequence(source).Take(i).Max()));

            dynamic query = CastSequence(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Min(query);
            query = WindowExtension.Max(query);
            query = WindowExtension.Select(query, (Func<T, T, T, Tuple<T, T>>)((d, min, max) => Tuple.Create(min, max)));

            IEnumerable<Tuple<T, T>> result = query;
            foreach (var v in expected.Zip(result, (l, r) => Tuple.Create(l, r))) ;
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void MinMaxSelector()
        {
            var source = new int?[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new List<Tuple<T, T>>();
            for (int i = 1; i <= source.Length; ++i)
                expected.Add(Tuple.Create(CastSequence(source).Take(i).Min(), CastSequence(source).Take(i).Max()));

            dynamic query = CastSequence(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Min(query, (Func<T, T>)(x => x));
            query = WindowExtension.Max(query, (Func<T, T>)(x => x));
            query = WindowExtension.Select(query, (Func<T, T, T, Tuple<T, T>>)((d, min, max) => Tuple.Create(min, max)));

            IEnumerable<Tuple<T, T>> result = query;
            foreach (var v in expected.Zip(result, (l, r) => Tuple.Create(l, r))) ;
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void MinMaxNegativeSelector()
        {
            var source = new int?[] { -1, -2, -3, -4, -5, -6, -7, -8, -9 };
            var expected = new List<Tuple<T, T>>();
            for (int i = 1; i <= source.Length; ++i)
                expected.Add(Tuple.Create(CastSequence(source).Take(i).Min(), CastSequence(source).Take(i).Max()));

            dynamic query = CastSequence(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Min(query, (Func<T, T>)(x => x));
            query = WindowExtension.Max(query, (Func<T, T>)(x => x));
            query = WindowExtension.Select(query, (Func<T, T, T, Tuple<T, T>>)((d, min, max) => Tuple.Create(min, max)));

            IEnumerable<Tuple<T, T>> result = query;
            foreach (var v in expected.Zip(result, (l, r) => Tuple.Create(l, r))) ;
            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void MinMaxNullSelector()
        {
            var source = new int?[] { null, -2, 3, null, 5, -6, 7, null, 9 };
            var expected = new List<Tuple<T, T>>();
            for (int i = 1; i <= source.Length; ++i)
                expected.Add(Tuple.Create(CastSequence(source).Take(i).Min(), CastSequence(source).Take(i).Max()));

            dynamic query = CastSequence(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            query = WindowExtension.Min(query, (Func<T, T>)(x => x));
            query = WindowExtension.Max(query, (Func<T, T>)(x => x));
            query = WindowExtension.Select(query, (Func<T, T, T, Tuple<T, T>>)((d, min, max) => Tuple.Create(min, max)));

            IEnumerable<Tuple<T, T>> result = query;
            foreach (var v in expected.Zip(result, (l, r) => Tuple.Create(l, r))) ;
            Assert.That(result.SequenceEqual(expected));
        }
    }
}
