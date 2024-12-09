using EShop.UI;
using ITOps.Shared;

Console.Title = "EShop";

using var host = Host.CreateDefaultBuilder(args)
    .UseNServiceBus(_ => EShopEndpointConfiguration.Create("EShop.UI"))
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build();

await host.RunAsync();