# Agent Instructions

Read [README.md](README.md) for project setup and commands.

## What we value

- Build the simplest solution that works. Prefer clear names, small functions, and straightforward control flow.
- Keep changes focused and reversible. Add abstractions when the code has a concrete need for them.

## Conventions and boundaries

- Ask before adding external dependencies or changing database migrations.
- Put detailed workflows in skills or nested `AGENTS.md` files.
- Branch from `main` using `feature/<name>` for features and `bug/<name>` for fixes. Open pull requests against `main`.
- Use **What**, **Why**, **Decisions**, **Testing**, and **Risks** as pull request headings.
- Keep GraphQL resolvers in the API project and application logic in the Core project. Use `make update-client` to refresh Strawberry Shake's copy of the API schema.

## Verification and handoff

- For a behavior change, start with a failing test, make it pass with the smallest change, and then refactor.
- Run `make verify` for code changes. It includes `make test` and the dependency audit (`make scan`).
- Start the API with `make start` and exercise the affected operation at `/graphql`. For a smoke check, query `{ movies { id } }`.
- Exercise the real entry point after meaningful behavior changes and before opening or updating the pull request. Check the output and relevant side effects.
- Mock external systems at their boundaries. Confirm what test or dry-run modes skip, preserve the coverage of failing checks, and report any verification gaps.
- Before opening a pull request, get an independent review of correctness and security when a fresh reviewer is available.
