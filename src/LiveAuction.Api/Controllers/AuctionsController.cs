using LiveAuction.Api.Contracts;
using LiveAuction.Application.Auctions.Commands.CreateAuction;
using LiveAuction.Application.Auctions.Commands.PlaceBid;
using LiveAuction.Application.Auctions.Dtos;
using LiveAuction.Application.Auctions.Queries.GetAuctionById;
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
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuctionDto>> GetById(Guid id, CancellationToken ct)
        => Ok(await sender.Send(new GetAuctionByIdQuery(id), ct));
    
    [HttpPost("{id:guid}/bids")]
    public async Task<ActionResult<PlaceBidResult>> PlaceBid(
        Guid id, [FromBody] PlaceBidRequest request, CancellationToken ct)
        => Ok(await sender.Send(new PlaceBidCommand(id, request.BidderId, request.Amount), ct));
}