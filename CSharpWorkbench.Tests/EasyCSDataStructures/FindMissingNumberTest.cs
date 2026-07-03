using System.Linq;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class FindMissingNumber
{
    public static int MissingNumber(int[] nums)
    {
        int n = nums.Length;
        int expectedSum = n * (n + 1) / 2;
        int actualSum = nums.Sum();
        return expectedSum - actualSum;
    }
}

public class FindMissingNumberTest
{
    [Fact]
    public void Test_MissingNumber()
    {
        Assert.Equal(2, FindMissingNumber.MissingNumber(new[] { 3, 0, 1 }));
        Assert.Equal(2, FindMissingNumber.MissingNumber(new[] { 0, 1 }));
        Assert.Equal(8, FindMissingNumber.MissingNumber(new[] { 9, 6, 4, 2, 3, 5, 7, 0, 1 }));
    }
}
