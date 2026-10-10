using MediatR;

namespace LiveAuction.Application.Auctions.Commands.PlaceBid;

public record PlaceBidCommand(Guid AuctionId, Guid BidderId, decimal Amount)
    : IRequest<PlaceBidResult>;

public record PlaceBidResult(Guid BidId, decimal CurrentPrice, int BidCount);
