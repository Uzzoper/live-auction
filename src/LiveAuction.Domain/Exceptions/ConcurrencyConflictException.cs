namespace LiveAuction.Domain.Exceptions;

public class ConcurrencyConflictException()
    : Exception("The auction was modified by another request. Please retry.");