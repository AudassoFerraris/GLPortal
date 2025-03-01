# GLPortal

GLPortal is a portal designed for consulting data related to projects based on GitLab. It is specifically intended to organize issues in environments where the free version of GitLab is used, typically in on-premise setups.

## Features

GLPortal addresses some limitations of the free version of GitLab by providing structured issue management based on the following elements:

- **Releases** are managed using milestones.
- **Clients** are tracked using labels with a specific prefix (e.g., `Customer: Acme`).
- **Priorities** are assigned using labels with a defined prefix (e.g., `Priority: 1 Alta`, `Priority: 2 Medio/Alta`).

## Functionality

- Query issues via API using various filters.
- Export issues to Excel for sharing with clients or stakeholders who do not have direct access to GitLab.
- In the initial version, access to GitLab is handled via a single read-only token without user authentication.
- Future versions may introduce user authentication for system queries.
- Potential future enhancements include dashboard support and statistical analysis features.

## Technology Stack

- Developed using **.NET Blazor Server Side**.
- Utilizes **MudBlazor** components for the user interface.

## Open Source Initiative

The project is intended to be **open source** and hosted on **GitHub**.

## References

- [GitLab REST API Docs](https://docs.gitlab.com/api/api_resources/)
- [MudBlazor](https://mudblazor.com/)]
- [FluentAssertions](https://xceed.com/products/unit-testing/fluent-assertions/)

