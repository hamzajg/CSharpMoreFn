# CSharpMoreFn

Functional programming patterns and constructs in modern C# (.NET 8), inspired by *Pluralsight — Making Your C# Code More Functional*.

## Concepts & Patterns

| Construct | Location | Purpose |
|---|---|---|
| `Option<T>` | `CSharpMoreFn.Functional` | Eliminates `null` reference errors using explicit `Some(value)` and `None` states with monadic `Map`, `Bind`, and `Match`. |
| `Result<T, TError>` | `CSharpMoreFn.Functional` | Railway-oriented error handling returning typed `Ok` or `Fail` without throwing unhandled exceptions. |
| `Pipe` & `Compose` | `CSharpMoreFn.Functional` | Clean data transformation pipelines via extension methods and mathematical function composition. |
| `Curry` | `CSharpMoreFn.Functional` | Transforms multi-argument functions into chains of single-argument functions for easy partial application. |
| `OrderPipeline` | `CSharpMoreFn.Examples` | Complete business pipeline demonstrating validation, discount lookups, and tax calculation using pure functions. |

## Running the Application

```bash
dotnet run --project CSharpMoreFn
```

## Running the Tests

```bash
dotnet test
```

## Requirements

- .NET SDK 8.0 or newer.

## License

MIT — see [LICENSE](LICENSE).
