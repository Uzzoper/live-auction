using LiveAuction.Domain.Entities;
using LiveAuction.Domain.Repositories;
using MediatR;

namespace LiveAuction.Application.Auctions.Commands.CreateAuction;

public class CreateAuctionCommandHandler(IAuctionRepository auctions)
    : IRequestHandler<CreateAuctionCommand, Guid>
{
    public async Task<Guid> Handle(CreateAuctionCommand request, CancellationToken ct)
    {
        var auction = new Auction(
            request.SellerId,
            request.Title,
            request.StartingPrice,
            request.MinIncrement,
            request.EndsAt);
        
        await auctions.AddAsync(auction, ct);
        await auctions.SaveChangesAsync(ct);
        
        return auction.Id;
    }
}