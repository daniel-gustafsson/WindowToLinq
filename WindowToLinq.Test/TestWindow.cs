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
    public sealed class TestWindow
    {
        public TestWindow() { }
        bool debug = false;
        public TestWindow(bool debug)
        {
            this.debug = debug;
        }

        [Test]
        public void Window(
            [Random(Int32.MinValue, Int32.MaxValue, 3)] int seed
            , [Range(2, 50, 5)] int range)
        {
            int sourceLength = 100;
            Random rnd = new Random(seed);
            var source = Enumerable.Range(0, sourceLength).Select(i =>
            {
                int start = i + 1 - rnd.Next(range);
                return Tuple.Create(i
                    , Math.Min(Math.Max(start, 0), sourceLength - 1)
                    , Math.Min(Math.Max(start + rnd.Next((int)((double)range * 0.65)), 0), sourceLength));
            }).ToList();

            for (int i = 1; i < sourceLength; i++)
            {
                source[i] = Tuple.Create(
                    source[i].Item1
                    , Math.Max(source[i - 1].Item2, source[i].Item2)
                    , Math.Max(source[i - 1].Item3, source[i].Item3));
            }

            var expected = source.Select(r => Tuple.Create(
                Enumerable.Range(r.Item2, r.Item3 - r.Item2).Count()
                , Enumerable.Range(r.Item2, r.Item3 - r.Item2).Sum()));

            var result = source
                .Window((c, w, i) => c.Item1 + i >= c.Item2
                    , (c, w, i) => c.Item1 + i < c.Item3
                    , (c, w) => Tuple.Create(w.Count(), w.Sum(x => x.Item1)));

            if (debug)
            {
                foreach (var v in source.Zip(expected, (l, r) => new { l, r }).Zip(result, (l, r) => new { s = l.l, e = l.r, r = r }))
                    Console.WriteLine(v);
            }

            Assert.That(result.SequenceEqual(expected));
        }

        [Test]
        public void AggregateWindow(
            [Random(Int32.MinValue, Int32.MaxValue, 3)] int seed
            , [Range(2, 50, 5)] int range)
        {
            int sourceLength = 100;
            Random rnd = new Random(seed);
            var source = Enumerable.Range(0, sourceLength).Select(i =>
            {
                int start = i + 1 - rnd.Next(range);
                return Tuple.Create(i
                    , Math.Min(Math.Max(start, 0), sourceLength - 1)
                    , Math.Min(Math.Max(start + rnd.Next((int)((double)range * 0.65)), 0), sourceLength));
            }).ToList();

            for (int i = 1; i < sourceLength; i++)
            {
                source[i] = Tuple.Create(
                    source[i].Item1
                    , Math.Max(source[i - 1].Item2, source[i].Item2)
                    , Math.Max(source[i - 1].Item3, source[i].Item3));
            }

            var expected = source.Select(r => Tuple.Create(
                Enumerable.Range(r.Item2, r.Item3 - r.Item2).Count()
                , Enumerable.Range(r.Item2, r.Item3 - r.Item2).Sum()
                , r.Item2 == r.Item3 ? 0 : (double)(Enumerable.Range(r.Item2, r.Item3 - r.Item2).Sum()) / (double)Enumerable.Range(r.Item2, r.Item3 - r.Item2).Count()));


            var result = source
                .Window((c, w, i) => c.Item1 + i >= c.Item2
                    , (c, w, i) => c.Item1 + i < c.Item3)
                .Count()
                .Sum(s => s.Item1)
                .Average(s => s.Item1)
                .Select((src, cnt, sum, avg) => Tuple.Create(cnt, sum, avg));

            if (debug)
            {
                foreach (var v in source.Zip(expected, (l, r) => new { l, r }).Zip(result, (l, r) => new { s = l.l, e = l.r, r = r }))
                    Console.WriteLine(v);
            }

            Assert.That(result.SequenceEqual(expected));
        }

        public IEnumerable<object> WindowMethods
        {
            get
            {
                return from mi in typeof(WindowExtension).GetMethods(BindingFlags.Public | BindingFlags.Static)
                       where Regex.IsMatch(mi.Name, "^Window")
                       select mi.MakeGenericMethod(mi.GetGenericArguments().Select(p => typeof(int)).ToArray());
            }
        }

        object ConstructArgument(ParameterInfo paramInfo, int[] source)
        {
            object arg = null;
            switch (paramInfo.Name)
            {
                case "source":
                    arg = source;
                    break;

                case "partitionSelector":
                    arg = (Func<int, int>)(x => x);
                    break;

                case "partitionComparer":
                    arg = EqualityComparer<int>.Default;
                    break;

                case "precedingBound":
                case "followingBound":
                    if (paramInfo.ParameterType.Name == "Func`2")
                        arg = (Func<int, bool>)(i => true);
                    else
                        arg = (Func<int, int, int, bool>)((c, w, i) => true);
                    break;

                case "selector":
                    if (paramInfo.ParameterType.Name == "Func`3")
                        arg = (Func<int, IEnumerable<int>, int>)((s, w) => w.Sum());
                    else
                        arg = (Func<int, int, IEnumerable<int>, int>)((s, i, w) => w.Sum());
                    break;
            }
            return arg;
        }

        [Test, TestCaseSource("WindowMethods")]
        public void WindowMethod(MethodInfo mi)
        {
            int[] source = { 1, 1, 1, 2, 3, 3 };
            int[] expected = { 11, 11, 11, 11, 11, 11 };

            var paramInfos = mi.GetParameters();
            if (paramInfos.Where(p => p.Name == "partitionSelector").Count() > 0)
                expected = new int[] { 3, 3, 3, 2, 6, 6 };

            var callArguments = paramInfos.Select(p => ConstructArgument(p, source)).ToArray();

            dynamic query = mi.Invoke(null, callArguments);

            if (paramInfos.Where(p => p.Name == "selector").Count() == 0)
            {
                query = WindowExtension.Sum(query);
                query = WindowExtension.Select(query, (Func<int, int, int>)((src, sum) => sum));
            }

            IEnumerable<int> result = query;
            Assert.That(result.SequenceEqual(expected));
        }
    }
}
