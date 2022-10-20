using System.Text.Json.Serialization;

namespace ListingsCalculator.Funda.Client.Model;

public class ListingsResponse
{
    public Paging Paging { get; set; } = new();    
    
    [JsonPropertyName("TotaalAantalObjecten")]
    public int Count { get; set; }

    [JsonPropertyName("Objects")]
    public Listing[] Listings { get; set; } = {};
}