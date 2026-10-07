using LiveAuction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiveAuction.Infrastructure.Persistence.Configurations;

public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
        builder.Property(a => a.StartingPrice).HasPrecision(18, 2);
        builder.Property(a => a.MinIncrement).HasPrecision(18, 2);
        builder.Property(a => a.CurrentPrice).HasPrecision(18, 2);
        builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(a => a.RowVersion).IsRowVersion();

        builder.HasMany(a => a.Bids)
            .WithOne()
            .HasForeignKey(b => b.AuctionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(a => a.Bids)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}