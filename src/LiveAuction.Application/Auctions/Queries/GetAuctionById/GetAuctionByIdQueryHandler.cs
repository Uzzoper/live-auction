using LiveAuction.Application.Auctions.Dtos;
using LiveAuction.Domain.Exceptions;
using LiveAuction.Domain.Repositories;
using MediatR;

namespace LiveAuction.Application.Auctions.Queries.GetAuctionById;

public class GetAuctionByIdQueryHandler(IAuctionRepository auctions)
    : IRequestHandler<GetAuctionByIdQuery, AuctionDto>
{
    public async Task<AuctionDto> Handle(GetAuctionByIdQuery request, CancellationToken ct)
    {
        var auction = await auctions.GetByIdAsync(request.Id, ct)
                      ?? throw new AuctionNotFoundException(request.Id);

        return AuctionDto.From(auction);
    }
}
