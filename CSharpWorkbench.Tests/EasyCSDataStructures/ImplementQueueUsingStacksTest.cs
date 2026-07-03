using System.Collections.Generic;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class MyQueue
{
    private Stack<int> s1 = new();
    private Stack<int> s2 = new();

    public void Push(int x) => s1.Push(x);

    public int Pop()
    {
        Peek();
        return s2.Pop();
    }

    public int Peek()
    {
        if (s2.Count == 0)
        {
            while (s1.Count > 0) s2.Push(s1.Pop());
        }
        return s2.Peek();
    }

    public bool Empty() => s1.Count == 0 && s2.Count == 0;
}

public class ImplementQueueUsingStacksTest
{
    [Fact]
    public void Test_QueueUsingStacks()
    {
        var q = new MyQueue();
        q.Push(1);
        q.Push(2);
        Assert.Equal(1, q.Peek());
        Assert.Equal(1, q.Pop());
        Assert.False(q.Empty());
    }
}
