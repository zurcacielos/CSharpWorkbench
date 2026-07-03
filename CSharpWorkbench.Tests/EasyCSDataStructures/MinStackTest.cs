using System;
using System.Collections.Generic;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class MinStack
{
    private Stack<int> s = new();
    private Stack<int> minStack = new();

    public void Push(int val)
    {
        s.Push(val);
        if (minStack.Count == 0 || val <= minStack.Peek()) minStack.Push(val);
    }

    public void Pop()
    {
        if (s.Pop() == minStack.Peek()) minStack.Pop();
    }

    public int Top() => s.Peek();
    public int GetMin() => minStack.Peek();
}

public class MinStackTest
{
    [Fact]
    public void Test_MinStack()
    {
        var minStack = new MinStack();
        minStack.Push(-2);
        minStack.Push(0);
        minStack.Push(-3);
        Assert.Equal(-3, minStack.GetMin());
        minStack.Pop();
        Assert.Equal(0, minStack.Top());
        Assert.Equal(-2, minStack.GetMin());
    }
}
