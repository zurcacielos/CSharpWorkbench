using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class MergeIntervals
{
    public static int[][] Merge(int[][] intervals)
    {
        if (intervals.Length <= 1) return intervals;
        Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0]));
        var result = new List<int[]>();
        var current = intervals[0];
        result.Add(current);
        foreach (var interval in intervals)
        {
            if (interval[0] <= current[1])
            {
                current[1] = Math.Max(current[1], interval[1]);
            }
            else
            {
                current = interval;
                result.Add(current);
            }
        }
        return result.ToArray();
    }
}

public class MergeIntervalsTest
{
    [Fact]
    public void Test_MergeIntervals()
    {
        var intervals = new[] { new[] { 1, 3 }, new[] { 2, 6 }, new[] { 8, 10 }, new[] { 15, 18 } };
        var merged = MergeIntervals.Merge(intervals);
        Assert.Equal(3, merged.Length);
        Assert.Equal(new[] { 1, 6 }, merged[0]);
    }
}
