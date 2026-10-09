using LiveAuction.Application.Auctions.Dtos;
using MediatR;

namespace LiveAuction.Application.Auctions.Queries.GetAuctionById;

public record GetAuctionByIdQuery(Guid Id) : IRequest<AuctionDto>;