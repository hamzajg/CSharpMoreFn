# AGENTS.md — CSharpMoreFn

Guidance for AI agents (and humans) working in this repository.

## Project

CSharpMoreFn demonstrates functional-programming patterns in modern C# on
.NET 10, inspired by *Pluralsight — Making Your C# Code More Functional*.
It's a teaching/reference repo: small constructs, each pinned by tests.
Stack: C#, `net10.0`, xUnit-style tests (actually plain test project —
check `CSharpMoreFn.Tests` for the framework in use).

## Commands

Verified 2026-11-13 against CI (`.github/workflows/ci.yml`).

| Task | Command |
|---|---|
| Build | `dotnet build --configuration Release` |
| Test | `dotnet test --configuration Release --no-build` |
| Run demo | `dotnet run --project CSharpMoreFn` |

Requires: .NET 10 SDK (CI pins `10.0.x`). Solution: `CSharpMoreFn.sln`.

## Architecture

- `CSharpMoreFn/Functional/` — the FP building blocks:
  - `Option.cs` — `Option<T>` struct (`Some`/`None`, `Map`, `Bind`,
    `Match`) eliminating null-reference errors.
  - `Result.cs` — `Result<T,TError>` railway-oriented `Ok`/`Fail`.
  - `FunctionalExtensions.cs` — `Pipe`, `Compose`, `Curry` extensions.
- `CSharpMoreFn/Examples/OrderPipeline.cs` — end-to-end example: order
  validation → discount lookup → tax, all pure functions.
- `CSharpMoreFn/Program.cs` — console demo entry.
- `CSharpMoreFn.Tests/` — one test class per construct
  (`OptionTests`, `ResultTests`, `FunctionalExtensionsTests`,
  `OrderPipelineTests`).

## Conventions

- One construct per file in `Functional/`; examples in `Examples/` show
  composition. Keep constructs small, immutable (`readonly struct`), and
  total (no hidden exceptions — that's what `Result`/`Option` are for).
- Match the existing style: expression-bodied members, XML-doc-light,
  `ArgumentNullException` guards on public function args.
- Every new construct needs tests in `CSharpMoreFn.Tests/` — the tests are
  the spec.
- KISS: teach one idea per type; no frameworks, no DI, no async here.

## Landmines

- **Default branch is `master`, not `main`.**
- `Option<T>.Some` throws on null value — `None` is `default`; don't
  confuse the two.
- `obj/` dirs show both net8.0 and net10.0 artifacts — the csproj targets
  `net10.0` only; stale net8 obj output is build cache, ignore it.
- README says ".NET 8" in places but csproj + CI are .NET 10 — trust the
  csproj/CI.

## Agent rules

- Branch from `master`; name branches `feat/*`, `fix/*`, `chore/*`, `docs/*`.
- Docs-only changes still go through a PR; never commit to `master` directly.
- Verify before claiming: `dotnet build` + `dotnet test` and report the
  real result.
- Keep changes minimal and consistent with surrounding code. No drive-by
  refactors.
- Update AGENTS.md / docs/ARCHITECTURE.md when you add a construct or change
  the public API of `Functional/`.
