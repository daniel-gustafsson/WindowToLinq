using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    public static partial class WindowExtension
    {
        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence that contains elements to be counted.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<int, TSourceAggregates>> Count<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<TSource, TSourceAggregates, int, int>(
                source
                , 0
                , (a, s) => a + 1
                , (a, s) => a - 1
                , x => x);
        }

        /// <summary>
        /// Returns a number that represents how many elements in the specified sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence that contains elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<int, TSourceAggregates>> Count<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return new WindowAggregateFunction<TSource, TSourceAggregates, int, int>(
                source
                , 0
                , (a, s) => a + (predicate(s) ? 1 : 0)
                , (a, s) => a - (predicate(s) ? 1 : 0)
                , x => x);
        }

        /// <summary>
        /// Returns an Int64 that represents the total number of elements in a sequence. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence that contains elements to be counted.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<long, TSourceAggregates>> LongCount<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregateFunction<TSource, TSourceAggregates, long, long>(
                source
                , 0
                , (a, s) => a + 1
                , (a, s) => a - 1
                , x => x);
        }

        /// <summary>
        /// Returns an Int64 that represents how many elements in a sequence satisfy a condition. 
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate sequence that contains elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The source sequence and any aggregates so far.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<long, TSourceAggregates>> LongCount<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return new WindowAggregateFunction<TSource, TSourceAggregates, long, long>(
                source
                , 0
                , (a, s) => a + (predicate(s) ? 1 : 0)
                , (a, s) => a - (predicate(s) ? 1 : 0)
                , x => x);
        }
    }
}
