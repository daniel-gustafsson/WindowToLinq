using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    public static partial class WindowExtension
    {
        /// <summary>
        /// Partitions the source sequence into a sequence of sequences.
        /// </summary>
        /// <remarks>
        /// Each sub sequence contains consecutive values with the same key values. No reordering or buffering is done, so dicontinous groups with the same key will not be part of the same sequence.
        /// </remarks>
        /// <typeparam name="TSource">The type of the source element</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="keySelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <returns>A sequence of sequences.</returns>
        public static IEnumerable<IEnumerable<TSource>> Partition<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> keySelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            return PartitionImpl(source, keySelector, EqualityComparer<TPartitionKey>.Default);
        }

        /// <summary>
        /// Partitions the source sequence into a sequence of sequences.
        /// </summary>
        /// <remarks>
        /// Each sub sequence contains consecutive values with the same key values. No reordering or buffering is done, so dicontinous groups with the same key will not be part of the same sequence.
        /// </remarks>
        /// <typeparam name="TSource">The type of the source element</typeparam>
        /// <typeparam name="TPartitionKey">The type of the partition key</typeparam>
        /// <param name="source">The source sequence</param>
        /// <param name="keySelector">Selects the key from the source on which the window will be partitioned. Each time the key changes, the window will restart.</param>
        /// <param name="keyComparer">An IEqualityComparer&lt;T&gt;to compare partition keys with.</param>
        /// <returns>A sequence of sequences.</returns>
        public static IEnumerable<IEnumerable<TSource>> Partition<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> keySelector
            , IEqualityComparer<TPartitionKey> keyComparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (keyComparer == null) throw new ArgumentNullException("keyComparer");

            return PartitionImpl(source, keySelector, keyComparer);
        }

        static IEnumerable<TSource> GetPartition<TSource>(Func<Tuple<bool, TSource>> sourceItr)
        {
            Tuple<bool, TSource> current = sourceItr();
            while (current.Item1)
            {
                yield return current.Item2;
                current = sourceItr();
            }
        }

        static IEnumerable<IEnumerable<TSource>> PartitionImpl<TSource, TPartitionKey>(
            this IEnumerable<TSource> source
            , Func<TSource, TPartitionKey> keySelector
            , IEqualityComparer<TPartitionKey> keyComparer)
        {
            using (IEnumerator<TSource> iSource = source.GetEnumerator())
            {
                bool hasInput = iSource.MoveNext();
                while (hasInput)
                {
                    TPartitionKey currentPartition = keySelector(iSource.Current);
                    yield return GetPartition(
                        () =>
                        {
                            bool ret = hasInput && keyComparer.Equals(keySelector(iSource.Current), currentPartition);
                            TSource data = default(TSource);
                            if (ret)
                            {
                                data = iSource.Current;
                                hasInput = iSource.MoveNext();
                            }
                            return Tuple.Create(ret, data);
                        });
                }
            }
        }
    }
}
