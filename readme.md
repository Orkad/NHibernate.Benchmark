# NHibernate.Benchmark

A [BenchmarkDotNet](https://benchmarkdotnet.org/) harness measuring NHibernate performance
characteristics — mapping-strategy initialization cost, projection vs. full-entity query cost,
and session/change-tracking cost as result-set size grows — plus an `EfCoreComparison/` suite
that puts NHibernate head-to-head against EF Core on those same three angles. See
[`CLAUDE.md`](./CLAUDE.md) for the full architecture writeup, build/run commands, and
conventions.

## Reports

Each benchmark class has its own report next to its source file, with the full results table and
a short takeaways section.

**NHibernate only** (`net48` + `net10.0`, except where noted):

- [`InitializationBenchmark`](./NHibernate.Benchmark/InitializationBenchmark.md) — `SessionFactory`
  build cost across Fluent / XML / ByCode mapping styles
- [`ProjectionBenchmark`](./NHibernate.Benchmark/ProjectionBenchmark.md) — full entity vs.
  LINQ projection query cost (`net10.0` only)
- [`TrackingBenchmark`](./NHibernate.Benchmark/TrackingBenchmark.md) — `session.Flush()` cost
  with no pending changes, as tracked-entity count grows (`net10.0` only)
- [`LatencyBenchmark`](./NHibernate.Benchmark/LatencyBenchmark.md) — non-NHibernate `Ping`
  baseline for the CI runner's noise floor

**EF Core comparison** (`net10.0` only — EF Core's current packages don't support `net48`):

- [`EfCoreComparison.InitializationBenchmark`](./NHibernate.Benchmark/EfCoreComparison/InitializationBenchmark.md) —
  NHibernate (Fluent baseline + ByCode) vs. EF Core first-use model build cost
- [`EfCoreComparison.ProjectionBenchmark`](./NHibernate.Benchmark/EfCoreComparison/ProjectionBenchmark.md) —
  full entity vs. projection, both ORMs, same `ElementsCount` range
- [`EfCoreComparison.TrackingBenchmark`](./NHibernate.Benchmark/EfCoreComparison/TrackingBenchmark.md) —
  `session.Flush()` vs. `context.SaveChanges()`, both ORMs

Numbers below are a snapshot from CI runs [#8](https://github.com/Orkad/NHibernate.Benchmark/actions/runs/32296584205)
and [#10](https://github.com/Orkad/NHibernate.Benchmark/actions/runs/32401249960) (`windows-latest`
GitHub Actions runners, Hyper-V VMs — expect noise from virtualization) — they will drift as the
code and runtime versions change. Re-run `benchmarks.yml` and refresh the linked reports for
current figures rather than trusting this snapshot indefinitely.

## Synthesis

**Within NHibernate:**

- **ByCode is the cheapest mapping style to bootstrap**, on both runtimes and in both the
  NHibernate-only and EF Core comparison suites — consistently ~30–37% faster than Fluent to
  build a `SessionFactory`, with lower allocations.
- **Projecting into a DTO beats loading full entities at every meaningful scale**, and the gap
  widens with row count (up to ~4–5× faster at 5000 rows for a minimal projection). Even
  projecting *all* fields into a DTO still beats full-entity hydration — avoiding proxy/tracking
  machinery matters more than column count.
- **`Flush()`/change-tracking cost scales linearly with tracked-entity count**, independent of
  whether anything is actually dirty — NHibernate has to walk the whole first-level cache on
  every flush. Below a few hundred entities, `RunStrategy.Monitoring`'s lack of warmup makes the
  numbers too noisy to read literally; the trend only becomes reliable at larger scale.

**NHibernate vs. EF Core:**

- **EF Core carries a real fixed-cost premium at small scale** — roughly 1.3–2× slower than
  NHibernate for session/context initialization and for small-result-set queries, largely
  independent of data volume at that end.
- **That premium shrinks, and sometimes reverses, as volume grows.** At 5000 rows, EF Core's
  projection query is *faster* than NHibernate's, and its full-entity fetch is essentially tied.
  At the flush/tracking end, EF Core's `SaveChanges()` becomes ~2× faster than NHibernate's
  `Flush()` once tracked-entity counts exceed roughly 65,000, while also allocating ~12% less
  memory throughout.
- **Read/no-tracking optimizations pay off more consistently for EF Core than for NHibernate**
  in this suite: `AsNoTracking()` reliably beats EF Core's tracked fetch at every non-trivial
  volume, whereas NHibernate's read-only flag doesn't show the same consistent win in a
  benchmark that never calls `Flush()`.
- **Bottom line**: if the workload is dominated by process/session startup or small,
  latency-sensitive queries, NHibernate (ByCode) has a clear edge in this suite. If the workload
  is dominated by large result sets or bulk change-tracking passes, the two ORMs converge, and
  EF Core can come out ahead — so the right choice depends more on the shape of the workload
  than on either ORM being categorically faster.
