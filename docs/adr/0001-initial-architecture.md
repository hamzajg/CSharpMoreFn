# ADR 0001 — Initial architecture

Date: 2026-11-13 (baseline recorded retroactively)
Status: accepted (describes what is built)

## Context

A teaching repo for FP patterns in modern C#, following a Pluralsight
course. It must demonstrate each construct in isolation and stay readable.

## Decisions

1. **Hand-rolled FP primitives over libraries.** `Option<T>` and
   `Result<T,E>` are implemented in-repo rather than pulled from
   LanguageExt/FsCheck-style packages. Rationale: the point is to show the
   mechanics; a dependency would hide them.
2. **`readonly struct` for `Option<T>`.** Value-type, allocation-free,
   `IsSome`/`IsNone` discriminated union over `T?`. Rationale: keeps the
   construct honest C# — no runtime magic.
3. **Railway-oriented `Result`.** `Ok`/`Fail` compose via `Bind`/`Map`
   without exceptions. Rationale: demonstrates error-as-value explicitly.
4. **One construct per file + one test class per construct.** Flat
   `Functional/` and `Tests/` layouts. Rationale: mirrors the course
   structure; each file is a self-contained lesson.
5. **net10.0, console demo only.** No web/API host, no DI container.
   Rationale: KISS — the constructs are the product.
6. **Tests pin behaviour.** Every public member has coverage; the suite is
   the executable spec. Rationale: teaching code must not silently rot.

## Consequences

- These are pedagogical implementations — they intentionally omit
  production concerns (e.g. full `Option` API surface, async variants).
- Extending means adding a construct + tests, not refactoring the core.
