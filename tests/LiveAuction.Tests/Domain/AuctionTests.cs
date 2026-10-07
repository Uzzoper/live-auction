using LiveAuction.Domain.Entities;
using LiveAuction.Domain.Exceptions;

namespace LiveAuction.Tests.Domain;

public class AuctionTests
{
    private static readonly DateTime Now = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    private static Auction CreateAuction(Guid? sellerId = null) =>
        new(sellerId ?? Guid.NewGuid(), "Vintage watch", 100m, 10m, Now.AddHours(1));

    [Fact]
    public void PlaceBid_BelowStartingPrice_Throws()
    {
        var auction = CreateAuction();

        Assert.Throws<BidTooLowException>(() =>
            auction.PlaceBid(Guid.NewGuid(), 50m, Now));
    }
    
    [Fact]
    public void PlaceBid_AfterEndsAt_Throws()
    {
        var auction = CreateAuction();

        Assert.Throws<AuctionClosedException>(() =>
            auction.PlaceBid(Guid.NewGuid(), 100m, Now.AddHours(2)));
    }

    [Fact]
    public void PlaceBid_BySeller_Throws()
    {
        var sellerId = Guid.NewGuid();
        var auction = CreateAuction(sellerId);

        Assert.Throws<SellerCannotBidException>(() =>
            auction.PlaceBid(sellerId, 100m, Now));
    }

    [Fact]
    public void PlaceBid_Valid_UpdatesCurrentPriceAndBidCount()
    {
        var auction = CreateAuction();

        var bid = auction.PlaceBid(Guid.NewGuid(), 100m, Now);

        Assert.Equal(100m, bid.Amount);
        Assert.Equal(100m, auction.CurrentPrice);
        Assert.Equal(1, auction.BidCount);
        Assert.Single(auction.Bids);
    }

    [Fact]
    public void PlaceBid_SecondBidBelowIncrement_Throws()
    {
        var auction = CreateAuction();
        auction.PlaceBid(Guid.NewGuid(), 100m, Now);

        var ex = Assert.Throws<BidTooLowException>(() =>
            auction.PlaceBid(Guid.NewGuid(), 105m, Now));

        Assert.Equal(110m, ex.Minimum);
    }
}