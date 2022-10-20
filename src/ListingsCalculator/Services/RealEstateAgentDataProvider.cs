using CSharpFunctionalExtensions;
using ListingsCalculator.Extensions;
using ListingsCalculator.Funda.Client;
using ListingsCalculator.Funda.Client.Model;
using ListingsCalculator.Model;

namespace ListingsCalculator.Services;

public class RealEstateAgentDataProvider : IRealEstateAgentDataProvider
{
    private const int PageSize = 25;
    private readonly IFundaClient _fundaClient;

    public RealEstateAgentDataProvider(IFundaClient fundaClient) => 
        _fundaClient = fundaClient;

    public async Task<Result<IEnumerable<RealEstateAgent>>> GetRealEstateAgentsWithMostListings(
        string city,
        int count,
        CancellationToken cancellation) =>
        await ProcessListings(
            count, 
            nextPage => _fundaClient.GetListings(city, nextPage, PageSize, cancellation));

    public async Task<Result<IEnumerable<RealEstateAgent>>> GetRealEstateAgentsWithMostListingsWithGarden(
        string city,
        int count,
        CancellationToken cancellation) =>
        await ProcessListings(
            count,
            nextPage => _fundaClient.GetListingsWithGarden(city, nextPage, PageSize, cancellation));

    private async Task<Result<IEnumerable<RealEstateAgent>>> ProcessListings(int count,
        Func<int, Task<ListingsResponse>> getListings)
    {
        var realEstateAgents = new Dictionary<int, RealEstateAgent>();
        ListingsResponse listingsResponse;
        var nextPage = 1;

        do
        {
            listingsResponse = await getListings.Invoke(nextPage);
            CountListings(listingsResponse.Listings, realEstateAgents);
            nextPage++;
        } while (nextPage <= listingsResponse.Paging.Count);

        return Result.Success(realEstateAgents.SortByListingsCount().Take(count));
    }

    private static void CountListings(
        IEnumerable<Listing> listings,
        IDictionary<int, RealEstateAgent> realEstateAgents)
    {
        foreach (var listing in listings)
        {
            realEstateAgents.IncrementListingsCount(listing);
        }
    }
}