using LiveAuction.Domain.Entities;

namespace LiveAuction.Application.Auctions.Dtos;

public record AuctionDto(
    Guid Id,
    Guid SellerId,
    string Title,
    decimal StartingPrice,
    decimal CurrentPrice,
    int BidCount,
    DateTime EndsAt,
    string Status)
{
    public static AuctionDto From(Auction auction) => new(
        auction.Id,
        auction.SellerId,
        auction.Title,
        auction.StartingPrice,
        auction.CurrentPrice,
        auction.BidCount,
        auction.EndsAt,
        auction.Status.ToString());
}