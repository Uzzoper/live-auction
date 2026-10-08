using MediatR;

namespace LiveAuction.Application.Auctions.Commands.CreateAuction;

public record CreateAuctionCommand(
    Guid SellerId,
    string Title,
    decimal StartingPrice,
    decimal MinIncrement,
    DateTime EndsAt) : IRequest<Guid>;