# Backend API for Azure DevOps and KeyVault Interaction

This .NET API project is designed to provide a seamless interface for interacting with Azure DevOps and Azure KeyVault. It is stateless and allows users to perform various operations related to these services through a set of well-defined endpoints. This README will guide you through the setup, endpoints, and usage of this API.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Endpoints](#endpoints)
  - [GET Projects](#get-projects)
  - [GET Secrets from KeyVault](#get-secrets)
  - [GET Deleted Secrets from KeyVault](#get-deleted-secrets-from-keyvault)
  - [Delete Secrets](#delete-secrets)
  - [Recover Secrets](#recover-secrets)
  - [Copy-Paste Entire KeyVaults](#copy-paste-entire-keyvaults)
  - [Get Variables from Variable Groups](#get-variables-from-variable-groups)
  - [Delete, Add, Update Variables of Variable Groups](#delete-add-update-variables-of-variable-groups)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Prerequisites
Before you can use this API, make sure you have the following prerequisites installed and configured:

- .NET SDK (version X.X or higher)
- Azure DevOps SDK NuGet packages
- Azure KeyVault access and authentication credentials
- Visual Studio or a compatible IDE for development
- An Azure DevOps organization and project with the necessary permissions

## Getting Started
1. Clone this repository to your local machine.
2. Open the project in your preferred IDE.
3. Configure your Azure DevOps and KeyVault authentication credentials.
4. Build and run the project.

## Endpoints

### GET Projects

**Endpoint:** `/api/projects`
**Description:** Retrieve a list of projects from your Azure DevOps organization.

### GET Secrets

**Endpoint:** `/api/secrets`
**Description:** Retrieve secrets from Azure KeyVault.

### GET Deleted Secrets from KeyVault

**Endpoint:** `/api/deletedsecrets`
**Description:** Retrieve deleted secrets from Azure KeyVault.

### Delete Secrets

**Endpoint:** `/api/deletesecrets`
**Description:** Delete secrets from Azure KeyVault.

### Recover Secrets

**Endpoint:** `/api/recoversecrets`
**Description:** Recover deleted secrets in Azure KeyVault.

### Copy-Paste Entire KeyVaults

**Endpoint:** `/api/copykeyvaults`
**Description:** Copy and paste entire KeyVaults, including secrets, from one location to another.

### Get Variables from Variable Groups

**Endpoint:** `/api/variablegroups`
**Description:** Retrieve variables from variable groups inside Azure DevOps Libraries.

### Delete, Add, Update Variables of Variable Groups

**Endpoint:** `/api/variablegroups`
**Description:** Perform CRUD operations on variables within variable groups inside Azure DevOps Libraries.

## Usage
Once the API is up and running, you can interact with it using your preferred API client or by making HTTP requests directly. Make sure to provide the required parameters and authentication tokens as needed for each endpoint.

Enjoy using the Azure DevOps and KeyVault interaction API! If you encounter any issues or have suggestions for improvements, please feel free to open an issue or submit a pull request.