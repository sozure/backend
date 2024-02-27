# VGManager Backend: A backend API for Azure git, profile and pipeline interactions

This .NET API project is designed to provide a seamless interface for interacting with Azure DevOps and Azure KeyVault. It is stateless and allows users to perform various operations related to these services through a set of well-defined endpoints. This README will guide you through the setup, endpoints, and usage of this API.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Endpoints](#endpoints)
  - [GET Build pipeline](#get-build-pipeline)
  - [GET All build pipelines](#get-all-build-pipelines)
  - [Run build pipeline](#run-build-pipeline)
  - [Run build pipelines](#run-build-pipelines)
  - [Get file paths](#get-file-paths)
  - [Get config files](#get-config-files)
  - [Get git repositories](#get-git-repositories)
  - [Get variables from git repository](#get-variables-from-git-repository)
  - [Get git branches](#get-git-branches)
  - [Get git tags](#get-git-tags)
  - [Create git tag](#create-git-tag)
  - [Get profile](#get-profile)
  - [Get environments](#get-environments)
  - [Get connected variable groups](#get-connected-variable-groups)
  - [Get connected azure projects](#get-connected-azure-projects)

- [Usage](#usage)

## Prerequisites
Before you can use this API, make sure you have the following prerequisites installed and configured:

- .NET SDK (version 6.0 or higher)
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
### Build Pipeline
#### - GET Build pipeline
**Endpoint:** `/api/BuildPipeline/GetRepositoryId`
**Description:** Retrieve a build pipeline from your Azure DevOps organization.

#### - GET All build pipelines
**Endpoint:** `/api/BuildPipeline/GetAll`
**Description:** Retrieve a list of build pipelines from Azure DevOps organization.

#### - Run build pipeline
**Endpoint:** `/api/BuildPipeline/Run`
**Description:** Run a build pipeline from your Azure DevOps organization.

#### - Run build pipelines
**Endpoint:** `/api/BuildPipeline/RunAll`
**Description:** Run a list of build pipelines from your Azure DevOps organization.

### Git file
#### - Get file paths
**Endpoint:** `/api/GitFile/FilePath`
**Description:** Get file path by file name from Azure DevOps Git.

#### - Get config files
**Endpoint:** `/api/GitFile/ConfigFiles`
**Description:** Get config files by extension name from Azure DevOps Git.

### Git repository
#### - Get git repositories
**Endpoint:** `/api/GitRepository`
**Description:** Get git repositories from Azure DevOps Git.

#### - Get variables from git repository
**Endpoint:** `/api/GitRepository/Variables`
**Description:** Get environment variables from Azure DevOps Git repository.

### Git version
#### - Get git branches
**Endpoint:** `/api/GitVersion/Branches`
**Description:** Get git branches by git repository from Azure DevOps Git.

#### - Get git tags
**Endpoint:** `/api/GitVersion/Tags`
**Description:** Get git tags by git repository from Azure DevOps Git.

#### - Create git tag
**Endpoint:** `/api/GitVersion/Tag/Create`
**Description:** Create git tag from Azure DevOps Git.

### Profile
#### - Get profile
**Endpoint:** `/api/Profile`
**Description:** Get profile from Azure DevOps.

### Release pipeline
#### - Get environments
**Endpoint:** `/api/ReleasePipeline/GetEnvironments`
**Description:** Get environments by release pipeline from Azure DevOps.

#### - Get connected variable groups
**Endpoint:** `/api/ReleasePipeline/GetVariableGroups`
**Description:** Get connected variable groups by release pipeline from Azure DevOps.

#### - Get connected azure projects
**Endpoint:** `/api/ReleasePipeline/GetProjects`
**Description:** Get connected azure projects by release pipeline from Azure DevOps.

## Usage
Once the API is up and running, you can interact with it using your preferred API client or by making HTTP requests directly. Make sure to provide the required parameters and authentication tokens as needed for each endpoint.

Enjoy using the Azure DevOps and KeyVault interaction API! If you encounter any issues or have suggestions for improvements, please feel free to open an issue or submit a pull request.