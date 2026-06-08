# WaiterApp API — Agent Notes

## Stack

- .NET 10.0 ASP.NET Core Web API, single project (`waiter-app-api/waiter-app-api.csproj`)
- PostgreSQL via Npgsql.EntityFrameworkCore.PostgreSQL
- EF Core Code-First with Migrations
- Root namespace: `WaiterApp`

## Run & Build

```bash
dotnet run --project waiter-app-api   # http://localhost:5039
dotnet build waiter-app-api/waiter-app-api.csproj
```

PostgreSQL must be running. Connection string is loaded from the `ConnectionStrings__DefaultConnection` environment variable (via `.env` file using DotNetEnv). Copy `.env.example` to `.env` and fill in the values — `.env` is gitignored.

## EF Core Migrations

```bash
dotnet ef migrations add <Name> --project waiter-app-api --startup-project waiter-app-api
dotnet ef database update --project waiter-app-api --startup-project waiter-app-api
```

## Architecture

```
Controllers → Services (Interfaces/) → Repositories (Interfaces/) → AppDbContext
```

- DI registrations are all in `Program.cs` (no `Startup.cs`)
- Every Service and Repository has a matching interface in a nested `Interfaces/` folder
- JSON output: camelCase property names, null values omitted (`JsonIgnoreCondition.WhenWritingNull`)
- Global error handling via `GlobalErrorMiddleware` (catches unhandled exceptions → 500)
- **Request validation** uses FluentValidation with SharpGrip auto-validation — validators in `Validators/` folder run automatically before controller actions; invalid requests return 400 with field-level errors
- Route parameters (e.g. `{orderId}`, `{categoryId}`) are still validated manually in controllers via `Guid.TryParse` — FluentValidation only covers body/form models

## Domain Quirks

- **All entity IDs are `Guid`** — controllers parse route params from string via `Guid.TryParse`
- **Product creation uses `[FromForm]`** (multipart form data), not JSON body, because of image upload
- **Ingredients are passed as a JSON string** in the `Ingredients` form field, deserialized server-side in `ProductService`
- **Price in `CreateProductRequest`** is a `string` — validated as decimal by FluentValidation, then parsed with `CultureInfo.InvariantCulture` in the controller
- **Category in `CreateProductRequest`** is a `string` — validated as GUID by FluentValidation, then parsed with `Guid.Parse` in the controller
- **Order statuses**: only `WAITING`, `IN_PRODUCTION`, `DONE` are valid (hardcoded `HashSet` in `OrderService`)
- **OrderItem** has a composite key `{OrderId, ProductId}` with `Quantity` payload
- **Product images** are saved to `wwwroot/uploads/` with filename `{timestamp}-{originalFilename}`, served via `UseStaticFiles()`

## Known Issues

- Health check route is `/heathcheck` (typo — missing `l`). Don't "fix" without updating any clients.

## What's Missing

- No authentication or authorization
- No test project
- No CI/CD workflows
- No Swagger UI wired up (only the `Microsoft.AspNetCore.OpenApi` package is referenced)
- The `.http` file still references the default `weatherforecast` endpoint

## Agent skills

### Issue tracker

Issues live in GitHub Issues on `luiizfigueiredo/waiter-app-api` (uses the `gh` CLI). See `docs/agents/issue-tracker.md`.

### Triage labels

Five canonical roles, each mapped to its default label string (`needs-triage`, `needs-info`, `ready-for-agent`, `ready-for-human`, `wontfix`). See `docs/agents/triage-labels.md`.

### Domain docs

Single-context — one `CONTEXT.md` + `docs/adr/` at the repo root. See `docs/agents/domain.md`.