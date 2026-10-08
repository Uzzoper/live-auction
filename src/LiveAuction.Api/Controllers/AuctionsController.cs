using LiveAuction.Application.Auctions.Commands.CreateAuction;
using LiveAuction.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LiveAuction.Api.Controllers;

[ApiController]
[Route("auctions")]
public class AuctionsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAuctionCommand command, CancellationToken ct)
    {
        var id = await sender.Send(command, ct);
        return Created($"/auctions/{id}", new { id });
    }
}