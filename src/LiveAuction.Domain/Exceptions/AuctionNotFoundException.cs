namespace LiveAuction.Domain.Exceptions;

public class AuctionNotFoundException(Guid id)
    : Exception($"Auction {id} was not found.");