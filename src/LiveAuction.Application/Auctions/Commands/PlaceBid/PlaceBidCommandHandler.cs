using LiveAuction.Domain.Exceptions;
using LiveAuction.Domain.Repositories;
using MediatR;

namespace LiveAuction.Application.Auctions.Commands.PlaceBid;

public class PlaceBidCommandHandler(IAuctionRepository auctions, TimeProvider clock)
    : IRequestHandler<PlaceBidCommand, PlaceBidResult>
{
    public async Task<PlaceBidResult> Handle(PlaceBidCommand request, CancellationToken ct)
    {
        var auction = await auctions.GetByIdAsync(request.AuctionId, ct)
                      ?? throw new AuctionNotFoundException(request.AuctionId);

        var bid = auction.PlaceBid(
            request.BidderId,
            request.Amount,
            clock.GetUtcNow().UtcDateTime);

        await auctions.SaveChangesAsync(ct);

        return new PlaceBidResult(bid.Id, auction.CurrentPrice, auction.BidCount);
    }
}
