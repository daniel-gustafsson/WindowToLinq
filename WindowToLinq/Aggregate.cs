using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    public static partial class WindowExtension
    {
        /// <summary>
        /// Applies an accumulator function over a window.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source. </typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">An window to aggregate over.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<TSource, TSourceAggregates>> Aggregate<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, TSource, TSource> accumulator)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (accumulator == null) throw new ArgumentNullException("accumulator");

            return new WindowAggregateFunction<TSource, TSourceAggregates, Tuple<TSource, bool>, TSource>(
                source
                , Tuple.Create(default(TSource), false)
                , (a, s) => Tuple.Create(a.Item2 ? accumulator(a.Item1, s) : s, true)
                , (a, s) => { throw new InvalidOperationException("Bounded precedence window used on aggregate without decumulator"); }
                , x => x.Item1);
        }

        /// <summary>
        /// Applies an accumulator function over a window. The specified seed value is used as the initial accumulator value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source. </typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">An window to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<TAccumulate, TSourceAggregates>> Aggregate<TSource, TSourceAggregates, TAccumulate>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , TAccumulate seed
            , Func<TAccumulate, TSource, TAccumulate> accumulator)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (accumulator == null) throw new ArgumentNullException("accumulator");

            return new WindowAggregateFunction<TSource, TSourceAggregates, TAccumulate, TAccumulate>(
                source
                , seed
                , accumulator
                , (a, s) => { throw new InvalidOperationException("Bounded precedence window used on aggregate without decumulator"); }
                , x => x);
        }

        /// <summary>
        /// Applies an accumulator function over a window. The specified seed value is used as the initial accumulator value, and the specified function is used to select the result value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source. </typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">An window to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <param name="aggregateSelector">A function to transform the final accumulator value into the result value.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<TResult, TSourceAggregates>> Aggregate<TSource, TSourceAggregates, TAccumulate, TResult>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , TAccumulate seed
            , Func<TAccumulate, TSource, TAccumulate> accumulator
            , Func<TAccumulate, TResult> aggregateSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (accumulator == null) throw new ArgumentNullException("accumulator");
            if (aggregateSelector == null) throw new ArgumentNullException("aggregateSelector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, TAccumulate, TResult>(
                source
                , seed
                , accumulator
                , (a, s) => { throw new InvalidOperationException("Bounded precedence window used on aggregate without decumulator"); }
                , aggregateSelector);
        }

        /// <summary>
        /// Applies an accumulator function over a window.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source. </typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <param name="source">An window to aggregate over.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <param name="decumulator">A decumulator function to be invoked on each element.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<TSource, TSourceAggregates>> Aggregate<TSource, TSourceAggregates>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , Func<TSource, TSource, TSource> accumulator
            , Func<TSource, TSource, TSource> decumulator)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (accumulator == null) throw new ArgumentNullException("accumulator");
            if (decumulator == null) throw new ArgumentNullException("decumulator");

            return new WindowAggregateFunction<TSource, TSourceAggregates, TSource, TSource>(
                source
                , default(TSource)
                , accumulator
                , decumulator
                , x => x);
        }

        /// <summary>
        /// Applies an accumulator function over a window. The specified seed value is used as the initial accumulator value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source. </typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <param name="source">An window to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <param name="decumulator">A decumulator function to be invoked on each element.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<TAccumulate, TSourceAggregates>> Aggregate<TSource, TSourceAggregates, TAccumulate>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , TAccumulate seed
            , Func<TAccumulate, TSource, TAccumulate> accumulator
            , Func<TAccumulate, TSource, TAccumulate> decumulator)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (accumulator == null) throw new ArgumentNullException("accumulator");
            if (decumulator == null) throw new ArgumentNullException("decumulator");

            return new WindowAggregateFunction<TSource, TSourceAggregates, TAccumulate, TAccumulate>(source, seed, accumulator, decumulator, x => x);
        }

        /// <summary>
        /// Applies an accumulator function over a window. The specified seed value is used as the initial accumulator value, and the specified function is used to select the result value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source. </typeparam>
        /// <typeparam name="TSourceAggregates">The type of the previous aggregate tuple.</typeparam>
        /// <typeparam name="TAccumulate">The type of the accumulator value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="source">An window to aggregate over.</param>
        /// <param name="seed">The initial accumulator value.</param>
        /// <param name="accumulator">An accumulator function to be invoked on each element.</param>
        /// <param name="decumulator">A decumulator function to be invoked on each element.</param>
        /// <param name="aggregateSelector">A function to transform the final accumulator value into the result value.</param>
        /// <returns>A new sequence consisting of the source, any previous aggregates and this aggregate.</returns>
        public static IWindowAggregateFunction<TSource, Tuple<TResult, TSourceAggregates>> Aggregate<TSource, TSourceAggregates, TAccumulate, TResult>(
            this IWindowAggregateEnumerable<TSource, TSourceAggregates> source
            , TAccumulate seed
            , Func<TAccumulate, TSource, TAccumulate> accumulator
            , Func<TAccumulate, TSource, TAccumulate> decumulator
            , Func<TAccumulate, TResult> aggregateSelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (accumulator == null) throw new ArgumentNullException("accumulator");
            if (decumulator == null) throw new ArgumentNullException("decumulator");
            if (aggregateSelector == null) throw new ArgumentNullException("aggregateSelector");

            return new WindowAggregateFunction<TSource, TSourceAggregates, TAccumulate, TResult>(source, seed, accumulator, decumulator, aggregateSelector);
        }
    }
}
