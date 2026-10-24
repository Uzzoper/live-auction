namespace LiveAuction.IntegrationTests;

[CollectionDefinition(Name)]
public class IntegrationCollection : ICollectionFixture<CustomWebApplicationFactory>
{
    public const string Name = "Integration";
}
