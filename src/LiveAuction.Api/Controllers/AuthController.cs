using System.Security.Claims;
using LiveAuction.Api.Auth;
using LiveAuction.Api.Contracts;
using LiveAuction.Application.Auth.Commands.Login;
using LiveAuction.Application.Auth.Commands.Register;
using LiveAuction.Infrastructure.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LiveAuction.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    ISender sender,
    IOptions<JwtOptions> jwtOptions,
    TimeProvider clock)  : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request, CancellationToken ct)
    {
        var id = await sender.Send(
            new RegisterCommand(request.Email, request.DisplayName, request.Password), ct);

        return StatusCode(StatusCodes.Status201Created, new { id });
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new LoginCommand(request.Email, request.Password), ct);

        Response.Cookies.Append(AuthCookie.Name, result.Token, new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = clock.GetUtcNow().AddMinutes(jwtOptions.Value.ExpirationMinutes),
            Path = "/"
        });

        return Ok(new LoginResponse(result.UserId, result.DisplayName));
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(AuthCookie.Name);
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
        => Ok(new { id = User.GetUserId(), displayName = User.FindFirstValue("name") });
}