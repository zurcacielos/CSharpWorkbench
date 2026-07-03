using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class RemoveDuplicatesFromSortedArray
{
    public static int RemoveDuplicates(int[] nums)
    {
        if (nums.Length == 0) return 0;
        int i = 0;
        for (int j = 1; j < nums.Length; j++)
        {
            if (nums[j] != nums[i])
            {
                i++;
                nums[i] = nums[j];
            }
        }
        return i + 1;
    }
}

public class RemoveDuplicatesFromSortedArrayTest
{
    [Fact]
    public void Test_RemoveDuplicates()
    {
        var nums = new[] { 1, 1, 2 };
        int k = RemoveDuplicatesFromSortedArray.RemoveDuplicates(nums);
        Assert.Equal(2, k);
        Assert.Equal(1, nums[0]);
        Assert.Equal(2, nums[1]);
    }
}
