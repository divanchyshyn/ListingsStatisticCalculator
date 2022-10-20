using System.Net;
using ListingsCalculator.Funda.Client.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Refit;

namespace ListingsCalculator.Funda.Client.Extensions;

public static class FundaClientExtensions
{
    public static IServiceCollection AddFundaClient(
        this IServiceCollection services,
        FundaClientSettings clientSettings)
    {
        services.AddRefitClient<IFundaClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(clientSettings.BaseUrl);
            })
            .AddPolicyHandler((serviceProvider, _) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<IFundaClient>>();
                var retryPolicy = GetRetryPolicy(logger);
                retryPolicy.WrapAsync(GetCircuitBreaker());

                return retryPolicy;
            });

        return services;
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(r =>
                r.StatusCode is 
                    HttpStatusCode.TooManyRequests 
                    or HttpStatusCode.ServiceUnavailable
                    or HttpStatusCode.Unauthorized)
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt) * 10),
                (result, delay,retry, _ ) =>
                    logger.Log(LogLevel.Warning, $"Failed to fetch data. Failure status code {result.Result.StatusCode}.{Environment.NewLine}" +
                                                 $"Retry attempt number {retry}. Will retry in {delay.Seconds} seconds"));

    private static AsyncCircuitBreakerPolicy GetCircuitBreaker() =>
        Policy.Handle<Exception>()
            .CircuitBreakerAsync(10, TimeSpan.FromSeconds(10));
}