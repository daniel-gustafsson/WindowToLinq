using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace WindowToLinq.Test
{
    public static class Lambda
    {
        public static Func<TSource, TResult> Cast<TSource, TResult>()
        {
            Type inputType = typeof(TSource);
            Type resultType = typeof(TResult);
            ParameterExpression inputParam = Expression.Parameter(inputType, "input");
            Expression inputExp = inputParam;
            Expression cast = null;
            if (resultType.Name == "String")
            {
                Expression toString = Expression.Call(inputExp, inputType.GetMethod("ToString", Type.EmptyTypes));
                if (inputType.Name == "Nullable`1")
                {
                    cast = Expression.Condition(
                        Expression.Equal(inputExp, Expression.Constant(null, inputType))
                        , Expression.Constant(null, resultType)
                        , toString);
                }
                else
                {
                    cast = toString;
                }
            }
            else
            {
                if (inputType.Name == "Nullable`1" && resultType.Name != "Nullable`1")
                {
                    MethodInfo getValueOrDefault = inputType.GetMethod("GetValueOrDefault", Type.EmptyTypes);
                    inputExp = Expression.Call(inputExp, getValueOrDefault);
                }
                cast = Expression.Convert(inputExp, resultType);
            }
            return Expression.Lambda<Func<TSource, TResult>>(cast, inputParam).Compile();
        }

        public static Func<TElement, bool> LessThan<TElement>(TElement value)
        {
            return LessThan<TElement, TElement>(value, "");
        }
        public static Func<TSource, bool> LessThan<TSource, TElement>(TElement value, string propertyName)
        {
            ParameterExpression sourceParam = Expression.Parameter(typeof(TSource), "source");
            ConstantExpression valueExp = Expression.Constant(value, typeof(TElement));
            Expression exp = Expression.LessThan(Property(sourceParam, propertyName), valueExp);
            return Expression.Lambda<Func<TSource, bool>>(exp, sourceParam).Compile();
        }

        public static Func<TElement, bool> GreaterThan<TElement>(TElement value)
        {
            return GreaterThan<TElement, TElement>(value, "");
        }
        public static Func<TSource, bool> GreaterThan<TSource, TElement>(TElement value, string propertyName)
        {
            ParameterExpression sourceParam = Expression.Parameter(typeof(TSource), "source");
            ConstantExpression valueExp = Expression.Constant(value, typeof(TElement));
            Expression exp = Expression.GreaterThan(Property(sourceParam, propertyName), valueExp);
            return Expression.Lambda<Func<TSource, bool>>(exp, sourceParam).Compile();
        }

        public static Func<TElement, bool> Between<TElement>(TElement min, TElement max)
        {
            return Between<TElement, TElement>(min, max, "");
        }
        public static Func<TSource, bool> Between<TSource, TElement>(TElement min, TElement max, string propertyName)
        {
            ParameterExpression sourceParam = Expression.Parameter(typeof(TSource), "source");
            ConstantExpression minExp = Expression.Constant(min, typeof(TElement));
            ConstantExpression maxExp = Expression.Constant(max, typeof(TElement));
            Expression sourceExp = Property(sourceParam, propertyName);
            Expression exp = Expression.And(
                Expression.GreaterThanOrEqual(sourceExp, minExp)
                , Expression.LessThanOrEqual(sourceExp, maxExp));
            return Expression.Lambda<Func<TSource, bool>>(exp, sourceParam).Compile();
        }

        public static Delegate Select<T>(int count)
        {
            var param = Enumerable.Range(1, count + 1).Select(p => Expression.Parameter(typeof(T), "param" + p)).ToList();
            return Expression.Lambda(Expression.NewArrayInit(typeof(T), param.Skip(1)), param).Compile();
        }

        static Expression Property(Expression exp, string propertyName)
        {
            if (propertyName == "")
                return exp;
            else
                return Expression.Property(exp, propertyName);
        }
    }

    public struct TestValueType : IComparable<TestValueType>
    {
        private int value;
        public int Value { get { return value; } }

        public TestValueType(int value)
        {
            this.value = value;
        }

        public static implicit operator TestValueType(int value)
        {
            return new TestValueType(value);
        }

        public int CompareTo(TestValueType other)
        {
            if (this.Value < other.Value)
                return -1;
            else if (this.Value > other.Value)
                return 1;
            else
                return 0;
        }
    }

    public sealed class SequenceComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(IEnumerable<T> x)
        {
            return x.GetHashCode();
        }
    }
}
