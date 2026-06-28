Console.WriteLine("Welcome to the CSharpWorkbench!");
Console.WriteLine("--------------------------------");
Console.WriteLine("To run specific tests, copy and paste the following commands into the Antigravity terminal:\n");

Console.WriteLine("> Run LINQ Practice tests:");
Console.WriteLine("dotnet test --filter LinqTest\n");

Console.WriteLine("> Run Orders Summary tests:");
Console.WriteLine("dotnet test --filter OrdersSummaryTest\n");

Console.WriteLine("> Run Basic Challenges tests:");
Console.WriteLine("dotnet test --filter BasicChallengesTests\n");

Console.WriteLine("> Run ALL tests:");
Console.WriteLine("dotnet test\n");
