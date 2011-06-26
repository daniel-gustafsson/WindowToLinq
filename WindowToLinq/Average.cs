using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    public static partial class WindowExtension
    {
        /// <summary>
        /// Computes the average of a sequence of Int32 values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of Int32 values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<int, Tuple<double, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<int, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<int, TSourceAggregates, Tuple<long, long>, double>(
                source
                , Tuple.Create(0L, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + s, a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - s, a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0 : (double)r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of Int32 values that are obtained by invoking a transform function on each element of the input sequence. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<double, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, int> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<long, long>, double>(
                source
                , Tuple.Create(0L, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + selector(s), a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - selector(s), a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0 : (double)r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Int32 values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of nullable Int32 values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<int>, Tuple<Nullable<double>, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<int>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<int>, TSourceAggregates, Tuple<long, long>, Nullable<double>>(
                source
                , Tuple.Create(0L, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + s.GetValueOrDefault(), a.Item2 + (s.HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - s.GetValueOrDefault(), a.Item2 - (s.HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<double>) : (double)r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Int32 values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<double>, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<int>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<long, long>, Nullable<double>>(
                source
                , Tuple.Create(0L, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + selector(s).GetValueOrDefault(), a.Item2 + (selector(s).HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - selector(s).GetValueOrDefault(), a.Item2 - (selector(s).HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<double>) : (double)r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of Int64 values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of Int64 values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<long, Tuple<double, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<long, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<long, TSourceAggregates, Tuple<long, long>, double>(
                source
                , Tuple.Create(0L, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + s, a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - s, a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0 : (double)r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of Int64 values that are obtained by invoking a transform function on each element of the input sequence. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<double, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, long> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<long, long>, double>(
                source
                , Tuple.Create(0L, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + selector(s), a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - selector(s), a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0 : (double)r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Int64 values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of nullable Int64 values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<long>, Tuple<Nullable<double>, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<long>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<long>, TSourceAggregates, Tuple<long, long>, Nullable<double>>(
                source
                , Tuple.Create(0L, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + s.GetValueOrDefault(), a.Item2 + (s.HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - s.GetValueOrDefault(), a.Item2 - (s.HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<double>) : (double)r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Int64 values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<double>, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<long>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<long, long>, Nullable<double>>(
                source
                , Tuple.Create(0L, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + selector(s).GetValueOrDefault(), a.Item2 + (selector(s).HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - selector(s).GetValueOrDefault(), a.Item2 - (selector(s).HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<double>) : (double)r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of Single values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of Single values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<float, Tuple<float, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<float, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<float, TSourceAggregates, Tuple<float, long>, float>(
                source
                , Tuple.Create(0.0F, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + s, a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - s, a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0F : r.Item1 / (float)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of Single values that are obtained by invoking a transform function on each element of the input sequence. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<float, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, float> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<float, long>, float>(
                source
                , Tuple.Create(0.0F, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + selector(s), a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - selector(s), a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0F : r.Item1 / (float)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Single values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of nullable Single values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<float>, Tuple<Nullable<float>, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<float>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<float>, TSourceAggregates, Tuple<float, long>, Nullable<float>>(
                source
                , Tuple.Create(0.0F, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + s.GetValueOrDefault(), a.Item2 + (s.HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - s.GetValueOrDefault(), a.Item2 - (s.HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<float>) : r.Item1 / (float)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Single values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<float>, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<float>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<float, long>, Nullable<float>>(
                source
                , Tuple.Create(0.0F, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + selector(s).GetValueOrDefault(), a.Item2 + (selector(s).HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - selector(s).GetValueOrDefault(), a.Item2 - (selector(s).HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<float>) : r.Item1 / (float)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of Double values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of Double values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<double, Tuple<double, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<double, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<double, TSourceAggregates, Tuple<double, long>, double>(
                source
                , Tuple.Create(0.0, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + s, a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - s, a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0 : r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of Double values that are obtained by invoking a transform function on each element of the input sequence. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<double, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, double> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<double, long>, double>(
                source
                , Tuple.Create(0.0, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + selector(s), a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - selector(s), a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0 : r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Double values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of nullable Double values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<double>, Tuple<Nullable<double>, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<double>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<double>, TSourceAggregates, Tuple<double, long>, Nullable<double>>(
                source
                , Tuple.Create(0.0, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + s.GetValueOrDefault(), a.Item2 + (s.HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - s.GetValueOrDefault(), a.Item2 - (s.HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<double>) : r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Double values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<double>, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<double>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<double, long>, Nullable<double>>(
                source
                , Tuple.Create(0.0, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + selector(s).GetValueOrDefault(), a.Item2 + (selector(s).HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - selector(s).GetValueOrDefault(), a.Item2 - (selector(s).HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<double>) : r.Item1 / (double)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of Decimal values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of Decimal values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<decimal, Tuple<decimal, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<decimal, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<decimal, TSourceAggregates, Tuple<decimal, long>, decimal>(
                source
                , Tuple.Create(0.0M, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + s, a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - s, a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0M : r.Item1 / (decimal)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of Decimal values that are obtained by invoking a transform function on each element of the input sequence. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<decimal, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<decimal, long>, decimal>(
                source
                , Tuple.Create(0.0M, 0L) // Sum, Count
                , (a, s) => Tuple.Create(a.Item1 + selector(s), a.Item2 + 1)
                , (a, s) => Tuple.Create(a.Item1 - selector(s), a.Item2 - 1)
                , r => (r.Item2 <= 0) ? 0.0M : r.Item1 / (decimal)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Decimal values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of nullable Decimal values to calculate the average of.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<Nullable<decimal>, Tuple<Nullable<decimal>, TSourceAggregates>> Average<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<decimal>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<decimal>, TSourceAggregates, Tuple<decimal, long>, Nullable<decimal>>(
                source
                , Tuple.Create(0.0M, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + s.GetValueOrDefault(), a.Item2 + (s.HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - s.GetValueOrDefault(), a.Item2 - (s.HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<decimal>) : r.Item1 / (decimal)r.Item2);
        }

        /// <summary>
        /// Computes the average of a sequence of nullable Decimal values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence of values to calculate the average of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<decimal>, TSourceAggregates>> Average<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<decimal>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<decimal, long>, Nullable<decimal>>(
                source
                , Tuple.Create(0.0M, 0L) // Sum, CountNonNull
                , (a, s) => Tuple.Create(a.Item1 + selector(s).GetValueOrDefault(), a.Item2 + (selector(s).HasValue ? 1 : 0))
                , (a, s) => Tuple.Create(a.Item1 - selector(s).GetValueOrDefault(), a.Item2 - (selector(s).HasValue ? 1 : 0))
                , r => (r.Item2 <= 0) ? default(Nullable<decimal>) : r.Item1 / (decimal)r.Item2);
        }
    }
}
