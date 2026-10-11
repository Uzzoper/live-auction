using MediatR;

namespace LiveAuction.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;

public record LoginResult(string Token, Guid UserId, string DisplayName);
