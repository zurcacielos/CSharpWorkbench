using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class TopKFrequentElements
{
    public static int[] TopK(int[] nums, int k)
    {
        return nums.GroupBy(x => x)
                   .OrderByDescending(g => g.Count())
                   .Take(k)
                   .Select(g => g.Key)
                   .ToArray();
    }
}

public class TopKFrequentElementsTest
{
    [Fact]
    public void Test_TopKFrequentElements()
    {
        Assert.Equal(new[] { 1, 2 }, TopKFrequentElements.TopK(new[] { 1, 1, 1, 2, 2, 3 }, 2));
    }
}
