using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class IntersectionOfTwoArrays
{
    public static int[] Intersection(int[] nums1, int[] nums2)
    {
        var set1 = new HashSet<int>(nums1);
        var set2 = new HashSet<int>(nums2);
        set1.IntersectWith(set2);
        return set1.ToArray();
    }
}

public class IntersectionOfTwoArraysTest
{
    [Fact]
    public void Test_Intersection()
    {
        var res = IntersectionOfTwoArrays.Intersection(new[] { 1, 2, 2, 1 }, new[] { 2, 2 });
        Assert.Single(res);
        Assert.Equal(2, res[0]);
    }
}
