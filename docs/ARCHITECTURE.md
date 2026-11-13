# Architecture — CSharpMoreFn

Current-state map. Descriptive, not aspirational; see `docs/adr/` for why.

## Layers

```
CSharpMoreFn/
  Functional/     pure FP primitives — no I/O, no dependencies
    Option.cs       Option<T> struct: Some/None + Map/Bind/Match
    Result.cs       Result<T,TError>: Ok/Fail railway composition
    FunctionalExtensions.cs  Pipe / Compose / Curry extension methods
  Examples/
    OrderPipeline.cs  reference composition: validate → discount → tax
  Program.cs       console demo wiring the examples
CSharpMoreFn.Tests/   xUnit-style tests, one class per construct
```

## Design

- **Primitives are pure and total.** `Option`/`Result` make absence and
  failure explicit values; methods return new values, never mutate.
- **Composition over inheritance.** `Pipe`/`Compose`/`Curry` turn function
  chains into data-flow pipelines; `OrderPipeline` is the canonical
  consumer.
- **Tests are the contract.** Each construct has a dedicated test class;
  behaviour is pinned by examples, not docs.

## Data flow

`Program.cs` → `Examples/OrderPipeline` → `Functional/*` primitives.
Nothing persists; the app is a console demo of the types.
