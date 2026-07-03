using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class GroupAnagrams
{
    public static IList<IList<string>> Group(string[] strs)
    {
        var map = new Dictionary<string, List<string>>();
        foreach (var s in strs)
        {
            var chars = s.ToCharArray();
            Array.Sort(chars);
            var key = new string(chars);
            if (!map.ContainsKey(key)) map[key] = new List<string>();
            map[key].Add(s);
        }
        return map.Values.ToList<IList<string>>();
    }
}

public class GroupAnagramsTest
{
    [Fact]
    public void Test_GroupAnagrams()
    {
        var result = GroupAnagrams.Group(new[] { "eat", "tea", "tan", "ate", "nat", "bat" });
        Assert.Equal(3, result.Count);
    }
}
