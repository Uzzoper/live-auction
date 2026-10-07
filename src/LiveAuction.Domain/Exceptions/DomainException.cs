namespace LiveAuction.Domain.Exceptions;

public abstract class DomainException(string message) : Exception(message);
