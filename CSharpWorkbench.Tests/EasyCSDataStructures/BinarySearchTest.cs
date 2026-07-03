using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class BinarySearch
{
    public static int Search(int[] nums, int target)
    {
        int left = 0, right = nums.Length - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (nums[mid] == target) return mid;
            if (nums[mid] < target) left = mid + 1;
            else right = mid - 1;
        }
        return -1;
    }
}

public class BinarySearchTest
{
    [Fact]
    public void Test_BinarySearch()
    {
        Assert.Equal(4, BinarySearch.Search(new[] { -1, 0, 3, 5, 9, 12 }, 9));
        Assert.Equal(-1, BinarySearch.Search(new[] { -1, 0, 3, 5, 9, 12 }, 2));
    }
}
