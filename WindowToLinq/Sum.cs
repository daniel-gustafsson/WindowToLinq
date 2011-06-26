using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    public static partial class WindowExtension
    {
        /// <summary>
        /// Computes the sum of a sequence of Int32 values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Int32 values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<int, Tuple<int, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<int, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<int, TSourceAggregates, int, int>(
                source
                , 0
                , (a, s) => a + s
                , (a, s) => a - s
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of Int32 values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Int32 values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<int, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, int> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, int, int>(
                source
                , 0
                , (a, s) => a + selector(s)
                , (a, s) => a - selector(s)
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Int32 values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Int32 values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<Nullable<int>, Tuple<Nullable<int>, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<int>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<int>, TSourceAggregates, Nullable<int>, Nullable<int>>(
                source
                , 0
                , (a, s) => a + s.GetValueOrDefault()
                , (a, s) => a - s.GetValueOrDefault()
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Int32 values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Int32 values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<int>, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<int>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Nullable<int>, Nullable<int>>(
                source
                , 0
                , (a, s) => a + selector(s).GetValueOrDefault()
                , (a, s) => a - selector(s).GetValueOrDefault()
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of Int64 values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Int64 values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<long, Tuple<long, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<long, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<long, TSourceAggregates, long, long>(
                source
                , 0L
                , (a, s) => a + s
                , (a, s) => a - s
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of Int64 values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Int64 values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<long, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, long> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, long, long>(
                source
                , 0L
                , (a, s) => a + selector(s)
                , (a, s) => a - selector(s)
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Int64 values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Int64 values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<Nullable<long>, Tuple<Nullable<long>, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<long>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<long>, TSourceAggregates, Nullable<long>, Nullable<long>>(
                source
                , 0L
                , (a, s) => a + s.GetValueOrDefault()
                , (a, s) => a - s.GetValueOrDefault()
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Int64 values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Int64 values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<long>, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<long>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Nullable<long>, Nullable<long>>(
                source
                , 0L
                , (a, s) => a + selector(s).GetValueOrDefault()
                , (a, s) => a - selector(s).GetValueOrDefault()
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of Single values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Single values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<float, Tuple<float, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<float, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<float, TSourceAggregates, float, float>(
                source
                , 0.0F
                , (a, s) => a + s
                , (a, s) => a - s
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of Single values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Single values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<float, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, float> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, float, float>(
                source
                , 0.0F
                , (a, s) => a + selector(s)
                , (a, s) => a - selector(s)
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Single values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Single values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<Nullable<float>, Tuple<Nullable<float>, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<float>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<float>, TSourceAggregates, Nullable<float>, Nullable<float>>(
                source
                , 0.0F
                , (a, s) => a + s.GetValueOrDefault()
                , (a, s) => a - s.GetValueOrDefault()
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Single values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Single values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<float>, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<float>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Nullable<float>, Nullable<float>>(
                source
                , 0.0F
                , (a, s) => a + selector(s).GetValueOrDefault()
                , (a, s) => a - selector(s).GetValueOrDefault()
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of Double values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Double values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<double, Tuple<double, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<double, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<double, TSourceAggregates, double, double>(
                source
                , 0.0
                , (a, s) => a + s
                , (a, s) => a - s
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of Double values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Double values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<double, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, double> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, double, double>(
                source
                , 0.0
                , (a, s) => a + selector(s)
                , (a, s) => a - selector(s)
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Double values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Double values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<Nullable<double>, Tuple<Nullable<double>, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<double>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<double>, TSourceAggregates, Nullable<double>, Nullable<double>>(
                source
                , 0.0
                , (a, s) => a + s.GetValueOrDefault()
                , (a, s) => a - s.GetValueOrDefault()
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Double values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Double values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<double>, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<double>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Nullable<double>, Nullable<double>>(
                source
                , 0.0
                , (a, s) => a + selector(s).GetValueOrDefault()
                , (a, s) => a - selector(s).GetValueOrDefault()
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of Decimal values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Decimal values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<decimal, Tuple<decimal, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<decimal, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<decimal, TSourceAggregates, decimal, decimal>(
                source
                , 0.0M
                , (a, s) => a + s
                , (a, s) => a - s
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of Decimal values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of Decimal values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<decimal, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, decimal> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, decimal, decimal>(
                source
                , 0.0M
                , (a, s) => a + selector(s)
                , (a, s) => a - selector(s)
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Decimal values. 
        /// </summary>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Decimal values to calculate the sum of.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<Nullable<decimal>, Tuple<Nullable<decimal>, TSourceAggregates>> Sum<TSourceAggregates>(
            this IWindowAggregateEnumerable<Nullable<decimal>, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<Nullable<decimal>, TSourceAggregates, Nullable<decimal>, Nullable<decimal>>(
                source
                , 0.0M
                , (a, s) => a + s.GetValueOrDefault()
                , (a, s) => a - s.GetValueOrDefault()
                , x => x);
        }

        /// <summary>
        /// Computes the sum of a sequence of nullable Decimal values. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A sequence of nullable Decimal values to calculate the sum of.</param>
        /// <param name="selector">A transform function to apply to each element</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<Nullable<decimal>, TSourceAggregates>> Sum<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, Nullable<decimal>> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Nullable<decimal>, Nullable<decimal>>(
                source
                , 0.0M
                , (a, s) => a + selector(s).GetValueOrDefault()
                , (a, s) => a - selector(s).GetValueOrDefault()
                , x => x);
        }
    }
}
