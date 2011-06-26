using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    public static partial class WindowExtension
    {
        /// <summary>
        /// Filters the latest specified aggregate on an open window.
        /// Multiple filters can be added after each other.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate function to filter.</param>
        /// <param name="predicate">The filter function taking the source element and returning true if element is part of the aggregate.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates filters and this filter.</returns>
        public static IWindowAggregateFunction<TSource, TSourceAggregates> Where<TSource, TSourceAggregates>(
            this IWindowAggregateFunction<TSource, TSourceAggregates> source
            , Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return new WindowAggregateFilter<TSource, TSourceAggregates>(source, (d, i) => predicate(d));
        }

        /// <summary>
        /// Filters the latest specified aggregate on an open window.
        /// Multiple filters can be added after each other.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">A window aggregate function to filter.</param>
        /// <param name="predicate">The filter function taking the source element, an index into the window and returning true if element is part of the aggregate.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates filters and this filter.</returns>
        public static IWindowAggregateFunction<TSource, TSourceAggregates> Where<TSource, TSourceAggregates>(
            this IWindowAggregateFunction<TSource, TSourceAggregates> source
            , Func<TSource, int, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            return new WindowAggregateFilter<TSource, TSourceAggregates>(source, predicate);
        }


        sealed class WindowAggregateFilter<TSource, TSourceAggregates> : IWindowAggregateFunction<TSource, TSourceAggregates>
        {
            readonly IWindowAggregateEnumerable<TSource, TSourceAggregates> source;
            readonly Func<TSource, int, bool> predicate;

            public WindowAggregateFilter(
                IWindowAggregateEnumerable<TSource, TSourceAggregates> source
                , Func<TSource, int, bool> predicate)
            {
                this.source = source;
                this.predicate = predicate;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public IEnumerator<Tuple<TSource, TSourceAggregates>> GetEnumerator()
            {
                return GetEnumerator((d, i) => { }, (d, i) => { }, () => { }, (d, i) => true);
            }


            public IEnumerator<Tuple<TSource, TSourceAggregates>> GetEnumerator(
                Action<TSource, int> accumulate, Action<TSource, int> decumulate, Action reset, Func<TSource, int, bool> predicate)
            {
                using (IEnumerator<Tuple<TSource, TSourceAggregates>> iSource = this.source.GetEnumerator(
                    (d, i) => { accumulate(d, i); } // Pass through
                    , (d, i) => { decumulate(d, i); } // Pass through
                    , () => { reset(); } // Pass through
                    , (d, i) => this.predicate(d, i) && predicate(d, i))) // Test this and any later predicates
                {
                    while (iSource.MoveNext())
                    {
                        yield return iSource.Current;
                    }
                }
            }

            public Tuple<TSource, TSourceAggregates> GetDefault()
            {
                return this.source.GetDefault();
            }
        }
    }
}
