using System.Net;
using System.Net.Http.Json;

namespace LiveAuction.IntegrationTests;

[Collection(IntegrationCollection.Name)]
public class AuthorizationTests(CustomWebApplicationFactory factory)
{
    private record IdResponse(Guid Id);

    [Fact]
    public async Task PlaceBid_WithoutLogin_Returns401()
    {
        var anonymous = factory.CreateClient();

        var response = await anonymous.PostAsJsonAsync(
            $"/auctions/{Guid.NewGuid()}/bids", new { amount = 100 });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task PlaceBid_BySeller_Returns422()
    {
        var (seller, _) = await AuthHelper.CreateAuthenticatedClientAsync(factory);

        var created = await seller.PostAsJsonAsync("/auctions", new
        {
            title = "Seller cannot bid",
            startingPrice = 100,
            minIncrement = 10,
            endsAt = DateTime.UtcNow.AddDays(1)
        });
        created.EnsureSuccessStatusCode();
        var auctionId = (await created.Content.ReadFromJsonAsync<IdResponse>())!.Id;

        var response = await seller.PostAsJsonAsync($"/auctions/{auctionId}/bids", new { amount = 100 });

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }
}
