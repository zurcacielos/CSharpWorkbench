using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class MergeSortedArrays
{
    public static void Merge(int[] nums1, int m, int[] nums2, int n)
    {
        int p1 = m - 1, p2 = n - 1, p = m + n - 1;
        while (p2 >= 0)
        {
            if (p1 >= 0 && nums1[p1] > nums2[p2])
                nums1[p--] = nums1[p1--];
            else
                nums1[p--] = nums2[p2--];
        }
    }
}

public class MergeSortedArraysTest
{
    [Fact]
    public void Test_MergeSortedArrays()
    {
        var nums1 = new[] { 1, 2, 3, 0, 0, 0 };
        MergeSortedArrays.Merge(nums1, 3, new[] { 2, 5, 6 }, 3);
        Assert.Equal(new[] { 1, 2, 2, 3, 5, 6 }, nums1);
    }
}
