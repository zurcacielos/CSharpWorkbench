using System.Collections.Generic;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class ValidParentheses
{
    public static bool IsValid(string s)
    {
        var stack = new Stack<char>();
        foreach (var c in s)
        {
            if (c == '(' || c == '{' || c == '[') stack.Push(c);
            else 
            {
                if (stack.Count == 0) return false;
                var top = stack.Pop();
                if (c == ')' && top != '(') return false;
                if (c == '}' && top != '{') return false;
                if (c == ']' && top != '[') return false;
            }
        }
        return stack.Count == 0;
    }
}

public class ValidParenthesesTest
{
    [Fact]
    public void Test_ValidParentheses()
    {
        Assert.True(ValidParentheses.IsValid("()"));
        Assert.True(ValidParentheses.IsValid("()[]{}"));
        Assert.False(ValidParentheses.IsValid("(]"));
    }
}
