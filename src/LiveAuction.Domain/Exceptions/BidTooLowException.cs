namespace LiveAuction.Domain.Exceptions;

public class BidTooLowException(decimal minimum) : DomainException($"The bid must be at least {minimum}.")
{
    public decimal Minimum { get; } = minimum;
}
