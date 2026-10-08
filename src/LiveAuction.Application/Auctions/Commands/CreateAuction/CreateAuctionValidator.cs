using FluentValidation;

namespace LiveAuction.Application.Auctions.Commands.CreateAuction;

public class CreateAuctionValidator : AbstractValidator<CreateAuctionCommand>
{
    public CreateAuctionValidator()
    {
        RuleFor(x => x.SellerId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.StartingPrice).GreaterThan(0);
        RuleFor(x => x.MinIncrement).GreaterThan(0);
        RuleFor(x => x.EndsAt).GreaterThan(DateTime.UtcNow);
    }
}