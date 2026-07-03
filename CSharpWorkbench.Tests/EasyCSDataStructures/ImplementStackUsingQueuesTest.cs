using System.Collections.Generic;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class MyStack
{
    private Queue<int> q = new();

    public void Push(int x)
    {
        q.Enqueue(x);
        int sz = q.Count;
        while (sz > 1)
        {
            q.Enqueue(q.Dequeue());
            sz--;
        }
    }

    public int Pop() => q.Dequeue();
    public int Top() => q.Peek();
    public bool Empty() => q.Count == 0;
}

public class ImplementStackUsingQueuesTest
{
    [Fact]
    public void Test_StackUsingQueues()
    {
        var s = new MyStack();
        s.Push(1);
        s.Push(2);
        Assert.Equal(2, s.Top());
        Assert.Equal(2, s.Pop());
        Assert.False(s.Empty());
    }
}
