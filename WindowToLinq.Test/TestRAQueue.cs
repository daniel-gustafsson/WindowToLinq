using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WindowToLinq;
using System.Reflection;

namespace WindowToLinq.Test
{
    [TestFixture]
    public sealed class TestRAQueue
    {
        [Test]
        public void Wraparound()
        {
            MethodInfo mi = typeof(RAQueue<int>).GetMethod("CreateForWraparoundTest", BindingFlags.NonPublic | BindingFlags.Static);
            RAQueue<int> queue = (RAQueue<int>)mi.Invoke(null, new object[] { 13, UInt32.MaxValue - 50 });
            for (int i = 1; i <= 10; i++)
                queue.Enqueue(i);

            for (int i = 11; i <= 100; i++)
            {
                Assert.That(queue.SequenceEqual(Enumerable.Range(i - 10, 10)));
                queue.Enqueue(i);
                queue.Dequeue();
            }
        }
    }
}
