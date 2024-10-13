# Skimart

## Introduction

Your run-of-the-mill project of an e-commerce app, this one sells ski products!

Did this just to have a backend since I wanted to learn a bit of _Angular_
for work. The frontend repos for this is at [skimart-ng-ui](https://github.com/fajrocha/skimart-ng-ui.git).

The solution uses  _Clean Architecture_, which I also wanted to explore since it was a service architecture that was 
being driven on new projects at work.

## Key frameworks and libraries

- [ASP.NET](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0).
- [Entity Framework](https://learn.microsoft.com/en-us/ef/core/), ORM for data access;
- [MediatR](https://github.com/jbogard/MediatR), to implement
  a CQRS(ish) pattern where each request is separated in its own **command/query** and respective **handler**.
- [ErrorOr](https://github.com/amantinband/error-or) to essentially use the result pattern.
- [FluentValidation](https://github.com/FluentValidation/FluentValidation) for input validation.
- [Serilog](https://github.com/serilog/serilog), for logging.
- [FluentAssertions](https://github.com/fluentassertions/fluentassertions) for readable test assertions.

# Stripe Setup
## Secrets

The following properties must be set on `secrets.json`:
- `PaymentService:PublishableKey` (from _Stripe_ website)
- `PaymentService:SecretKey` (from _Stripe_ website)
- `PaymentService.WebhookSecret` (from _Stripe_ CLI)

## Running Stripe CLI
On Windows:

- Login: 

```shell
.\stripe.exe login
```

- Listen on webhook payment events:

```shell
 .\stripe.exe listen -f https://localhost:7070/api/payment/webhook -e payment_intent.succeeded,payment_intent.payment_failed
```

