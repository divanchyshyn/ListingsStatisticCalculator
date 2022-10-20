using ListingsCalculator.Services;
using ListingsCalculator.Services.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace ListingsCalculator.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddListingsCalculator(this IServiceCollection services) =>
        services
            .AddScoped<IMessageSync, ConsoleMessageSync>()
            .AddScoped<IRealEstateAgentDataProvider, RealEstateAgentDataProvider>()
            .Decorate<IRealEstateAgentDataProvider, RealEstateAgentDataProviderExceptionHandlingDecorator>()
            .AddScoped<IListingsStatisticCalculator, ListingsStatisticCalculator>();
}