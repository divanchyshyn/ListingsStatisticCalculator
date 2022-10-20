using ListingsCalculator.Funda.Client.Model;
using ListingsCalculator.Model;

namespace ListingsCalculator.Extensions;

public static class RealEstateAgentDictionaryExtensions
{
    public static IEnumerable<RealEstateAgent> SortByListingsCount(this IDictionary<int, RealEstateAgent> dictionary) =>
        dictionary
            .OrderByDescending(d => d.Value.ListingsCount)
            .Select(i => i.Value);

    public static void IncrementListingsCount(
        this IDictionary<int, RealEstateAgent> dictionary, 
        Listing listing)
    {
        dictionary.TryGetValue(listing.RealEstateAgentId, out var realEstateAgent);
        dictionary[listing.RealEstateAgentId] = UpdateRealEstateAgent(realEstateAgent, listing);
    }

    private static RealEstateAgent UpdateRealEstateAgent(RealEstateAgent? realEstateAgent, Listing listing)
    {
        if (realEstateAgent == null)
            return new RealEstateAgent(listing.RealEstateAgentId, listing.RealEstateAgentName, 1);

        realEstateAgent.IncrementListingsCount();
        return realEstateAgent;
    }
}