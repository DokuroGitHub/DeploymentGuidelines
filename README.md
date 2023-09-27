# Deployment Guidelines

## Create folder

```bash
mkdir DeploymentGuidelines
cd DeploymentGuidelines
```

## Create git repository

```bash
dotnet new gitignore
git init
git add .
git commit -m "first commit"
git push --set-upstream "https://gitlab.com/DokuroGitHub/DeploymentGuidelines.git" master
git push --set-upstream "https://github.com/DokuroGitHub/DeploymentGuidelines.git" master

git remote add gitlab https://gitlab.com/DokuroSignalR/ApiSignalR.git
git remote add github https://github.com/DokuroGitHub/ApiSignalR.git

```

## Create project

```bash
dotnet new webapi --name ApiCSharp
# Folder for deployment(DeploymentGuidelines) should contain 1 .csproj or .sln file
dotnet new sln --name ApiCSharp
dotnet sln add ApiCSharp
```

## run project

```bash
cd ApiCSharp
dotnet watch
```
