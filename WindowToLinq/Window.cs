using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    /// <summary>
    /// A collection of window aggregate extensions for Linq to Objects.
    /// </summary>
    /// <remarks>
    /// In this extension to Linq to Objects, a window is defined as a subsequence of a source sequence
    /// that is specified by the boundary functions associated with the window. The boundary functions
    /// can operate on the values and/or the index of the values of the source sequence. The window can
    /// then be operated on as a sequence itself or can be used in an optimized way with specific aggregate
    /// functions. The latter is especially useful if operating on an unending input source.
    /// </remarks>
    public static partial class WindowExtension
    {
        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on the current source value (first parameter), the window source value (second parameter)
        /// and an index (third parameter) that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound
            , Func<TSource, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(
                source
                , x => true
                , EqualityComparer<bool>.Default
                , precedingBound
                , followingBound
                , (current, index, window) => selector(current, window));
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on an index that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TResult>(
            this IEnumerable<TSource> source
            , Func<int, bool> precedingBound
            , Func<int, bool> followingBound
            , Func<TSource, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(
                source, x => true
                , EqualityComparer<bool>.Default
                , (c, w, i) => precedingBound(i)
                , (c, w, i) => followingBound(i)
                , (current, index, window) => selector(current, window));
        }


        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on the current source value (first parameter), the window source value (second parameter)
        /// and an index (third parameter) that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value. The second parameter is the index of the current value in the window sequence.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound
            , Func<TSource, int, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(source, x => true, EqualityComparer<bool>.Default, precedingBound, followingBound, selector);
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on an index that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value. The second parameter is the index of the current value in the window sequence.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TResult>(
            this IEnumerable<TSource> source
            , Func<int, bool> precedingBound
            , Func<int, bool> followingBound
            , Func<TSource, int, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(source, x => true, EqualityComparer<bool>.Default, (c, w, i) => precedingBound(i), (c, w, i) => followingBound(i), selector);
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on the current source value (first parameter), the window source value (second parameter)
        /// and an index (third parameter) that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TPartitionKey, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound
            , Func<TSource, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(
                source
                , partitionSelector
                , EqualityComparer<TPartitionKey>.Default
                , precedingBound
                , followingBound
                , (current, index, window) => selector(current, window));
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on an index that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TPartitionKey, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<int, bool> precedingBound
            , Func<int, bool> followingBound
            , Func<TSource, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(
                source
                , partitionSelector
                , EqualityComparer<TPartitionKey>.Default
                , (c, w, i) => precedingBound(i)
                , (c, w, i) => followingBound(i)
                , (current, index, window) => selector(current, window));
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on the current source value (first parameter), the window source value (second parameter)
        /// and an index (third parameter) that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value. The second parameter is the index of the current value in the window sequence.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TPartitionKey, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound
            , Func<TSource, int, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(source, partitionSelector, EqualityComparer<TPartitionKey>.Default, precedingBound, followingBound, selector);
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on an index that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value. The second parameter is the index of the current value in the window sequence.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TPartitionKey, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<int, bool> precedingBound
            , Func<int, bool> followingBound
            , Func<TSource, int, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(source, partitionSelector, EqualityComparer<TPartitionKey>.Default, (c, w, i) => precedingBound(i), (c, w, i) => followingBound(i), selector);
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on the current source value (first parameter), the window source value (second parameter)
        /// and an index (third parameter) that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TPartitionKey, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound
            , Func<TSource, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(source, partitionSelector, partitionComparer, precedingBound, followingBound, (current, index, window) => selector(current, window));
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on an index that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TPartitionKey, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<int, bool> precedingBound
            , Func<int, bool> followingBound
            , Func<TSource, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(
                source
                , partitionSelector
                , partitionComparer
                , (c, w, i) => precedingBound(i)
                , (c, w, i) => followingBound(i)
                , (current, index, window) => selector(current, window));
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate on the current source value (first parameter), the window source value (second parameter)
        /// and an index (third parameter) that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value. The second parameter is the index of the current value in the window sequence.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TPartitionKey, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound
            , Func<TSource, int, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(source, partitionSelector, partitionComparer, precedingBound, followingBound, selector);
        }

        /// <summary>
        /// Opens a window on a sequence of values limited by supplied bounds.
        /// </summary>
        /// <remarks>
        /// For each value in the source sequence, a window is calculated from the supplied preceding and following bounds.
        /// The bound functions can operate an index that is centered around the current source value.
        /// Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If all bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the source sequence</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <typeparam name="TResult">The type of elements in the result sequence</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="precedingBound">
        /// A function specifying the preceding bound of the window.
        /// </param>
        /// <param name="followingBound">
        /// A function specifying the following bound of the window.
        /// </param>
        /// <param name="selector">
        /// A transformation from the source value and the window sequence to the result value. The second parameter is the index of the current value in the window sequence.
        /// </param>
        /// <returns>The result enumerable.</returns>
        public static IEnumerable<TResult> Window<TSource, TPartitionKey, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<int, bool> precedingBound
            , Func<int, bool> followingBound
            , Func<TSource, int, IEnumerable<TSource>, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");
            if (selector == null) throw new ArgumentNullException("selector");

            return WindowImpl(source, partitionSelector, partitionComparer, (c, w, i) => precedingBound(i), (c, w, i) => followingBound(i), selector);
        }


        static IEnumerable<TResult> WindowImpl<TSource, TPartitionKey, TResult>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound
            , Func<TSource, int, IEnumerable<TSource>, TResult> selector)
        {
            foreach (IEnumerable<TSource> partition in source.Partition(partitionSelector, partitionComparer))
            {
                using (IEnumerator<TSource> isource = partition.GetEnumerator())
                {
                    SinglePassSequence<TSource> buffer = new SinglePassSequence<TSource>(partition);
                    using (var windowStart = buffer.GetEnumerator())
                    using (var windowEnd = buffer.GetEnumerator())
                    using (var current = buffer.GetEnumerator())
                    {
                        windowStart.MoveNext();
                        windowEnd.MoveNext();

                        while (current.MoveNext())
                        {
                            while (windowEnd.Valid
                                && followingBound(
                                    current.Current
                                    , windowEnd.Current
                                    , current.DistanceBefore(windowEnd)))
                            {
                                windowEnd.MoveNext();
                            }

                            while (windowStart.Valid
                                && !windowStart.HasSamePositionAs(windowEnd)
                                && !precedingBound(
                                    current.Current
                                    , windowStart.Current
                                    , current.DistanceBefore(windowStart)))
                            {
                                windowStart.MoveNext();
                            }

                            yield return selector(current.Current, windowStart.DistanceBefore(current), buffer.Range(windowStart, windowEnd));
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Interface for an aggregateable enumerable (window).
        /// </summary>
        /// <typeparam name="TElement">The type of element in the source sequence.</typeparam>
        /// <typeparam name="TAggregates">The type of a tuple of aggregates</typeparam>
        public interface IWindowAggregateEnumerable<TElement, TAggregates> : IEnumerable<Tuple<TElement, TAggregates>>
        {
            /// <summary>
            /// Get an enumerator that calls the supplied actions on each value to aggregate.
            /// </summary>
            /// <param name="accumulate">An action to be called when accumulating a new value.</param>
            /// <param name="decumulate">An action to be called when decumulating a previously accumulated value.</param>
            /// <param name="reset">An action to be called when aggregates should be reset to the original seed.</param>
            /// <param name="predicate">A predicate that returns true if the current items should be used by the aggregate. Index is reset to 0 for each partition.</param>
            /// <returns>A sequence of tuples containing the window source and all aggregates.</returns>
            IEnumerator<Tuple<TElement, TAggregates>> GetEnumerator(
                Action<TElement, int> accumulate
                , Action<TElement, int> decumulate
                , Action reset
                , Func<TElement, int, bool> predicate);

            /// <summary>
            /// Get a tuple containing all the aggregates as if they were called on an empty sequence.
            /// </summary>
            /// <returns></returns>
            Tuple<TElement, TAggregates> GetDefault();
        }

        /// <summary>
        /// Interface for an aggregate function over an aggregate enumerable (window).
        /// </summary>
        /// <typeparam name="TElement">The type of element in the source sequence.</typeparam>
        /// <typeparam name="TAggregates">The type of a tuple of aggregates</typeparam>
        public interface IWindowAggregateFunction<TElement, TAggregates> : IWindowAggregateEnumerable<TElement, TAggregates>
        {
        }

        sealed class WindowAggregateFunction<TSource, TSourceAggregates, TAccumulate, TResult> : IWindowAggregateFunction<TSource, Tuple<TResult, TSourceAggregates>>
        {
            readonly IWindowAggregateEnumerable<TSource, TSourceAggregates> source;
            readonly TAccumulate seed;
            readonly Func<TAccumulate, TSource, TAccumulate> accumulator;
            readonly Func<TAccumulate, TSource, TAccumulate> decumulator;
            readonly Func<TAccumulate, TResult> resultSelector;

            public WindowAggregateFunction(
                IWindowAggregateEnumerable<TSource, TSourceAggregates> source
                , TAccumulate seed
                , Func<TAccumulate, TSource, TAccumulate> accumulator
                , Func<TAccumulate, TSource, TAccumulate> decumulator
                , Func<TAccumulate, TResult> resultSelector)
            {
                this.source = source;
                this.seed = seed;
                this.accumulator = accumulator;
                this.decumulator = decumulator;
                this.resultSelector = resultSelector;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public IEnumerator<Tuple<TSource, Tuple<TResult, TSourceAggregates>>> GetEnumerator()
            {
                return GetEnumerator((d, i) => { }, (d, i) => { }, () => { }, (d, i) => true);
            }


            public IEnumerator<Tuple<TSource, Tuple<TResult, TSourceAggregates>>> GetEnumerator(
                Action<TSource, int> accumulate, Action<TSource, int> decumulate, Action reset, Func<TSource, int, bool> predicate)
            {
                TAccumulate value = this.seed;
                using (IEnumerator<Tuple<TSource, TSourceAggregates>> iSource = this.source.GetEnumerator(
                    (d, i) =>
                    { // Accumulate if predicate is true
                        if (predicate(d, i))
                            value = this.accumulator(value, d);
                        accumulate(d, i);
                    }
                    , (d, i) =>
                    { // Decumulate if predicate is true
                        if (predicate(d, i))
                            value = this.decumulator(value, d);
                        decumulate(d, i);
                    }
                    , () =>
                    { // Reset
                        value = this.seed;
                        reset();
                    }
                    , (d, i) => true))
                {
                    while (iSource.MoveNext())
                    {
                        yield return Tuple.Create(iSource.Current.Item1, Tuple.Create(this.resultSelector(value), iSource.Current.Item2));
                    }
                }
            }

            public Tuple<TSource, Tuple<TResult, TSourceAggregates>> GetDefault()
            {
                Tuple<TSource, TSourceAggregates> prevDefault = this.source.GetDefault();
                return Tuple.Create(prevDefault.Item1, Tuple.Create(this.resultSelector(this.seed), prevDefault.Item2));
            }
        }

        sealed class WindowAggregator<TSource, TPartitionKey> : IWindowAggregateEnumerable<TSource, int>
        {
            readonly IEnumerable<TSource> source;
            readonly Func<TSource, TPartitionKey> partitionSelector;
            readonly IEqualityComparer<TPartitionKey> partitionComparer;
            readonly Func<TSource, TSource, int, bool> precedingBound;
            readonly Func<TSource, TSource, int, bool> followingBound;

            public WindowAggregator(
                IEnumerable<TSource> source
                , Func<TSource, TPartitionKey> partitionSelector
                , IEqualityComparer<TPartitionKey> partitionComparer
                , Func<TSource, TSource, int, bool> precedingBound
                , Func<TSource, TSource, int, bool> followingBound)
            {
                this.source = source;
                this.partitionSelector = partitionSelector;
                this.partitionComparer = partitionComparer;
                this.precedingBound = precedingBound;
                this.followingBound = followingBound;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public IEnumerator<Tuple<TSource, int>> GetEnumerator()
            {
                foreach (var item in this.source)
                    yield return Tuple.Create(item, 0);
            }


            IEnumerable<Tuple<TSource, int>> GetEnumerator(
                IEnumerable<TSource> partition
                , Action<TSource, int> accumulate
                , Action<TSource, int> decumulate)
            {
                SinglePassSequence<TSource> buffer = new SinglePassSequence<TSource>(partition);
                using (var windowStart = buffer.GetEnumerator())
                using (var windowEnd = buffer.GetEnumerator())
                using (var current = buffer.GetEnumerator())
                {
                    windowStart.MoveNext();
                    windowEnd.MoveNext();
                    int accumulatorIndex = 0;
                    int decumulatorIndex = 0;

                    while (current.MoveNext())
                    {
                        while (windowEnd.Valid
                            && this.followingBound(
                                current.Current
                                , windowEnd.Current
                                , current.DistanceBefore(windowEnd)))
                        {
                            checked { accumulate(windowEnd.Current, unchecked(accumulatorIndex++)); }
                            windowEnd.MoveNext();
                        }

                        while (windowStart.Valid
                            && !windowStart.HasSamePositionAs(windowEnd)
                            && !this.precedingBound(
                                current.Current
                                , windowStart.Current
                                , current.DistanceBefore(windowStart)))
                        {
                            checked { decumulate(windowStart.Current, unchecked(decumulatorIndex++)); }
                            windowStart.MoveNext();
                        }

                        yield return Tuple.Create(current.Current, 0);
                    }
                }
            }

            IEnumerable<Tuple<TSource, int>> GetEnumerator(
                IEnumerable<TSource> partition
                , Action<TSource, int> accumulate)
            {
                SinglePassSequence<TSource> buffer = new SinglePassSequence<TSource>(partition);
                using (var windowEnd = buffer.GetEnumerator())
                using (var current = buffer.GetEnumerator())
                {
                    windowEnd.MoveNext();
                    int accumulatorIndex = 0;

                    while (current.MoveNext())
                    {
                        while (windowEnd.Valid
                            && this.followingBound(
                                current.Current
                                , windowEnd.Current
                                , current.DistanceBefore(windowEnd)))
                        {
                            checked { accumulate(windowEnd.Current, unchecked(accumulatorIndex++)); }
                            windowEnd.MoveNext();
                        }

                        yield return Tuple.Create(current.Current, 0);
                    }
                }
            }

            public IEnumerator<Tuple<TSource, int>> GetEnumerator(
                Action<TSource, int> accumulate
                , Action<TSource, int> decumulate
                , Action reset
                , Func<TSource, int, bool> predicate)
            {
                foreach (IEnumerable<TSource> partition in this.source.Partition(this.partitionSelector, this.partitionComparer))
                {
                    reset();

                    if (this.precedingBound != null)
                    {
                        foreach (var value in GetEnumerator(partition, accumulate, decumulate))
                            yield return value;
                    }
                    else
                    {
                        foreach (var value in GetEnumerator(partition, accumulate))
                            yield return value;
                    }
                }
            }


            public Tuple<TSource, int> GetDefault()
            {
                return Tuple.Create(default(TSource), 0);
            }
        }


        /// <summary>
        /// Opens a window with specified bounds that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If both bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> Window<TSource>(
            this IEnumerable<TSource> source
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, bool>(source, x => true, EqualityComparer<bool>.Default, precedingBound, followingBound);
        }

        /// <summary>
        /// Opens a window with specified bounds that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If both bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> Window<TSource>(
            this IEnumerable<TSource> source
            , Func<int, bool> precedingBound
            , Func<int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, bool>(source, x => true, EqualityComparer<bool>.Default, (c, w, i) => precedingBound(i), (c, w, i) => followingBound(i));
        }

        /// <summary>
        /// Opens a window with specified bounds that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If both bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> Window<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, EqualityComparer<TPartitionKey>.Default, precedingBound, followingBound);
        }

        /// <summary>
        /// Opens a window with specified bounds that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If both bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> Window<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<int, bool> precedingBound
            , Func<int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, EqualityComparer<TPartitionKey>.Default, (c, w, i) => precedingBound(i), (c, w, i) => followingBound(i));
        }

        /// <summary>
        /// Opens a window with specified bounds that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If both bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> Window<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<TSource, TSource, int, bool> precedingBound
            , Func<TSource, TSource, int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, partitionComparer, precedingBound, followingBound);
        }

        /// <summary>
        /// Opens a window with specified bounds that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If both bound functions returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> Window<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<int, bool> precedingBound
            , Func<int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, partitionComparer, (c, w, i) => precedingBound(i), (c, w, i) => followingBound(i));
        }

        /// <summary>
        /// Opens a window with specified following bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedPreceding<TSource>(
            this IEnumerable<TSource> source
            , Func<TSource, TSource, int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, bool>(source, x => true, EqualityComparer<bool>.Default, null, followingBound);
        }

        /// <summary>
        /// Opens a window with specified following bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedPreceding<TSource>(
            this IEnumerable<TSource> source
            , Func<int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, bool>(source, x => true, EqualityComparer<bool>.Default, null, (c, w, i) => followingBound(i));
        }

        /// <summary>
        /// Opens a window with specified following bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedPreceding<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<TSource, TSource, int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, EqualityComparer<TPartitionKey>.Default, null, followingBound);
        }

        /// <summary>
        /// Opens a window with specified following bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedPreceding<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, EqualityComparer<TPartitionKey>.Default, null, (c, w, i) => followingBound(i));
        }

        /// <summary>
        /// Opens a window with specified following bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedPreceding<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<TSource, TSource, int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, partitionComparer, null, followingBound);
        }

        /// <summary>
        /// Opens a window with specified following bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="followingBound">A function specifying the following bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedPreceding<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<int, bool> followingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (followingBound == null) throw new ArgumentNullException("followingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, partitionComparer, null, (c, w, i) => followingBound(i));
        }

        /// <summary>
        /// Opens a window with specified preceding bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedFollowing<TSource>(
            this IEnumerable<TSource> source
            , Func<TSource, TSource, int, bool> precedingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");

            return new WindowAggregator<TSource, bool>(source, x => true, EqualityComparer<bool>.Default, precedingBound, (w, c, i) => true);
        }

        /// <summary>
        /// Opens a window with specified preceding bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedFollowing<TSource>(
            this IEnumerable<TSource> source
            , Func<int, bool> precedingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");

            return new WindowAggregator<TSource, bool>(source, x => true, EqualityComparer<bool>.Default, (c, w, i) => precedingBound(i), (w, c, i) => true);
        }

        /// <summary>
        /// Opens a window with specified preceding bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedFollowing<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<TSource, TSource, int, bool> precedingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, EqualityComparer<TPartitionKey>.Default, precedingBound, (w, c, i) => true);
        }

        /// <summary>
        /// Opens a window with specified preceding bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedFollowing<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , Func<int, bool> precedingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, EqualityComparer<TPartitionKey>.Default, (c, w, i) => precedingBound(i), (w, c, i) => true);
        }

        /// <summary>
        /// Opens a window with specified preceding bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedFollowing<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<TSource, TSource, int, bool> precedingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, partitionComparer, precedingBound, (w, c, i) => true);
        }

        /// <summary>
        /// Opens a window with specified preceding bound that can be aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// 
        /// The bound functions can operate on the source value (first parameter) or an index (second parameter) that is centered around the current source value. Current value has index 0, preceding values have negative indexes and following values positive indexes.
        /// If the bound function returns true, the source value is considered part of the window.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <param name="precedingBound">A function specifying the preceding bound of the window.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnboundedFollowing<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer
            , Func<int, bool> precedingBound)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");
            if (precedingBound == null) throw new ArgumentNullException("precedingBound");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, partitionComparer, (c, w, i) => precedingBound(i), (w, c, i) => true);
        }

        /// <summary>
        /// Opens an unbounded window aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The source sequence</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnbounded<TSource>(
            this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new WindowAggregator<TSource, bool>(source, x => true, EqualityComparer<bool>.Default, null, (c, w, i) => true);
        }

        /// <summary>
        /// Opens an unbounded window aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnbounded<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, EqualityComparer<TPartitionKey>.Default, null, (c, w, i) => true);
        }

        /// <summary>
        /// Opens an unbounded window aggregated on. The window stays open during aggregates until a later call to Select.
        /// </summary>
        /// <remarks>
        /// Aggregates are specified by following overloaded calls to Aggregate, Count, Min, Max, Sum and Average.
        /// All the aggregate functions on an open window will be sent each element in the source sequence just once
        /// for accumulation, and possibly once more for decumulationg. Min and Max does not support decumulation.
        /// Decumulation only happens if the preceding bound moves forward in a partition.
        /// WindowUnboundedPreceding is an optimized version that does not buffer rows before the current item
        /// and therefore allow running aggregates on unending enumerables.
        /// </remarks>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="partitionSelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="partitionComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <returns>A window sequence that can be aggregated on.</returns>
        public static IWindowAggregateEnumerable<TSource, int> WindowUnbounded<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> partitionSelector
            , IEqualityComparer<TPartitionKey> partitionComparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (partitionSelector == null) throw new ArgumentNullException("partitionSelector");
            if (partitionComparer == null) throw new ArgumentNullException("partitionComparer");

            return new WindowAggregator<TSource, TPartitionKey>(source, partitionSelector, partitionComparer, null, (c, w, i) => true);
        }
    }
}
