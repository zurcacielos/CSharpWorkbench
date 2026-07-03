using System;
using System.Linq;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class ReverseWordsInAString
{
    public static string ReverseWords(string s)
    {
        var words = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Array.Reverse(words);
        return string.Join(" ", words);
    }
}

public class ReverseWordsInAStringTest
{
    [Fact]
    public void Test_ReverseWords()
    {
        Assert.Equal("blue is sky the", ReverseWordsInAString.ReverseWords("the sky is blue"));
        Assert.Equal("world hello", ReverseWordsInAString.ReverseWords("  hello world  "));
    }
}
