using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    public static partial class WindowExtension
    {
        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA1, int>> source
            , Func<TSource, TA1, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA1, int>> source
            , Func<TSource, TA1, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(i.Item1, i.Item2.Item1);
        }

        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1, TA2>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA2, Tuple<TA1, int>>> source
            , Func<TSource, TA1, TA2, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1, TA2>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA2, Tuple<TA1, int>>> source
            , Func<TSource, TA1, TA2, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(i.Item1, i.Item2.Item2.Item1, i.Item2.Item1);
        }

        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1, TA2, TA3>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>> source
            , Func<TSource, TA1, TA2, TA3, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1, TA2, TA3>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>> source
            , Func<TSource, TA1, TA2, TA3, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(i.Item1, i.Item2.Item2.Item2.Item1, i.Item2.Item2.Item1, i.Item2.Item1);
        }

        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1, TA2, TA3, TA4>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1, TA2, TA3, TA4>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(i.Item1, i.Item2.Item2.Item2.Item2.Item1, i.Item2.Item2.Item2.Item1, i.Item2.Item2.Item1, i.Item2.Item1);
        }

        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1, TA2, TA3, TA4, TA5>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1, TA2, TA3, TA4, TA5>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(
                    i.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item1
                    , i.Item2.Item1);
        }

        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by selector. </typeparam>
        /// <typeparam name="TA1">The type of the value of the first aggregate.</typeparam>
        /// <typeparam name="TA2">The type of the value of the second aggregate.</typeparam>
        /// <typeparam name="TA3">The type of the value of the third aggregate.</typeparam>
        /// <typeparam name="TA4">The type of the value of the fourth aggregate.</typeparam>
        /// <typeparam name="TA5">The type of the value of the fifth aggregate.</typeparam>
        /// <typeparam name="TA6">The type of the value of the sixth aggregate.</typeparam>
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(
                    i.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item1
                    , i.Item2.Item1);
        }

        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
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
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(
                    i.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item1
                    , i.Item2.Item1);
        }

        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
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
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(
                    i.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item1
                    , i.Item2.Item1);
        }

        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
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
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA9, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA9, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(
                    i.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item1
                    , i.Item2.Item1);
        }

        /// <summary>
        /// Projects the source sequence and the previous aggregated values into a new form.
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
        /// <param name="source">A window to select previously aggregated values on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of source and previous aggregates.</returns>
        public static IEnumerable<TResult> Select<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA10, Tuple<TA9, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            return SelectImpl(source, selector);
        }
        static IEnumerable<TResult> SelectImpl<TSource, TResult, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10>(
            this IWindowAggregateEnumerable<TSource, Tuple<TA10, Tuple<TA9, Tuple<TA8, Tuple<TA7, Tuple<TA6, Tuple<TA5, Tuple<TA4, Tuple<TA3, Tuple<TA2, Tuple<TA1, int>>>>>>>>>>> source
            , Func<TSource, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8, TA9, TA10, TResult> selector)
        {
            foreach (var i in source)
                yield return selector(
                    i.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item2.Item1
                    , i.Item2.Item2.Item1
                    , i.Item2.Item1);
        }
    }
}
