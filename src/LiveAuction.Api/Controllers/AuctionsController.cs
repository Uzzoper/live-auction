using LiveAuction.Api.Auth;
using LiveAuction.Api.Contracts;
using LiveAuction.Application.Auctions.Commands.CreateAuction;
using LiveAuction.Application.Auctions.Commands.PlaceBid;
using LiveAuction.Application.Auctions.Dtos;
using LiveAuction.Application.Auctions.Queries.GetAuctionById;
using LiveAuction.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveAuction.Api.Controllers;

[ApiController]
[Route("auctions")]
public class AuctionsController(ISender sender) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CreateAuctionRequest request, CancellationToken ct)
    {
        var id = await sender.Send(new CreateAuctionCommand(
            User.GetUserId(),
            request.Title,
            request.StartingPrice,
            request.MinIncrement,
            request.EndsAt), ct);

        return Created($"/auctions/{id}", new { id });
    }

    [HttpPost("{id:guid}/bids")]
    [Authorize]
    public async Task<ActionResult<PlaceBidResult>> PlaceBid(
        Guid id, [FromBody] PlaceBidRequest request, CancellationToken ct)
        => Ok(await sender.Send(new PlaceBidCommand(id, User.GetUserId(), request.Amount), ct));
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuctionDto>> GetById(Guid id, CancellationToken ct)
        => Ok(await sender.Send(new GetAuctionByIdQuery(id), ct));
    
}