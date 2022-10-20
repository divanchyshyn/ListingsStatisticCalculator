using System.Text.Json.Serialization;

namespace ListingsCalculator.Funda.Client.Model;

public class Paging
{
    [JsonPropertyName("AantalPaginas")]
    public int Count { get; set; }
}