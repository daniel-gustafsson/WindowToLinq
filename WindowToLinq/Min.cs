﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    public static partial class WindowExtension
    {
        /// <summary>
        /// Returns the minimum value in a sequence of Int32 values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<int, Tuple<int, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<int, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum Int32 value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<int, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, int> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable Int32 values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<int>, Tuple<Nullable<int>, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<int>, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable Int32 value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<int>, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<int>> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a sequence of Int64 values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<long, Tuple<long, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<long, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum Int64 value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<long, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, long> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable Int64 values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<long>, Tuple<Nullable<long>, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<long>, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable Int64 value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<long>, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<long>> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a sequence of Single values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<float, Tuple<float, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<float, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum Single value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<float, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, float> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable Single values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<float>, Tuple<Nullable<float>, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<float>, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable Single value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<float>, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<float>> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a sequence of Double values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<double, Tuple<double, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<double, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum Double value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<double, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, double> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable Double values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<double>, Tuple<Nullable<double>, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<double>, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable Double value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<double>, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<double>> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a sequence of Decimal values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<decimal, Tuple<decimal, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<decimal, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum Decimal value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<decimal, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, decimal> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a sequence of nullable Decimal values.
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<decimal>, Tuple<Nullable<decimal>, TSourceAggregates>> Min<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<decimal>, TSourceAggregates> source)
        {
            return MinImpl(source);
        }

        /// <summary>
        /// Invokes a transform function on each element of a sequence and returns the minimum nullable Decimal value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<decimal>, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<decimal>> selector)
        {
            return MinImpl(source, selector);
        }

        /// <summary>
        /// Returns the minimum value in a generic sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<TSource, TSourceAggregates>> Min<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            IComparer<TSource> cmp = Comparer<TSource>.Default;
            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<TSource, bool>, TSource>(
                source
                , Tuple.Create(default(TSource), false) // source, initialized
                , (a, s) => Tuple.Create(!a.Item2 || a.Item1 == null || s != null && cmp.Compare(a.Item1, s) > 0 ? s : a.Item1, true)
                , (a, s) => { throw new InvalidOperationException("Bounded precedence window cannot be used with Min aggregate"); }
                , x => x.Item1);
        }

        /// <summary>
        /// Invokes a transform function on each element of a generic sequence and returns the minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<TResult, TSourceAggregates>> Min<TSource, TResult, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            IComparer<TResult> cmp = Comparer<TResult>.Default;
            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<TResult, bool>, TResult>(
                source
                , Tuple.Create(default(TResult), false) // source, initialized
                , (a, s) => Tuple.Create(!a.Item2 || a.Item1 == null || s != null && cmp.Compare(a.Item1, selector(s)) > 0 ? selector(s) : a.Item1, true)
                , (a, s) => { throw new InvalidOperationException("Bounded precedence window cannot be used with Min aggregate"); }
                , x => x.Item1);
        }

        static IWindowAggregateFunction<TSource, Tuple<TSource, TSourceAggregates>> MinImpl<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source)
            where TSource : struct, IComparable<TSource>
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Nullable<TSource>, TSource>(
                source
                , default(Nullable<TSource>)
                , (a, s) => !a.HasValue || a.Value.CompareTo(s) > 0 ? s : a
                , (a, s) => { throw new InvalidOperationException("Bounded precedence window cannot be used with Min aggregate"); }
                , x => x.GetValueOrDefault());
        }

        static IWindowAggregateFunction<TSource, Tuple<TResult, TSourceAggregates>> MinImpl<TSource, TResult, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, TResult> selector)
            where TResult : struct, IComparable<TResult>
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Nullable<TResult>, TResult>(
                source
                , default(Nullable<TResult>)
                , (a, s) => !a.HasValue || a.Value.CompareTo(selector(s)) > 0 ? selector(s) : a
                , (a, s) => { throw new InvalidOperationException("Bounded precedence window cannot be used with Min aggregate"); }
                , x => x.GetValueOrDefault());
        }

        static IWindowAggregateFunction<Nullable<TSource>, Tuple<Nullable<TSource>, TSourceAggregates>> MinImpl<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<TSource>, TSourceAggregates> source)
            where TSource : struct, IComparable<TSource>
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<TSource>, TSourceAggregates, Nullable<TSource>, Nullable<TSource>>(
                source
                , default(Nullable<TSource>)
                , (a, s) => !a.HasValue || s.HasValue && a.Value.CompareTo(s.Value) > 0 ? s : a
                , (a, s) => { throw new InvalidOperationException("Bounded precedence window cannot be used with Min aggregate"); }
                , x => x);
        }

        static IWindowAggregateFunction<TSource, Tuple<Nullable<TResult>, TSourceAggregates>> MinImpl<TSource, TResult, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<TResult>> selector)
            where TResult : struct, IComparable<TResult>
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Nullable<TResult>, Nullable<TResult>>(
                source
                , default(Nullable<TResult>)
                , (a, s) => !a.HasValue || selector(s).HasValue && a.Value.CompareTo(selector(s).Value) > 0 ? selector(s) : a
                , (a, s) => { throw new InvalidOperationException("Bounded precedence window cannot be used with Min aggregate"); }
                , x => x);
        }
    }
}
