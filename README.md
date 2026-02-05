# ChaReq

A .NET Core application to manage and log change requests made for trial and non-trial systems.

- Repo-Status: Active
- Repo-Contents: System
- Repo-Service-Name: ChaReq
- Repo-Ownership-Rating: 3
- Repo-Quality-Rating: 4
- Repo-Next-Review-Due: 2026-06-09

### Cloning and running the project for the first time

Secret configuration values are not stored in source control, you must set them inside Visual Studio before attempting to run the web application. When deployed to the server the secrets are stored as environment variables.

1. Copy the secrets string from shared User Secrets LastPass folder
2. In Visual Studio, right-click the WebApp and click 'Manage User Secrets'
3. Paste the secrets string into the secrets.json file and save

**Development Team**: Jenna Ward

**User Roles**

'User' has access to view requests, and create new ones.

'Worker' also has access to fill out the authorisations and assign users and labels to requests. They can also manage Labels, Trials and Users through the admin pages.

'Authoriser' currently has the same permissions as a Worker, but was envisioned that it was to be only able to do initial authorisations and act as an administrator.

### Important Information
#### YTUBot GitHub Token

- YTUBot personal access token currently in use: ChaReq 20221201

- Expiration date: This token has no expiration date.

- Usage: Automate GitHub-related tasks associated with work requests.
