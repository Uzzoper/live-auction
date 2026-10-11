using LiveAuction.Application.Abstractions;
using LiveAuction.Domain.Exceptions;
using LiveAuction.Domain.Repositories;
using MediatR;

namespace LiveAuction.Application.Auth.Commands.Login;

public class LoginCommandHandler(
    IUserRepository users,
    IPasswordHasher hasher,
    IJwtTokenGenerator tokens)
    : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await users.GetByEmailAsync(request.Email, ct);

        if (user is null || !hasher.Verify(user.PasswordHash, request.Password))
            throw new InvalidCredentialsException();

        return new LoginResult(tokens.Generate(user), user.Id, user.DisplayName);
    }
}