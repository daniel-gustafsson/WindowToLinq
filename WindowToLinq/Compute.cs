using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    public static partial class WindowExtension
    {
        /// <summary>
        /// This is an alias for a window on an input sequence that is optimized for multi aggregate calculation.
        /// The window has no preceding bound and ends at current row. That way multiple aggregates can be computed
        /// for all the elements of the input sequence without any buffering, and by fetching just the last value
        /// you simply get all aggregates from a single pass through the sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> BeginAggregate<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregator<TSource, bool>(source, x => true, EqualityComparer<bool>.Default, null, (c, w, i) => i <= 0);
        }

        /// <summary>
        /// A special version of LastOrDefault that returns the last value in a aggregated window sequence.
        /// If the sequence is empty, a tuple is returned with the source value as default, and all
        /// aggregates set to the results based on their seeds.
        /// Instead of calling this function directly, it is recommended ot use one of the Compute overloads instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">The source window sequence.</param>
        /// <returns>Returns a recursive tuple containing the default source and aggregates values.</returns>
        public static Tuple<TSource, TSourceAggregates> LastOrDefault<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            Tuple<TSource, TSourceAggregates> value = source.GetDefault();
            foreach (var tmp in source)
                value = tmp;
            return value;
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA1, int>> source
            , Func<TA1, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static TA1 Compute<TSource, TA1>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA1, int>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return value.Item2.Item1;
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1, TA2>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA2, Tuple<TA1, int>>> source
            , Func<TA1, TA2, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(value.Item2.Item2.Item1, value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static Tuple<TA1, TA2> Compute<TSource, TA1, TA2>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA2, Tuple<TA1, int>>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return Tuple.Create(value.Item2.Item2.Item1, value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1, TA2, TA3>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>> source
            , Func<TA1, TA2, TA3, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(value.Item2.Item2.Item2.Item1, value.Item2.Item2.Item1, value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static Tuple<TA1, TA2, TA3> Compute<TSource, TA1, TA2, TA3>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return Tuple.Create(value.Item2.Item2.Item2.Item1, value.Item2.Item2.Item1, value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1, TA2, TA3, TA4>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>> source
            , Func<TA1, TA2, TA3, TA4, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(value.Item2.Item2.Item2.Item2.Item1, value.Item2.Item2.Item2.Item1, value.Item2.Item2.Item1, value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static Tuple<TA1, TA2, TA3, TA4> Compute<TSource, TA1, TA2, TA3, TA4>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return Tuple.Create(value.Item2.Item2.Item2.Item2.Item1, value.Item2.Item2.Item2.Item1, value.Item2.Item2.Item1, value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1, TA2, TA3, TA4, TA5>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>> source
            , Func<TA1, TA2, TA3, TA4, TA5, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(
                value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static Tuple<TA1, TA2, TA3, TA4, TA5> Compute<TSource, TA1, TA2, TA3, TA4, TA5>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return Tuple.Create(
                value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>> source
            , Func<TA1, TA2, TA3, TA4, TA5, TA6, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static Tuple<TA1, TA2, TA3, TA4, TA5, TA6> Compute<TSource, TA1, TA2, TA3, TA4, TA5, TA6>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return Tuple.Create(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <typeparam name="TA7">The type of the value of the seventh aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>> source
            , Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <typeparam name="TA7">The type of the value of the seventh aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static Tuple<TA1, TA2, TA3, TA4, TA5, TA6, TA7> Compute<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return Tuple.Create(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <typeparam name="TA7">The type of the value of the seventh aggregate.</typeparam>
        /// <typeparam name="TA8">The type of the value of the eighth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>> source
            , Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <typeparam name="TA7">The type of the value of the seventh aggregate.</typeparam>
        /// <typeparam name="TA8">The type of the value of the eighth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static Tuple<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Tuple<TA8>> Compute<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return Tuple.Create(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <typeparam name="TA7">The type of the value of the seventh aggregate.</typeparam>
        /// <typeparam name="TA8">The type of the value of the eighth aggregate.</typeparam>
        /// <typeparam name="TA9">The type of the value of the ninth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA9, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>>> source
            , Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <typeparam name="TA7">The type of the value of the seventh aggregate.</typeparam>
        /// <typeparam name="TA8">The type of the value of the eighth aggregate.</typeparam>
        /// <typeparam name="TA9">The type of the value of the ninth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static Tuple<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Tuple<TA8, TA9>> Compute<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA9, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return new Tuple<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Tuple<TA8, TA9>>(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , Tuple.Create(
                    value.Item2.Item2.Item1
                    , value.Item2.Item1));
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// The selector function transforms the aggregates to a result type.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <typeparam name="TA7">The type of the value of the seventh aggregate.</typeparam>
        /// <typeparam name="TA8">The type of the value of the eighth aggregate.</typeparam>
        /// <typeparam name="TA9">The type of the value of the ninth aggregate.</typeparam>
        /// <typeparam name="TA10">The type of the value of the tenth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <param name="selector">A transform function to apply to the calculated aggregates.</param>
        /// <returns>The transformed value</returns>
        public static TResult Compute<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA10, Tuple<TA9, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>>>> source
            , Func<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            var value = source.LastOrDefault();
            return selector(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item1
                , value.Item2.Item1);
        }

        /// <summary>
        /// Computes all the previously defined aggregates by consuming the input sequence in and calculating all the aggregates in a single pass.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <typeparam name="TA7">The type of the value of the seventh aggregate.</typeparam>
        /// <typeparam name="TA8">The type of the value of the eighth aggregate.</typeparam>
        /// <typeparam name="TA9">The type of the value of the ninth aggregate.</typeparam>
        /// <typeparam name="TA10">The type of the value of the tenth aggregate.</typeparam>
        /// <param name="source">An aggregate window to compute previously specified aggregates on.</param>
        /// <returns>The first aggregate value</returns>
        public static Tuple<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Tuple<TA8, TA9, TA10>> Compute<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA10, Tuple<TA9, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>>>> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            var value = source.LastOrDefault();
            return new Tuple<TA1, TA2, TA3, TA4, TA5, TA6, TA7, Tuple<TA8, TA9, TA10>>(
                value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item2.Item1
                , value.Item2.Item2.Item2.Item2.Item1
                , Tuple.Create(
                    value.Item2.Item2.Item2.Item1
                    , value.Item2.Item2.Item1
                    , value.Item2.Item1));
        }
    }
}
