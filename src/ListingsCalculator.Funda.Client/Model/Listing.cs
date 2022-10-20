using System.Text.Json.Serialization;

namespace ListingsCalculator.Funda.Client.Model;

public class Listing
{
    [JsonPropertyName("MakelaarId")]
    public int RealEstateAgentId { get; set; }
    
    [JsonPropertyName("MakelaarNaam")]
    public string RealEstateAgentName { get; set; } = string.Empty;
}