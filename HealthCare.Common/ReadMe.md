# 📦 NuGet Package Publishing to Azure Artifacts

This repository is configured to **automatically build and publish NuGet packages** to an **Azure Artifacts feed** using **GitHub Actions**.

---

## Why Do We Need This?

- We maintain **shared libraries** (e.g., `HealthCare.Common.Authorization`, `HealthCare.Common.Models`, etc.) that are reused across multiple microservices.
- Instead of copying DLLs manually, we **publish them as NuGet packages**.
- Azure Artifacts acts as a **secure private NuGet feed**.
- GitHub Actions automates this process, ensuring:
  - Every build produces packages.
  - Packages are consistently versioned and pushed.
  - Duplicates are skipped automatically.

---

## Workflow Overview

The workflow file (`.github/workflows/publish-packages.yml`) does the following:

1. **Checkout Code**  
   Pulls the latest source code from GitHub.

2. **Setup .NET**  
   Installs the required .NET SDK version.

3. **Create NuGet Output Folder**  
   Creates a `./nupkgs` directory to store `.nupkg` files.

4. **Pack Projects**  
   Runs `dotnet pack` for each shared library project:
   - `HealthCare.Common.Authorization`
   - `HealthCare.Common.HttpClientHelper`
   - `HealthCare.Common.Models`
   - `HealthCare.Common.Persistence`

5. **Configure NuGet Source**  
   Adds Azure Artifacts feed as a NuGet source using:
   - Azure DevOps **Organization**
   - Azure DevOps **Project GUID** (important if project name has spaces)
   - Azure DevOps **Feed Name**
   - Azure DevOps **Personal Access Token (PAT)**

6. **Push Packages to Feed**  
   Uses `nuget.exe push` with:
   - `-ApiKey az` (placeholder, real auth uses PAT)  
   - `-SkipDuplicate` (avoids uploading same version twice)

---

## GitHub Secrets Setup

The workflow relies on **GitHub Secrets** to authenticate with Azure DevOps.

Add these in your GitHub repository → **Settings → Secrets and variables → Actions**:

- `AZURE_ORG` → Azure DevOps organization name (e.g., `healthorasolutions`)
- `AZURE_PROJECT` → Azure DevOps **Project GUID** (use GUID if name has spaces)
- `AZURE_FEED_NAME` → Azure Artifacts feed name (e.g., `HealthCareFeed`)
- `AZURE_FEED_PAT` → Personal Access Token with **Packaging (Read & Write)** permissions

---

## Run Manually (Without GitHub Actions)

If you want to build and push packages manually, follow these steps:

1. **Pack your projects**
   ```powershell
   mkdir nupkgs
   dotnet pack ./HealthCare.Common/HealthCare.Common.Authorization/HealthCare.Common.Authorization.csproj -o ./nupkgs -c Release
   dotnet pack ./HealthCare.Common/HealthCare.Common.HttpClientHelper/HealthCare.Common.HttpClientHelper.csproj -o ./nupkgs -c Release
   dotnet pack ./HealthCare.Common/HealthCare.Common.Models/HealthCare.Common.Models.csproj -o ./nupkgs -c Release
   dotnet pack ./HealthCare.Common/HealthCare.Common.Persistence/HealthCare.Common.Persistence.csproj -o ./nupkgs -c Release


2. **Configure NuGet Source**

./nuget.exe sources Remove -Name "HealthCareFeed" || echo "Feed not found"
./nuget.exe sources Add -Name "HealthCareFeed" `
  -Source "https://pkgs.dev.azure.com/<ORG>/<PROJECT_GUID>/_packaging/<FEED_NAME>/nuget/v3/index.json" `
  -Username "any" `
  -Password "<AZURE_FEED_PAT>" `
  -StorePasswordInClearText

3. **Push Package**

./nuget.exe push "nupkgs\*.nupkg" -Source "HealthCareFeed" -ApiKey az -SkipDuplicate

