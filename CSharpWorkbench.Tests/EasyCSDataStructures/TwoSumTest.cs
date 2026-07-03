using System;
using System.Collections.Generic;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class TwoSum
{
    public static int[] FindTwoSum(int[] nums, int target)
    {
        var map = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            int complement = target - nums[i];
            if (map.ContainsKey(complement))
            {
                return new[] { map[complement], i };
            }
            map[nums[i]] = i;
        }
        return Array.Empty<int>();
    }
}

public class TwoSumTest
{
    [Fact]
    public void Test_TwoSum()
    {
        Assert.Equal(new[] { 0, 1 }, TwoSum.FindTwoSum(new[] { 2, 7, 11, 15 }, 9));
        Assert.Equal(new[] { 1, 2 }, TwoSum.FindTwoSum(new[] { 3, 2, 4 }, 6));
        Assert.Empty(TwoSum.FindTwoSum(new[] { 1, 2 }, 10));
    }
}
