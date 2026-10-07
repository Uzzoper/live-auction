namespace LiveAuction.Domain.Exceptions;

public class SellerCannotBidException() : DomainException("The seller cannot bid on their own auction.");
