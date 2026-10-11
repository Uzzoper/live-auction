namespace LiveAuction.Domain.Exceptions;

public class EmailAlreadyInUseException()
    : Exception("This email is already registered.");
    