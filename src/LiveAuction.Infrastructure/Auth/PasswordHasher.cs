using LiveAuction.Application.Abstractions;
using LiveAuction.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LiveAuction.Infrastructure.Auth;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string password)
        => _hasher.HashPassword(null!, password);

    public bool Verify(string hash, string password)
        => _hasher.VerifyHashedPassword(null!, hash, password) != PasswordVerificationResult.Failed;
}
