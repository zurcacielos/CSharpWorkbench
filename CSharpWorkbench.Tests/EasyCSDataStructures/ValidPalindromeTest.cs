using System;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class ValidPalindrome
{
    public static bool IsPalindrome(string s)
    {
        int l = 0, r = s.Length - 1;
        while (l < r)
        {
            if (!char.IsLetterOrDigit(s[l])) l++;
            else if (!char.IsLetterOrDigit(s[r])) r--;
            else if (char.ToLower(s[l]) != char.ToLower(s[r])) return false;
            else { l++; r--; }
        }
        return true;
    }
}

public class ValidPalindromeTest
{
    [Fact]
    public void Test_ValidPalindrome()
    {
        Assert.True(ValidPalindrome.IsPalindrome("A man, a plan, a canal: Panama"));
        Assert.False(ValidPalindrome.IsPalindrome("race a car"));
    }
}
