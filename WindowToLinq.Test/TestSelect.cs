using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    [TestFixture(typeof(double))]
    [TestFixture(typeof(double?))]
    public sealed class TestSelect<T>
    {
        Func<int?, T> cast = Lambda.Cast<int?, T>();

        IEnumerable<T> CastSource(IEnumerable<int?> source)
        {
            return source.Select(cast);
        }

        IEnumerable<T[]> Result(dynamic selectResult)
        {
            foreach (var r in selectResult)
                yield return r;
        }

        IEnumerable<T[]> Expected(IEnumerable<T> expected, int count)
        {
            foreach (var e in expected)
                yield return Enumerable.Repeat(e, count).ToArray();
        }

        [Test]
        public void Select(
            [Range(1, 10)] int count)
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new int?[] { 1, 3, 6, 10, 15 };
            dynamic query = CastSource(source);
            query = WindowExtension.WindowUnboundedPreceding(query, (Func<int, bool>)(i => i <= 0));
            for (int i = 0; i < count; ++i)
                query = WindowExtension.Sum(query);

            MethodInfo selectMethod = typeof(WindowExtension).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(
                    d => d.Name == "Select"
                    && d.GetParameters().Last().ParameterType.Name == "Func`" + (count + 2))
                .Single();

            MethodInfo selectMethodBound = selectMethod.MakeGenericMethod(
                Enumerable.Repeat(typeof(T), count + 2).Select((t, i) => i == 1 ? t.MakeArrayType() : t).ToArray());

            var selectorParams = Enumerable.Range(1, count + 1).Select(p => Expression.Parameter(typeof(T), "param" + p)).ToList();
            Delegate selector = Expression.Lambda(Expression.NewArrayInit(typeof(T), selectorParams.Skip(1)), selectorParams).Compile();

            var result = Result(selectMethodBound.Invoke(null, new object[] { query, selector }));

            Assert.That(result.SequenceEqual(Expected(CastSource(expected), count), new SequenceComparer<T>()));
        }
    }
}
