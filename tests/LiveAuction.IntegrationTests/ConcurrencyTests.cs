using System.Net;
using System.Net.Http.Json;
using LiveAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace LiveAuction.IntegrationTests;

public class ConcurrencyTests(CustomWebApplicationFactory factory, ITestOutputHelper output)
    : IClassFixture<CustomWebApplicationFactory>
{
    private record IdResponse(Guid Id);
    private record AuctionResponse(decimal CurrentPrice, int BidCount);

    [Fact]
    public async Task ConcurrentBids_WithSameAmount_ProduceExactlyOneWinner()
    {
        var client = factory.CreateClient();

        var created = await client.PostAsJsonAsync("/auctions", new
        {
            sellerId = Guid.NewGuid(),
            title = "Concurrency test",
            startingPrice = 100,
            minIncrement = 10,
            endsAt = DateTime.UtcNow.AddDays(1)
        });
        created.EnsureSuccessStatusCode();
        var auctionId = (await created.Content.ReadFromJsonAsync<IdResponse>())!.Id;

        const int bidders = 20;
        var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var tasks = Enumerable.Range(0, bidders).Select(async _ =>
        {
            await gate.Task;
            return await client.PostAsJsonAsync($"/auctions/{auctionId}/bids", new
            {
                bidderId = Guid.NewGuid(),
                amount = 200
            });
        }).ToList();

        gate.SetResult();
        var responses = await Task.WhenAll(tasks);
        var statuses = responses.Select(r => r.StatusCode).ToList();
        
        foreach (var group in statuses.GroupBy(s => s))
            output.WriteLine($"{(int)group.Key} {group.Key}: {group.Count()}");

        Assert.Single(statuses, s => s == HttpStatusCode.OK);
        Assert.All(
            statuses.Where(s => s != HttpStatusCode.OK),
            s => Assert.True(s is HttpStatusCode.Conflict or HttpStatusCode.UnprocessableEntity));

        var auction = await client.GetFromJsonAsync<AuctionResponse>($"/auctions/{auctionId}");
        Assert.Equal(200m, auction!.CurrentPrice);
        Assert.Equal(1, auction.BidCount);

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Assert.Equal(1, await db.Bids.CountAsync(b => b.AuctionId == auctionId));
    }
}
