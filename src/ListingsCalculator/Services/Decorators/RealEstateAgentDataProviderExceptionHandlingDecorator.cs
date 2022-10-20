using CSharpFunctionalExtensions;
using ListingsCalculator.Model;
using Refit;

namespace ListingsCalculator.Services.Decorators;

public class RealEstateAgentDataProviderExceptionHandlingDecorator : IRealEstateAgentDataProvider
{
    private readonly IRealEstateAgentDataProvider _inner;

    public RealEstateAgentDataProviderExceptionHandlingDecorator(IRealEstateAgentDataProvider inner) =>
        _inner = inner;

    public async Task<Result<IEnumerable<RealEstateAgent>>> GetRealEstateAgentsWithMostListings(
        string city,
        int count,
        CancellationToken cancellation) =>
        await HandleException(() => _inner.GetRealEstateAgentsWithMostListings(city, count, cancellation));

    public async Task<Result<IEnumerable<RealEstateAgent>>> GetRealEstateAgentsWithMostListingsWithGarden(
        string city,
        int count,
        CancellationToken cancellation) =>
    await HandleException(() => _inner.GetRealEstateAgentsWithMostListingsWithGarden(city, count, cancellation));

    private static async Task<Result<IEnumerable<RealEstateAgent>>> HandleException(Func<Task<Result<IEnumerable<RealEstateAgent>>>> func)
    {
        try
        {
            return await func.Invoke();
        }
        catch (ApiException ex)
        {
            return Result.Failure<IEnumerable<RealEstateAgent>>(
                $"Failed to fetched real estate agents data.{Environment.NewLine}" +
                $"With reason: {ex.Message}");
        }
    }
}