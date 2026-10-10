# Live Auction

Real-time auction API built with ASP.NET Core 10, Clean Architecture and CQRS.
The core problem it solves: many bidders placing bids on the same auction at the same time, without ever accepting two winners.

## Features

- Create auctions and query them by ID
- Place bids with business rules enforced in the domain (minimum increment, closed auction, seller cannot bid)
- Optimistic concurrency on the auction row, proven by an integration test against SQL Server
- Input validation in the MediatR pipeline and a global exception handler returning `ProblemDetails`

## Tech stack

- ASP.NET Core 10 (controllers)
- MediatR (CQRS) and FluentValidation
- Entity Framework Core with SQL Server
- OpenAPI with Scalar
- xUnit and `WebApplicationFactory`
- Docker Compose for the local database

## Architecture

```
src/
├── LiveAuction.Api              # Controllers, exception handling, Program
├── LiveAuction.Application      # Commands, queries, handlers, validators, DTOs
├── LiveAuction.Domain           # Entities, domain exceptions, repository contracts
└── LiveAuction.Infrastructure   # EF Core, migrations, repository implementations
tests/
├── LiveAuction.Tests            # Domain unit tests
└── LiveAuction.IntegrationTests # Concurrency test against a real database
```

Business rules live in the `Auction` entity (`PlaceBid`), not in services or controllers.
Handlers only orchestrate: load the aggregate, call the domain, save.

## Getting started

Requirements: [.NET 10 SDK](https://dotnet.microsoft.com/download) and Docker.

```bash
git clone https://github.com/Uzzoper/live-auction.git
cd live-auction

# SQL Server (bound to localhost only)
docker compose up -d

# Create the database
dotnet ef database update \
  --project src/LiveAuction.Infrastructure \
  --startup-project src/LiveAuction.Api

# Run the API
dotnet run --project src/LiveAuction.Api
```

Open the API reference at `http://localhost:5243/scalar/v1`.

The connection string and SA password in `appsettings.Development.json` and `docker-compose.yaml` are for local development only.

## API

| Method | Route | Description | Responses |
|---|---|---|---|
| POST | `/auctions` | Create an auction | 201, 400 |
| GET | `/auctions/{id}` | Get an auction | 200, 404 |
| POST | `/auctions/{id}/bids` | Place a bid | 200, 400, 404, 409, 422 |

Status codes:

- `400` invalid input (validation)
- `404` auction not found
- `409` concurrency conflict, the auction changed while the bid was being processed
- `422` business rule violated (bid too low, auction closed, seller bidding)

## Concurrency

Each auction row has a `rowversion` column used as a concurrency token.
Saving a bid runs `UPDATE ... WHERE Id = @id AND RowVersion = @version`, so only one request can win when several read the same state.

The integration test fires 20 identical bids at the same auction at the same time.
Exactly one wins; the losers are rejected with `409 Conflict` (stale row version)
or `422 Unprocessable Entity` (re-read after the winner and failed the minimum rule).
The exact split varies per run, but the invariants always hold:

```
200 OK: 1
409 Conflict or 422 Unprocessable Entity: 19
500: 0
Bids persisted: 1
```

The test also checks the final price, the bid count and the number of rows in `Bids`.

## Tests

```bash
# Domain unit tests
dotnet test tests/LiveAuction.Tests

# Integration tests (requires the SQL Server container; uses a separate LiveAuction_Tests database)
dotnet test tests/LiveAuction.IntegrationTests
```

## Roadmap

- [ ] JWT authentication with an HTTP-only cookie (bidder and seller taken from the token)
- [ ] Live bid updates with SignalR
- [ ] Angular client
- [ ] Load test with results
- [ ] CI pipeline

## Notes

MediatR is distributed under a commercial license model in recent versions. It runs without a key for development and testing, and the API logs a warning at startup.