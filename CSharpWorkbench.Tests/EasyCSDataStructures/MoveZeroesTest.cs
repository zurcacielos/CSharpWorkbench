using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class MoveZeroes
{
    public static void Move(int[] nums)
    {
        int lastNonZeroFoundAt = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] != 0)
            {
                nums[lastNonZeroFoundAt++] = nums[i];
            }
        }
        for (int i = lastNonZeroFoundAt; i < nums.Length; i++)
        {
            nums[i] = 0;
        }
    }
}

public class MoveZeroesTest
{
    [Fact]
    public void Test_MoveZeroes()
    {
        var nums = new[] { 0, 1, 0, 3, 12 };
        MoveZeroes.Move(nums);
        Assert.Equal(new[] { 1, 3, 12, 0, 0 }, nums);
    }
}
