using System;
using Xunit;

namespace CSharpWorkbench.Tests.EasyCSDataStructures;

public class BestTimeToBuyAndSellStock
{
    public static int MaxProfit(int[] prices)
    {
        int minPrice = int.MaxValue;
        int maxProfit = 0;
        foreach (int price in prices)
        {
            if (price < minPrice) minPrice = price;
            else if (price - minPrice > maxProfit) maxProfit = price - minPrice;
        }
        return maxProfit;
    }
}

public class BestTimeToBuyAndSellStockTest
{
    [Fact]
    public void Test_MaxProfit()
    {
        Assert.Equal(5, BestTimeToBuyAndSellStock.MaxProfit(new[] { 7, 1, 5, 3, 6, 4 }));
        Assert.Equal(0, BestTimeToBuyAndSellStock.MaxProfit(new[] { 7, 6, 4, 3, 1 }));
    }
}
