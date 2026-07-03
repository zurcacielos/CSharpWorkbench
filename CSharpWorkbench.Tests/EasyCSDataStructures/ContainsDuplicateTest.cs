using System.Collections.Generic;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class ContainsDuplicate
{
    public static bool Check(int[] nums)
    {
        var set = new HashSet<int>();
        foreach (var num in nums)
        {
            if (!set.Add(num)) return true;
        }
        return false;
    }
}

public class ContainsDuplicateTest
{
    [Fact]
    public void Test_ContainsDuplicate()
    {
        Assert.True(ContainsDuplicate.Check(new[] { 1, 2, 3, 1 }));
        Assert.False(ContainsDuplicate.Check(new[] { 1, 2, 3, 4 }));
    }
}
