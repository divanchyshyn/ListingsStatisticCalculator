using ListingsCalculator.Funda.Client.Model;
using Refit;

namespace ListingsCalculator.Funda.Client;

public interface IFundaClient
{
    [Get("/?pagesize={pageSize}&page={page}&zo=/{city}/&type=koop")]
    [QueryUriFormat(UriFormat.Unescaped)]
    Task<ListingsResponse> GetListings(
        string city, 
        int page, 
        int pageSize,
        CancellationToken cancellationToken);

    [Get("/?pagesize={pageSize}&page={page}&zo=/{city}/tuin/&type=koop")]
    [QueryUriFormat(UriFormat.Unescaped)]
    Task<ListingsResponse> GetListingsWithGarden(
        string city,
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}