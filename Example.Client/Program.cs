using Example.Client.Runners;

//Console.WriteLine("BasicRunner:");
//var basicRunner = new BasicRunner
//{
//    Iterations = 5,
//    Delay = 100
//};
//await basicRunner.RunAsync();

//Console.WriteLine();

//Console.WriteLine("StaticResolverRunner:");
//var staticResolverRunner = new StaticResolverRunner()
//{
//    Delay = 5000,
//    Iterations = 30
//};
//await staticResolverRunner.RunAsync();

//Console.WriteLine();

//Console.WriteLine("StaticResolverRandomBalanserRunner:");
//var staticResolverRandomBalanserRunner = new StaticResolverRandomBalanserRunner()
//{
//    Iterations = 20,
//};
//await staticResolverRandomBalanserRunner.RunAsync();

//Console.WriteLine();

//Console.WriteLine("FileResolverRunner:");
//var fileResolverRunner = new FileResolverRunner();
//await fileResolverRunner.RunAsync();

//Console.WriteLine();

//Console.WriteLine("FileResolverRandomBalanserRunner:");
//var fileResolverRandomBalanserRunner = new FileResolverRandomBalanserRunner()
//{
//    Iterations = 20
//};
//await fileResolverRandomBalanserRunner.RunAsync();

//Console.WriteLine();

Console.WriteLine("DiscoveryResolverRunner:");
var discoveryResolverRunner = new DiscoveryResolverRunner()
{
    Iterations = 30,
    Delay = 2000
};
await discoveryResolverRunner.RunAsync();

Console.WriteLine();

Console.Write("Press ENTER...");
Console.ReadLine();
