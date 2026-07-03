using System.Collections.Generic;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class FirstNonRepeatingCharacter
{
    public static int Find(string s)
    {
        var counts = new Dictionary<char, int>();
        foreach (var c in s) counts[c] = counts.GetValueOrDefault(c) + 1;
        for (int i = 0; i < s.Length; i++)
        {
            if (counts[s[i]] == 1) return i;
        }
        return -1;
    }
}

public class FirstNonRepeatingCharacterTest
{
    [Fact]
    public void Test_FirstNonRepeatingCharacter()
    {
        Assert.Equal(0, FirstNonRepeatingCharacter.Find("leetcode"));
        Assert.Equal(2, FirstNonRepeatingCharacter.Find("loveleetcode"));
        Assert.Equal(-1, FirstNonRepeatingCharacter.Find("aabb"));
    }
}
