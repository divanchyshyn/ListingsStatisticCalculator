using CSharpFunctionalExtensions;
using ListingsCalculator.Model;

namespace ListingsCalculator.Services;

public interface IRealEstateAgentDataProvider
{
    Task<Result<IEnumerable<RealEstateAgent>>> GetRealEstateAgentsWithMostListings(
        string city,
        int count,
        CancellationToken cancellation);

    Task<Result<IEnumerable<RealEstateAgent>>> GetRealEstateAgentsWithMostListingsWithGarden(
        string city,
        int count,
        CancellationToken cancellation);
}