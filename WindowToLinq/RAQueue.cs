using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowToLinq
{
    /// <summary>
    /// A random access queue.
    /// </summary>
    /// <remarks>
    /// A regular queue with random access functionality.
    /// Externa indexes into the queue are of the absolute value of the element added to the queue.
    /// This means the indexes can be much larger than the number of current elements in the queue.
    /// The indexes wrap around to zero after reaching UInt32.MaxValue.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the queue.</typeparam>
    public sealed class RAQueue<T> : IEnumerable<T>
    {
        const int defaultCapacity = 8;
        T[] buffer;
        uint start = 0;
        uint end = 0;

        uint Length
        {
            get
            {
                return unchecked(end - start);
            }
        }

        /// <summary>
        /// Gets the number of elements in the queue.
        /// </summary>
        public int Count
        {
            get
            {
                return (int)Length;
            }
        }

        // Make capacity a multiple of 2 to make index wraparound easier
        int AdjustCapacity(int capacity)
        {
            capacity = Enumerable.Range(0, 29).Select(i => 2 << i).Where(r => r >= capacity).FirstOrDefault();
            if (capacity == 0)
                throw new ArgumentOutOfRangeException();
            return capacity;
        }

        /// <summary>
        /// Gets or sets the capacity of the queue.
        /// </summary>
        /// <remarks>
        /// The maximum capacity of a RAQueue is 2^30 elements.
        /// </remarks>
        public int Capacity
        {
            get
            {
                return buffer.Length;
            }
            set
            {
                if (value < Count)
                    throw new ArgumentOutOfRangeException();

                Resize(AdjustCapacity(value));
            }
        }

        /// <summary>
        /// Initializes a new instance of a RAQueue with the default capacity.
        /// </summary>
        public RAQueue()
        {
            buffer = new T[defaultCapacity];
        }

        /// <summary>
        /// Initializes a new instance of a RAQueue with the supplied capacity.
        /// </summary>
        /// <param name="capacity">Capacity in number of elements. Maximum capacity is 2^30 elements.</param>
        public RAQueue(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", "Capacity is less than 0.");
            buffer = new T[AdjustCapacity(capacity)];
        }

        /// <summary>
        /// Clears the buffer of all elements and resets capacity to default.
        /// </summary>
        public void Clear()
        {
            buffer = new T[defaultCapacity];
            start = 0;
            end = 0;
        }

        /// <summary>
        /// Enqueues a new element at the end of the queue.
        /// </summary>
        /// <param name="elem">The element to enqueue.</param>
        public void Enqueue(T elem)
        {
            if (Count == Capacity)
            {
                if (buffer.Length == Int32.MaxValue)
                    throw new InvalidOperationException("The queue is at maxium capacity.");

                long newCapacity = buffer.LongLength + (buffer.LongLength < 1000 ? buffer.LongLength : 1000);
                Resize((int)Math.Min(newCapacity, (long)Int32.MaxValue));
            }

            unchecked { buffer[end++ % (uint)buffer.Length] = elem; }
        }

        /// <summary>
        /// Dequeues the first element of th equeue.
        /// </summary>
        /// <returns>The dequeued element.</returns>
        public T Dequeue()
        {
            if (Length == 0)
                throw new InvalidOperationException("The queue is empty");

            unchecked
            {
                buffer[start % (uint)buffer.Length] = default(T);
                return buffer[start++ % (uint)buffer.Length];
            }
        }

        /// <summary>
        /// Removes all elements up to but not including the element pointed to by the index.
        /// </summary>
        /// <param name="index">The absolute index, including any previously dequeued elements. Wraps around at UInt32.MaxValue.</param>
        public void RemoveUpTo(uint index)
        {
            if (!ValidIndex(index))
                throw new IndexOutOfRangeException();

            unchecked
            {
                while (start != index && Length > 0)
                {
                    buffer[start++ % (uint)buffer.Length] = default(T);
                }
            }
        }

        void Resize(int newCapacity)
        {
            unchecked
            {
                T[] old = buffer;
                buffer = new T[newCapacity];
                for (uint i = 0; i < Length; i++)
                    buffer[(start + i) % (uint)newCapacity] = old[(start + i) % (uint)old.Length];
            }
        }

        /// <summary>
        /// Indicates if an index is within the range of the current queue
        /// </summary>
        /// <param name="index">The index to check.</param>
        /// <returns>True if the index is valid. False otherwise.</returns>
        public bool ValidIndex(uint index)
        {
            bool invalid = unchecked((index - start > (uint)Int32.MaxValue) || index == end || (end - index > (uint)Int32.MaxValue));
            return !invalid;
        }

        /// <summary>
        /// Returns the current element at the start of the queue.
        /// </summary>
        /// <returns>The start element.</returns>
        public T Peek()
        {
            unchecked
            {
                if (Length == 0)
                    throw new InvalidOperationException();
                return buffer[end % (uint)buffer.Length];
            }
        }

        /// <summary>
        /// Returns the absolute index at front of the queue. Wraps around after UInt32.MaxValue.
        /// </summary>
        /// <returns>The index at the front.</returns>
        public uint IndexAtFront()
        {
            return start;
        }

        /// <summary>
        /// Returns the absolute index at the end of the queue. The end index is one step after the actual end of the queue and will never point to a valid element. Wraps around after UInt32.MaxValue.
        /// </summary>
        /// <returns>The index at the end.</returns>
        public uint IndexAtEnd()
        {
            return end;
        }

        /// <summary>
        /// Returns the element with the supplied index into the queue.
        /// </summary>
        /// <param name="index">The absolute index, including any previously dequeued elements. Wraps around at UInt32.MaxValue.</param>
        /// <returns>The indexed element.</returns>
        public T PeekAt(uint index)
        {
            if (!ValidIndex(index))
                throw new IndexOutOfRangeException();
            unchecked { return buffer[index % (uint)buffer.Length]; }
        }

        /// <summary>
        /// Returns a sequence of elements starting at the start index and ending at the element before the end index.
        /// </summary>
        /// <param name="start">The absolute index of the start element, including any previously dequeued elements. Wraps around at UInt32.MaxValue.</param>
        /// <param name="end">The absolute index of the end element, including any previously dequeued elements. Wraps around at UInt32.MaxValue.</param>
        /// <returns>The sequence.</returns>
        public IEnumerable<T> Range(uint start, uint end)
        {
            if (!ValidIndex(start) || (!ValidIndex(end) && end != this.end))
                throw new IndexOutOfRangeException();

            unchecked
            {
                uint s = this.start;
                uint e = this.end;
                for (uint i = start; i != end; ++i)
                {
                    if (s != this.start || e != this.end)
                        throw new InvalidOperationException("Queue was modified while iterating");
                    yield return buffer[i % (uint)buffer.Length];
                }
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates over the queue.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            unchecked
            {
                uint s = start;
                uint e = end;
                for (uint i = s; i != e; ++i)
                {
                    if (s != start || e != end)
                        throw new InvalidOperationException("Queue was modified while iterating");
                    yield return buffer[i % (uint)buffer.Length];
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        // For unit testing wraparound
        static RAQueue<T> CreateForWraparoundTest(int capacity, uint seedStart)
        {
            RAQueue<T> queue = new RAQueue<T>(capacity);
            queue.start = seedStart;
            queue.end = seedStart;
            return queue;
        }
    }
}
