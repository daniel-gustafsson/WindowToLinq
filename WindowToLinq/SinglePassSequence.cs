using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WindowToLinq
{
    /// <summary>
    /// A buffer for simutaneously iterating multiple times over a sequence in a singel pass.
    /// </summary>
    /// <remarks>
    /// The purpose of this class is to allow multiple simultaneous enumerations on an input sequence
    /// where the input sequence will be enumerated only once. This is accomplished by buffering the
    /// elements in the delta between the first (closest to the end of the input sequence) and last
    /// (closest to the start of the input sequence) enumerators. New enumerators can be added at any
    /// time, but they will not see elements that has already been stepped over by all the current enumerators.
    /// </remarks>
    /// <typeparam name="T">The type of element in the input sequence.</typeparam>
    public sealed class SinglePassSequence<T> : IEnumerable<T>, IDisposable
    {
        RAQueue<T> queue = new RAQueue<T>();
        IEnumerator<T> input;
        bool hasInput;
        List<Enumerator> enumerators = new List<Enumerator>();

        /// <summary>
        /// Initializes a new instance of a SinglePassBuffer.
        /// </summary>
        /// <param name="input">The sequence that the buffer will pass on to each of it's enumerators.</param>
        public SinglePassSequence(IEnumerable<T> input)
        {
            this.input = input.GetEnumerator();
            hasInput = this.input.MoveNext();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the source sequence.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public Enumerator GetEnumerator()
        {
            Enumerator e = new Enumerator(this, queue);
            enumerators.Add(e);
            return e;
        }

        bool MoveNext()
        {
            if (hasInput)
            {
                queue.Enqueue(input.Current);
                hasInput = input.MoveNext();
                return true;
            }
            else
            {
                return false;
            }
        }

        void ClearTail()
        {
            while (queue.Count > 0)
            {
                foreach (Enumerator e in enumerators)
                {
                    if (!e.Started || e.Position == queue.IndexAtFront())
                        return;
                }
                queue.Dequeue();
            }
        }

        /// <summary>
        /// Releases all resources used by the SinglePassBuffer&lt;T&gt;.Enumerator. 
        /// </summary>
        public void Dispose()
        {
            enumerators.Clear();
            input.Dispose();
        }

        /// <summary>
        /// Returns a sequence of the elements beginning at the first enumerator and ending before the second enumerator.
        /// An enumerator that has passed the end of the sequence is allowed as an argument to this function.
        /// </summary>
        /// <param name="begin">Begin iterating at the element that this enumerator points to.</param>
        /// <param name="end">Stop iterating at the element before the element this enumerator points to.</param>
        /// <returns>The sequence of elements.</returns>
        public IEnumerable<T> Range(Enumerator begin, Enumerator end)
        {
            return queue.Range(begin.Position, end.Position);
        }

        /// <summary>
        /// The enumerator class for the SinglePassBuffer.
        /// </summary>
        [DebuggerDisplay("Current = {Current}, Position = {Position}, Valid = {Valid}")]
        public sealed class Enumerator : IEnumerator<T>
        {
            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            public T Current { get { return queue.PeekAt(Position); } }
            object System.Collections.IEnumerator.Current { get { return this.Current; } }

            internal uint Position { get; set; }
            internal bool Started { get; private set; }

            SinglePassSequence<T> buffer;
            RAQueue<T> queue;

            /// <summary>
            /// True if the position of the enumerator points to a valid element, false otherwise.
            /// </summary>
            public bool Valid
            {
                get { return queue.ValidIndex(Position); }
            }

            internal Enumerator(SinglePassSequence<T> buffer, RAQueue<T> queue)
            {
                this.buffer = buffer;
                this.queue = queue;
                Position = unchecked(queue.IndexAtFront() - 1);
                Started = false;
            }


            /// <summary>
            /// Advances the enumerator to the next element of the SinglePassBuffer."/>
            /// </summary>
            /// <returns>True if the enumerator successfully advance to the next element; false otherwise.</returns>
            public bool MoveNext()
            {
                Started = true;

                unchecked { Position++; }
                if (!Valid)
                    buffer.MoveNext();

                buffer.ClearTail();

                return Valid;
            }

            /// <summary>
            /// Releases all resources used by the Enumerator.
            /// </summary>
            public void Dispose()
            {
                buffer.enumerators.Remove(this);
                buffer.ClearTail();
            }

            /// <summary>
            /// Not implemented.
            /// </summary>
            public void Reset() { throw new NotImplementedException(); }


            /// <summary>
            /// Checks if two enumerators point to the same element in the input buffer.
            /// </summary>
            /// <param name="other">Other enumerator to compare to.</param>
            /// <returns>True if both iterators point to the same element, false otherwise.</returns>
            public bool HasSamePositionAs(Enumerator other)
            {
                if (other == null)
                    return false;
                if (other.buffer != this.buffer)
                    return false;
                return other.Position == this.Position;
            }

            /// <summary>
            /// Calculates the distance (number of elements) between two enumerators
            /// </summary>
            /// <param name="other">The other enumerator to compare to</param>
            /// <returns>A positive number if this iterator comes before the other, or negative number the other comes before this.</returns>
            public int DistanceBefore(Enumerator other)
            {
                return (int)((long)other.Position - (long)this.Position);
            }
        }
    }
}
