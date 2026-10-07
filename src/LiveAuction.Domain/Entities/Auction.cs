using LiveAuction.Domain.Enums;
using LiveAuction.Domain.Exceptions;

namespace LiveAuction.Domain.Entities;

public class Auction
{
    private readonly List<Bid> _bids = [];
    
    private Auction() { }

    public Auction(
        Guid sellerId,
        string title,
        decimal startingPrice,
        decimal minIncrement,
        DateTime endsAt)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));
        if (startingPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(startingPrice));
        if (minIncrement <= 0)
            throw new ArgumentOutOfRangeException(nameof(minIncrement));

        Id = Guid.NewGuid();
        SellerId = sellerId;
        Title = title;
        StartingPrice = startingPrice;
        MinIncrement = minIncrement;
        EndsAt = endsAt;
        CurrentPrice = startingPrice;
        Status = AuctionStatus.Active;
    }
    
    public Guid Id { get; private set; }
    public Guid SellerId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public decimal StartingPrice { get; private set; }
    public decimal MinIncrement { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public int BidCount { get; private set; }
    public DateTime EndsAt { get; private set; }
    public AuctionStatus Status { get; private set; }
    public byte[] RowVersion { get; private set; } = [];
    
    public IReadOnlyCollection<Bid> Bids => _bids.AsReadOnly();

    public Bid PlaceBid(Guid bidderId, decimal amount, DateTime now)
    {
        if (Status != AuctionStatus.Active || now >= EndsAt) 
            throw new AuctionClosedException();
        
        if (bidderId == SellerId) 
            throw new SellerCannotBidException();
        
        var minimum = BidCount == 0 ? StartingPrice : CurrentPrice + MinIncrement;
        if (amount < minimum)
            throw new BidTooLowException(minimum);
        
        var bid = new Bid(Id, bidderId, amount, now);
        _bids.Add(bid);
        CurrentPrice = amount;
        BidCount++;
        return bid;
    }
}