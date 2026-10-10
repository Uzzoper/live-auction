namespace LiveAuction.Api.Contracts;

public record PlaceBidRequest(Guid BidderId, decimal Amount);