using System.Collections.Generic;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class MovingAverage
{
    private Queue<int> _queue = new();
    private int _size;
    private double _sum = 0;

    public MovingAverage(int size)
    {
        _size = size;
    }

    public double Next(int val)
    {
        if (_queue.Count == _size)
        {
            _sum -= _queue.Dequeue();
        }
        _queue.Enqueue(val);
        _sum += val;
        return _sum / _queue.Count;
    }
}

public class MovingAverageFromDataStreamTest
{
    [Fact]
    public void Test_MovingAverage()
    {
        var m = new MovingAverage(3);
        Assert.Equal(1.0, m.Next(1));
        Assert.Equal(5.5, m.Next(10));
        Assert.Equal(4.666666666666667, m.Next(3), 5);
        Assert.Equal(6.0, m.Next(5));
    }
}
