using System.Security.Claims;

namespace LiveAuction.Api.Auth;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
        => Guid.Parse(user.FindFirstValue("sub")
                      ?? throw new InvalidOperationException("Missing 'sub' claim."));
}