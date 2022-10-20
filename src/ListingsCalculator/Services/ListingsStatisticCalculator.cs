using CSharpFunctionalExtensions;
using ListingsCalculator.Model;

namespace ListingsCalculator.Services;

public class ListingsStatisticCalculator : IListingsStatisticCalculator
{
    private const string InitialMessage = "Getting the data. It might take a couple of minutes. Please be patient... or check our premium  plans on IDontWantToBeThrottledByFunda.com";
    private const string MostListingsTitle = "Real estate agents with the most listings on Funda";
    private const string MostListingsWithGardenTitle = "Real estate agents with the most listings on Funda with gardens";

    private readonly IRealEstateAgentDataProvider _realEstateAgentDataProvider;
    private readonly IMessageSync _messageSync;

    public ListingsStatisticCalculator(
        IRealEstateAgentDataProvider realEstateAgentDataProvider,
        IMessageSync messageSync)
    {
        _realEstateAgentDataProvider = realEstateAgentDataProvider;
        _messageSync = messageSync;
    }

    public async Task Calculate(string city, int count, CancellationToken cancellation)
    {
        _messageSync.Send(InitialMessage);

        var agentsWithMostListingsResult = await _realEstateAgentDataProvider
            .GetRealEstateAgentsWithMostListings(city, count, cancellation);
        var agentsWithMostListingsWithGardenResult = await _realEstateAgentDataProvider
            .GetRealEstateAgentsWithMostListingsWithGarden(city, count, cancellation);

        var result = Result.Combine(agentsWithMostListingsResult, agentsWithMostListingsWithGardenResult);
        if (result.IsFailure)
        {
            _messageSync.Send(result.Error);
            return;
        }

        PrintResults(agentsWithMostListingsResult.Value, $"{MostListingsTitle} in {city}");
        PrintResults(agentsWithMostListingsWithGardenResult.Value, $"{MostListingsWithGardenTitle} in {city}");
    }

    private void PrintResults(IEnumerable<RealEstateAgent> realEstateAgents, string title)
    {
        _messageSync.Send(string.Empty);
        _messageSync.Send(title);
        foreach (var realEstateAgent in realEstateAgents)
        {
            _messageSync.Send($"{realEstateAgent.Id} {realEstateAgent.Name}: {realEstateAgent.ListingsCount}");
        }
    }
}