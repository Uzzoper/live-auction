using LiveAuction.Application.Abstractions;
using LiveAuction.Domain.Entities;
using LiveAuction.Domain.Exceptions;
using LiveAuction.Domain.Repositories;
using MediatR;

namespace LiveAuction.Application.Auth.Commands.Register;

public class RegisterCommandHandler(IUserRepository users, IPasswordHasher hasher)
    : IRequestHandler<RegisterCommand, Guid>
{
    public async Task<Guid> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (await users.ExistsByEmailAsync(request.Email, ct))
            throw new EmailAlreadyInUseException();

        var user = new User(request.Email, request.DisplayName, hasher.Hash(request.Password));

        await users.AddAsync(user, ct);
        await users.SaveChangesAsync(ct);

        return user.Id;
    }
}
