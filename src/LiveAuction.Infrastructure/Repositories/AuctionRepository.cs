using LiveAuction.Domain.Entities;
using LiveAuction.Domain.Exceptions;
using LiveAuction.Domain.Repositories;
using LiveAuction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiveAuction.Infrastructure.Repositories;

public class AuctionRepository(AppDbContext context) : IAuctionRepository
{
    public Task<Auction?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => context.Auctions.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Auction auction, CancellationToken ct = default)
        => await context.Auctions.AddAsync(auction, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConcurrencyConflictException();
        }
    }
}