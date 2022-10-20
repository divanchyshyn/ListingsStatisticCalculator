using ListingsCalculator.Extensions;
using ListingsCalculator.Funda.Client.Extensions;
using ListingsCalculator.Funda.Client.Settings;
using ListingsCalculator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json").Build();

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services
            .AddLogging(loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Warning)
                .AddConsole())
            .AddListingsCalculator()
            .AddFundaClient(configuration.GetSetting<FundaClientSettings>()))
    .Build();

var listingsCalculator = host.Services.GetRequiredService<IListingsStatisticCalculator>();

using var source = new CancellationTokenSource();
await listingsCalculator.Calculate("amsterdam", 10, source.Token);
