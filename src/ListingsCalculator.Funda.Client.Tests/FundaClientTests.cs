using System.Net;
using System.Text.Json;
using FluentAssertions;
using ListingsCalculator.Funda.Client.Model;
using Refit;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace ListingsCalculator.Funda.Client.Tests;

public sealed class FundaClientTests : IDisposable
{
    private const int ItemsCount = 99;
    private readonly WireMockServer _server;
    private readonly IFundaClient _client;

    public FundaClientTests()
    {
        _server = WireMockServer.Start();

        var refitSettings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }),
        };

        _client = RestService
            .For<IFundaClient>($"{_server.Urls[0]}", refitSettings);
    }

    [Fact]
    public async Task GetListings()
    {
        MockGet("/");

        var result = await _client.GetListings("amsterdam", 1, 25, CancellationToken.None);

        result.Should().NotBeNull();
        result.Count.Should().Be(ItemsCount);
    }

    [Fact]
    public async Task GetListingsWithGarden()
    {
        MockGet("/");

        var result= await _client.GetListingsWithGarden("amsterdam", 1, 25, CancellationToken.None);

        result.Should().NotBeNull();
        result.Count.Should().Be(ItemsCount);
    }

    private void MockGet(string path)
    {
        _server
            .Given(Request
                .Create()
                .WithPath(path)
                .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBody(JsonSerializer.Serialize(new ListingsResponse
                    {
                        Count = ItemsCount
                    })));
    }

    public void Dispose()
    {
        _server.Stop();
        _server.Dispose();
    }
}