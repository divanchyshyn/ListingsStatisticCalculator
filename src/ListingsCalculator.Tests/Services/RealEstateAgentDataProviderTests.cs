using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using ListingsCalculator.Funda.Client;
using ListingsCalculator.Funda.Client.Model;
using ListingsCalculator.Model;
using ListingsCalculator.Services;
using Moq;

namespace ListingsCalculator.Tests.Services
{
    public class RealEstateAgentDataProviderTests
    {
        [Theory]
        [MemberData(nameof(TestData))]
        public async Task GetRealEstateAgentsWithMostListingsReturnsExpected(
            int count, 
            ListingsResponse fundaClientResponse,
            RealEstateAgent[] expectedResult)
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            SetupFundaClientMock(fundaClientResponse, fixture);
            var provider = fixture.Create<RealEstateAgentDataProvider>();

            var result = await provider.GetRealEstateAgentsWithMostListings(fixture.Create<string>(), count, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            var realEstateAgents = result.Value.ToArray();
            realEstateAgents.Should().HaveCount(count);
            for (var i = 0; i < count; i++)
            {
                realEstateAgents[i].Should().BeEquivalentTo(expectedResult[i]);
            }
        }

        private static void SetupFundaClientMock(ListingsResponse fundaClientResponse, Fixture fixture)
        {
            var fundaClientMock = fixture.Freeze<Mock<IFundaClient>>();
            fundaClientMock
                .Setup(m => m.GetListings(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None))
                .ReturnsAsync(fundaClientResponse);
        }

        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] {
                0,
                new ListingsResponse
                {
                    Count = 1,
                    Listings = new[]
                    {
                        CreateListing(1, "name1"),
                    }
                },
                new RealEstateAgent[] { }
            };

            yield return new object[] {
                1,
                new ListingsResponse
                {
                    Count = 4,
                    Listings = new[]
                    {
                        CreateListing(1, "name1"),
                        CreateListing(1, "name1"),
                        CreateListing(2, "name2"),
                        CreateListing(3, "name3"),
                    }
                },
                new RealEstateAgent[]
                {
                    new(1, "name1", 2),
                }
            };

            yield return new object[] {
                3,
                new ListingsResponse
                {
                    Count = 10,
                    Listings = new[]
                    {
                        CreateListing(1, "name1"),
                        CreateListing(1, "name1"),
                        CreateListing(1, "name1"),
                        CreateListing(2, "name2"),
                        CreateListing(2, "name2"),
                        CreateListing(3, "name3"),
                        CreateListing(3, "name3"),
                        CreateListing(4, "name4"),
                        CreateListing(5, "name5"),
                    }
                },
                new RealEstateAgent[]
                {
                    new(1, "name1", 3),
                    new(2, "name2", 2),
                    new(3, "name3", 2)
                }
            };

        }

        private static Listing CreateListing(int id, string name) =>
            new()
            {
                RealEstateAgentId = id,
                RealEstateAgentName = name
            };
    }
}