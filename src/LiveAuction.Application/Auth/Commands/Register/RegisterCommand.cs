using MediatR;

namespace LiveAuction.Application.Auth.Commands.Register;

public record RegisterCommand(string Email, string DisplayName, string Password)
    : IRequest<Guid>;
    