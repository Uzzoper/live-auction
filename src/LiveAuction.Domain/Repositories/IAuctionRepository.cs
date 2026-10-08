using LiveAuction.Domain.Entities;

namespace LiveAuction.Domain.Repositories;

public interface IAuctionRepository
{
    Task<Auction?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Auction auction, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}