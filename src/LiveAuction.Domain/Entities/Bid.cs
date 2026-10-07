namespace  LiveAuction.Domain.Entities;

public class Bid
{
    private Bid() {}

    internal Bid(Guid auctionId, Guid bidderId, decimal amount, DateTime placedAt)
    {
        Id = Guid.NewGuid();
        AuctionId = auctionId;
        BidderId = bidderId;
        Amount = amount;
        PlacedAt = placedAt;
    }
    
    public Guid Id { get; private set; }
    public Guid AuctionId { get; private set; }
    public Guid BidderId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PlacedAt { get; private set; }
}