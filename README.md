# Window To LINQ

Window To LINQ is a library of extension functions for .NET.

## Summary

LINQ query comprehension has many similarities to the SQL query language. One thing that is missing from the standard implementation of LINQ though, is window aggregate functions.  This library tries to remedy the situation by extending LINQ to Objects with generic window functionality.

## Reference

Library reference documentation can be found [here](http://despathy.github.com/WindowToLinq/reference/Index.html).

## Description

Window aggregate functions in SQL are great for efficient calculation of running aggregates, moving average and other things. Microsoft SQL server 2008 R2 only supports a simpler form of window functions without boundary limits, but there is hope for better support in the next version. It is my hope that Microsoft will extend the LINQ query comprehension language to include support for window functions with a complementing implementation of LINQ to SQL.

This library defines a window over a source sequence (`IEnumerable<T>`) to be a subsequence for each input element, specified by a partition selector and boundary functions for preceding and following bound. There are many overloads of the `Window`, `WindowUnbounded`, `WindowUnboundedPreceding` and `WindowUnboundedFollowing` extension functions, but there are two main types. There are those with a selector argument and those without.

The functions with a selector argument open a window and apply the selector with both the source element and the sequence of elements that is the window. The whole of the window therefore needs to be buffered.

```c#
var result = Enumerable.Range(1, 10)
    .Window(
        i => i >= -1    // Window start 1 element before current
        , i => i <= 1   // Window ends 1 element after current
        , (source, window) => Tuple.Create(source, window.Count(), window.Sum()));

// When iterated on, the result sequence should be the same as
var expected = new List<Tuple<int, int, int>> {
        Tuple.Create(1, 2, 3)
        , Tuple.Create(2, 3, 6)
        , Tuple.Create(3, 3, 9)
        , Tuple.Create(4, 3, 12)
        , Tuple.Create(5, 3, 15)
        , Tuple.Create(6, 3, 18)
        , Tuple.Create(7, 3, 21)
        , Tuple.Create(8, 3, 24)
        , Tuple.Create(9, 3, 27)
        , Tuple.Create(10, 2, 19)
    };

```

The window sequence in the selector is an IEnumerable<T> that can be freely iterated on.

Just like the SQL `OVER` clause, a window can be created on a partition:

```c#
var source = from s in Enumerable.Range(1, 3)
                from v in Enumerable.Range(s, 2)
                select Tuple.Create(s, v);

var result = source.Window(
        s => s.Item1    // Partition on first value
        , i => true     // No preceding bound
        , i => true     // No following bound
        , (src, window) => window.Sum(w => w.Item2));

var expected = new int[] { 3, 3, 5, 5, 7, 7 };


```

An important difference from the SQL syntax is that the `Window()` function does not include any sorting. So if you want your partitions to include all items that are the same, you need to make sure the sequence is sorted before applying the window. The boundary functions might depend on a source element being sorted too.

The window functions without a selector works a bit differently. They start a window (`IWindowAggregateEnumerable`) that can be aggregated on by applying aggregate functions directly on the result of the window, and later selected with the `Select()` function.

```c#
var result = Enumerable.Range(1, 10)
    .Window(i => i >= -1, i => i <= 1)
    .Count()
    .Sum()
    .Select((src, count, sum) => Tuple.Create(src, count, sum));
```

This example is the same as the first one, but with the count and sum functions applied to the result of the window instead. All the standard aggregate functions are implemented as extension functions on the `IWindowAggregateEnumerable` interface. There are a few important differences though. The `Aggregate()` takes an accumulator function and optionally a decumulator function. The reason for this is that the window only applies one accumulation for all the aggregates on a window once for each element, and at most one decumulation. Decumulation happens when the start of the window moves forward.

Not all aggregates can easily be decumulated. `Min` and `Max` are examples and they will throw `InvalidOperationsException` if used on a moving window where decumulation is necessary.

The `Average`, `Min` and `Max` functions behaves differently compared to the standard LINQ operators when applied to empty sequences. Instead of throwing, they return a default value of the type operated on. The rationale behind this is that window with boundaries that sometimes result in an empty window on some source elements might be useful and better to allow than result in the iteration aborted in error.

So what is the point of this alterative window syntax if there are only restrictions compared to just using a selector as an argument to the window? Well, it mostly has to do with performance. The window sequence never has to be iterated on separately for each aggregate performed. For a large window with many aggregates (or expensive to compute), this might make a difference. With a moving window, the whole of the window has to be buffered so there is no memory savings in the general case. The real point of this special window though is an optimization that is used if you start the window with the `WindowUnboundedPreceding` function. It has the semantics as a window with a preceding bound functions returning true on all inputs. Since the start of the window never moves forward (except on a partition boundary but then all aggregates are reset), no buffering is needed for elements before the current row, or window end whichever comes first (the window can end before the current row or start after the current row).

The following example illustrates the point above. It calculates a running total for each element without any buffering and can therefore even be used on unending input sequences.

```c#
var result = Enumerable.Range(1, 10000000)
    .WindowUnboundedPreceding(i => i <= 0)
    .Sum()
    .Select((src, sum) => sum)
    .Skip(1000)
    .Take(5);

var expected = new int[] { 501501, 502503, 503506, 504510, 505515 };
```

The aggregates can be individually filtered with arbitrary many filters on each:

```c#
var result = Enumerable.Range(1, 10)
    .WindowUnboundedPreceding(i => i <= 0)
    .Count().Where(s => s % 2 == 0).Where(s => s % 3 == 0) // Accumulated count of every sixth element
    .Select((src, sum) => sum);

var expected = new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1 };

```

Incidentally the last row of a window with no preceding bound happens to include the total aggregates on the whole sequence. Since the standard LINQ operators does not include a simple way of calculating multiple aggregates without iterating multiple times on the sequence I find this quite useful (as an alternative to for example [Push LINQ)( http://msmvps.com/blogs/jon_skeet/archive/2008/01/04/quot-push-quot-linq-revisited-next-attempt-at-an-explanation.aspx)).

```c#
var result = Enumerable.Range(1, 10)
    .WindowUnboundedPreceding(i => i <= 0)
    .Count()
    .Sum()
    .Select((src, count, sum) => Tuple.Create(count, sum))
    .Last();

var expected = Tuple.Create(10, 55);

```

To make this even neater I created an alias for the window function above that is named `BeginAggregate`. I also overloaded the `LastOrDefault` function on a window to instead of just returning a default value for the whole sequence, returning a row with the source as default value and all the aggregates calculated over an empty sequence. Lastly I implemented the `Compute` function that calls `LastOrDefault` and either returns a tuple with all the aggregates or apply a selector function on them. The following example is equivalent to the above except that it also handles empty sequences:

```c#
var result = Enumerable.Range(1, 10)
    .BeginAggregate()
    .Count()
    .Sum()
    .Compute();

var expected = Tuple.Create(10, 55);
```

The window functions use a class called `SinglePassSequence` as a layer of abstraction that can be useful by itself. The purpose of this class is to work as a layer between a single input sequence that you want to iterate on multiple times simultaneously, without passing through the input sequence more than one time. This is where the buffering happens for the window functions. Only the elements between the enumerators that are closest to the start and end of the input sequence will be buffered. You could for example, do something like (imagine that the Range function is really some stream that can only be iterated on once):

```c#
var source = new SinglePassSequence<int>(Enumerable.Range(1, 100));
            
var result = source
    .Zip(source.Skip(1), (l, r) => Tuple.Create(l, r))
    .Zip(source.Skip(2), (l, r) => Tuple.Create(l.Item1, l.Item2, r))
    .Skip(10)
    .Take(2);

var expected = new List<Tuple<int, int, int>> {
        Tuple.Create(11, 12, 13)
        , Tuple.Create(12, 13, 14)
    };

```

Partitioned windows use the `Partition` extension function that can be used separately. This is an operator that takes an input sequence and a partition selector that specifies the key that should be partitioned on. This might sound a bit like the `GroupBy` operator, but it is not. Just like the window functions, it depends on the import sequence already being sorted in the key order unless you want multiple partitions with the same key (which could very well be what you want). Also, while the `Partition` function returns a sequence of sequences (`IEnumerable<IEnumerable<T>>`), the inner sequence can only be iterated on once. This is because there is no buffering of the partitioned sequence involved at all.

## Testing

Unit tests are included as a separate VS 2010 project WindowToLinq.Test. Feel free to check out what’s tested. I wouldn’t call it enough for production level code, but it’s a start. Don’t expect the code to be free of bugs, but I’ll be happy to fix any serious problems if I get the time and anyone finds the library useful.

## Background

I made this library mainly as an educational project. I recently decided to learn C#. I have a background as a C++ / SQL programmer and noticed while reading about LINQ that the very useful window functions from SQL was nowhere to be found.

I have made a couple of trivial projects earlier in C# but mostly been using the language as I would C++ which is not optimal of course, so I saw this as an opportunity to get some practice with the intricacies of C# generic programming and LINQ in general.

The production of the main library was a sometimes frustrating experience in how to produce statically typed generic code that adheres to the functional principle of LINQ by avoiding having any state in the operators themselves. Though I’m not sure I have seen this stated as a requirement for the LINQ operators it seems like the way to go. My reasoning was that even the most complex query that you could produce with my library functions should be possible to execute multiple times, even simultaneously like with the `Zip` function for example, while not iterating through the source sequence more than once for each time my query was executed. This posed some interesting challenges that I’m happy that I managed to overcome and learn a lot from. As a reflection I can’t resist thinking about how much easier and less tedious an implementation in a dynamically typed language like python would be.

When it came to unit testing the whole thing, I went in a complete different way. Now I wasn’t bound by the statically typing restrictions so I could go all out with the dynamic capabilities of C# and .NET (of which I had no experience with beforehand). I learned a lot about dynamic coding with the dynamic keyword, expression trees and reflection. If you look at the unit test you might sometimes wonder why I went to such length of doing something dynamically instead of just duplicating code or writing a code generator for static unit tests which might have produced better test code. My defense is that I had more fun this way and learned more.

As I hinted above my hope is also that Microsoft will implement something like this in the standard LINQ. I would love to see the query comprehension syntax extended in some way to include window support, but I’m not arrogant enough to think I can make a proposal that takes all the ambiguity concerns into consideration. Also I expect the people at Microsoft to already have thought about functionality like this but not thought it important enough to implement yet. With newer SQL servers gaining more window function capabilities it seems more important that LINQ to SQL can take advantage of these often very efficient ways of applying aggregates. Of course it is possible to use them with dynamic SQL queries, but the whole point of LINQ to SQL is as far as I understand to query a database in a statically typed way. Unless I’m mistaken, extending LINQ to SQL seems impossible without access to the source code, or reimplement the whole thing from start.

## License

This software is distributed under the Apache License. See the LICENSE file for more info.

## Thanks

A special thanks goes to [Jon Skeet](http://msmvps.com/blogs/jon_skeet/) whose excellent book C# In Depth and Edulinq blog series helped inspire me to create this library.

