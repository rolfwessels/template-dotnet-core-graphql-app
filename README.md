# 🌐 .NET GraphQL App Template

[![CI](https://github.com/rolfwessels/template-dotnet-core-graphql-app/actions/workflows/github-action.yml/badge.svg)](https://github.com/rolfwessels/template-dotnet-core-graphql-app/actions/workflows/github-action.yml)

An ASP.NET Core 8 starter with a Hot Chocolate GraphQL API and a typed Strawberry Shake client. The in-memory movie catalog demonstrates queries, mutations, and subscriptions.

## 🛠 Prerequisites

- Git and `make` (use WSL on Windows for the Makefile commands).
- The .NET 8 SDK and `curl` for the local quick start, or Docker with Compose for the development container.

## 🚀 Quick start

Start the API from the repository root.

```sh
make start
```

In a second terminal, query the demo API.

```sh
curl --fail-with-body http://localhost:5085/graphql \
  -H 'Content-Type: application/json' \
  --data '{"query":"{ movies { id title } }"}'
```

The response contains two movies under `data.movies`, including **The Rise of the GraphQL Warrior**. Stop the API with Ctrl+C.

For Docker development, run `make build up` on the host, then `make start` in the development shell. Run the query from a second shell in the same container with `docker compose exec dev zsh` because the development container keeps port 5085 internal.

The dev shell shows a 🐳 prompt and saves Zsh history in a project volume across `make down` and rebuilds.

## 📋 Make commands

| Command | Purpose |
| --- | --- |
| `make start` | Start the API. |
| `make test` | Run the API and core tests with coverage. |
| `make update-client` | Refresh the Strawberry Shake schema. |
| `make build up` | Build the development container and open its shell. |
| `make down` | Stop the development container from the host. |

Run `make help` for more targets.

## 💻 Development

- The API, typed client, and movie catalog live in the `.Api`, `.Client`, and `.Core` projects under `src/`.
- When the API schema changes, run `dotnet tool restore` and then `make update-client` from the repository root with the API stopped. The target starts the API while updating the schema.
- See [AGENTS.md](AGENTS.md) for branch naming, pull requests, and verification.

## 📦 Releases and deployment

- `make publish` writes framework-dependent Linux and Windows x64 builds under `dist/`.
- The root `Dockerfile` contains both stages. `make build` builds the `dev` stage and `make docker-build` builds the `runtime` stage. Use `make docker-publish` to build and push after configuring your registry.
- Set the major and minor version with `versionPrefix` in the Makefile. The patch comes from the Git commit count. `make version` writes the current version to `src/version.json`.
- Create release tags such as `v1.2.0` on `main`. For an urgent fix to an older release, branch from its tag and merge the fix back into `main`.

## 📜 License

See [LICENSE.txt](LICENSE.txt).
