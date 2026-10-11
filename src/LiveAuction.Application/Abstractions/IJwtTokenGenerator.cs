using LiveAuction.Domain.Entities;

namespace LiveAuction.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}