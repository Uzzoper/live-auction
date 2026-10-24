namespace LiveAuction.Api.Contracts;

public record CreateAuctionRequest(
    string Title,
    decimal StartingPrice,
    decimal MinIncrement,
    DateTime EndsAt);
    