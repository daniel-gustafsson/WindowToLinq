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
    public sealed class TestCompute<T>
    {
        Func<int?, T> cast = Lambda.Cast<int?, T>();

        IEnumerable<T> CastSource(IEnumerable<int?> source)
        {
            return source.Select(cast);
        }

        [Test]
        public void ComputeSelector(
            [Range(1, 10)] int count)
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            var expected = new List<int?>();
            dynamic query = CastSource(source);
            query = WindowExtension.BeginAggregate(query);
            for (int i = 0; i < count; ++i)
            {
                query = WindowExtension.Sum(query);
                expected.Add(15);
            }

            MethodInfo selectMethod = typeof(WindowExtension).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(
                    d => d.Name == "Compute"
                    && d.GetParameters().Last().ParameterType.Name == "Func`" + (count + 1))
                .Single();

            MethodInfo selectMethodBound = selectMethod.MakeGenericMethod(
                Enumerable.Repeat(typeof(T), count + 2).Select((t, i) => i == 1 ? t.MakeArrayType() : t).ToArray());

            var selectorParams = Enumerable.Range(1, count).Select(p => Expression.Parameter(typeof(T), "param" + p)).ToList();
            Delegate selector = Expression.Lambda(Expression.NewArrayInit(typeof(T), selectorParams), selectorParams).Compile();

            var result = (T[])selectMethodBound.Invoke(null, new object[] { query, selector });

            Assert.That(result.SequenceEqual(CastSource(expected)));
        }

        [Test]
        public void ComputeTuple(
            [Range(1, 10)] int count)
        {
            var source = new int?[] { 1, 2, 3, 4, 5 };
            dynamic query = CastSource(source);
            query = WindowExtension.BeginAggregate(query);
            for (int i = 0; i < count; ++i)
                query = WindowExtension.Sum(query);

            dynamic result = WindowExtension.Compute(query);

            if (count == 1)
                Assert.That(result == cast(15));
            else
                Assert.That(result.Item1 == cast(15));

            if (count >= 2) Assert.That(result.Item2 == cast(15));
            if (count >= 3) Assert.That(result.Item3 == cast(15));
            if (count >= 4) Assert.That(result.Item4 == cast(15));
            if (count >= 5) Assert.That(result.Item5 == cast(15));
            if (count >= 6) Assert.That(result.Item6 == cast(15));
            if (count >= 7) Assert.That(result.Item7 == cast(15));
            if (count >= 8) Assert.That(result.Rest.Item1 == cast(15));
            if (count >= 9) Assert.That(result.Rest.Item2 == cast(15));
            if (count >= 10) Assert.That(result.Rest.Item3 == cast(15));
        }
    }
}
