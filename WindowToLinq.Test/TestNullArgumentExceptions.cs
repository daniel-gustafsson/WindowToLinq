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
    public sealed class TestNullArgumentExceptions
    {
        public IEnumerable<object[]> Methods
        {
            get
            {
                return from mi in typeof(WindowExtension).GetMethods(BindingFlags.Public | BindingFlags.Static)
                       from p in Enumerable.Range(0, mi.GetParameters().Length)
                       where !mi.GetParameters()[p].ParameterType.IsGenericParameter
                       select new object[] { mi, p };
            }
        }

        object ConstructArgument(Type paramType, int aggregateCount)
        {
            if (paramType.Name == "IWindowAggregateEnumerable`2" || paramType.Name == "IWindowAggregateFunction`2")
            {
                dynamic query = new int[] { 1 }.WindowUnbounded();
                for (int i = 0; i < Math.Max(aggregateCount, 1); i++)
                    query = WindowExtension.Sum(query);
                return query;
            }
            else if (paramType.Name == "IEnumerable`1")
            {
                return new int[] { 1 };
            }
            else if (Regex.IsMatch(paramType.Name, "^Func`"))
            {
                Type[] genericArgs = paramType.GetGenericArguments();
                Expression exp = Expression.Constant(Activator.CreateInstance(genericArgs.Last()));
                var lambdaParams = genericArgs.Take(genericArgs.Length - 1).Select(arg => Expression.Parameter(arg));
                return Expression.Lambda(exp, lambdaParams).Compile();
            }
            else if (paramType.Name == "IEqualityComparer`1")
            {
                return EqualityComparer<int>.Default;
            }

            return null;
        }

        [Test, TestCaseSource("Methods")]
        public void Method(
            MethodInfo mi
            , int argToTest)
        {
            var genericArgsInfo = mi.GetGenericArguments();
            int aggregateCount = genericArgsInfo.Count(t => Regex.IsMatch(t.Name, "^TA"));
            if (genericArgsInfo.Length > 0)
                mi = mi.MakeGenericMethod(genericArgsInfo.Select(
                    p => p.Name == "TSourceAggregates" ? typeof(Tuple<int, int>) : typeof(int)).ToArray());

            var paramInfos = mi.GetParameters();
            var callArguments = paramInfos.Select((p, i) =>
                {
                    object arg = null;
                    if (p.ParameterType.IsValueType)
                        arg = Activator.CreateInstance(p.ParameterType);
                    else if (i < argToTest)
                        arg = ConstructArgument(p.ParameterType, aggregateCount);
                    return Expression.Constant(arg, p.ParameterType);
                });

            Action callWithBoundArguments = Expression.Lambda<Action>(Expression.Call(mi, callArguments)).Compile();
            Assert.That(new TestDelegate(callWithBoundArguments), Throws.TypeOf(typeof(ArgumentNullException))
                .With.Property("ParamName").EqualTo(paramInfos[argToTest].Name));
        }
    }
}
