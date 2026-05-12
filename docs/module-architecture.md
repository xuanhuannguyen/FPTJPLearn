# JPLearn module architecture

This project uses a modular monolith structure. Keep modules independent at the feature boundary, while sharing infrastructure only through explicit common abstractions.

## Backend layers

- `JPLearn.Api`: HTTP boundary only. Controllers translate requests, resolve `CurrentUserId`, and return API responses. Do not put business rules here.
- `JPLearn.Core`: domain contracts, entities, DTOs, constants, and service interfaces. This layer should not depend on EF Core or ASP.NET controllers.
- `JPLearn.Infrastructure`: EF Core `AppDbContext`, entity configurations, migrations, seed data, and service implementations.

## Backend module shape

For a new module named `Payments`, prefer this shape:

```text
server/
  JPLearn.Core/
    Payments/
      DTOs/
      Entities/
      IPaymentService.cs
      PaymentConstants.cs
  JPLearn.Infrastructure/
    Services/
      PaymentService.cs
    Data/
      Configurations/
        PaymentConfigurations.cs
  JPLearn.Api/
    Controllers/
      PaymentsController.cs
```

Rules:

- Every user-owned query/mutation receives `CurrentUserId` from `ApiControllerBase`.
- Controllers must not contain `DevUserId`, SRS rules, payment rules, exam scoring rules, or EF queries.
- Service interfaces live in `Core/<Module>`.
- Service implementations live in `Infrastructure/Services`.
- EF table mapping lives in `Infrastructure/Data/Configurations`.
- Seed data lives in `Infrastructure/Data/Seed`.
- Public API routes use stable module names: `/api/users`, `/api/payments`, `/api/exams`.

## Current user

All controllers inherit from `ApiControllerBase` and use:

```csharp
CurrentUserId
```

`DevelopmentCurrentUserContext` resolves the user in this order:

1. JWT claim: `ClaimTypes.NameIdentifier`, `sub`, or `user_id`
2. `X-User-Id` request header for local testing
3. Development fallback user id `00000000-0000-0000-0000-000000000001`

When auth is implemented, replace the API registration for `ICurrentUserContext`; controllers and services should not change.

## Frontend feature shape

Every feature should own its API client, routes, pages, components, hooks, and types:

```text
client/src/features/payments/
  api/
    paymentsApi.ts
  components/
  hooks/
  pages/
  types/
    payments.types.ts
  payments.routes.tsx
```

Rules:

- Shared UI belongs in `client/src/shared/components`.
- Shared HTTP setup belongs in `client/src/shared/api`.
- Feature-to-feature imports are allowed only for stable reusable UI, not for business state.
- Route files export feature route arrays and are composed only in `Router.tsx`.

## New module checklist

1. Add entities and DTOs in `Core/<Module>`.
2. Add service interface in `Core/<Module>`.
3. Add EF configuration and `DbSet` in `Infrastructure`.
4. Add service implementation in `Infrastructure/Services`.
5. Register the service in `AddJPLearnInfrastructure`.
6. Add API controller inheriting `ApiControllerBase`.
7. Add frontend feature folder with API client, types, routes, and pages.
8. Run `dotnet build JPLearn.slnx` and `npm run build`.

## Suggested future modules

- `Users`: account, profile, subscriptions, permissions, and learning preferences.
- `Payments`: plans, orders, payment provider callbacks, invoices, entitlements.
- `ExamPractice`: exam packages, sections, questions, attempts, scoring, explanations, and review history.
