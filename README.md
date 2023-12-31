# Deployment Guidelines

## Api preview

<https://api-csharp-dovt2.azurewebsites.net/swagger/index.html>

## 1. Folk to your git repository

### 1.1. Click on the fork button

![1](files/1.png)

### 1.2. Create folk

![2](files/2.png)

### 1.3. Clone your git repository

![3](files/3.png)

```bash
# example
git clone https://github.com/Vo-Thanh-Do-18110270/DeploymentGuidelines
```

![4](files/4.png)

## 2 Run project locally

### 2.1. Open project using VSCode

![5](files/5.png)

### 2.2. Config appsettings.Development.json

```bash
# to use in memory database
"UseInMemoryDatabase": true,
```

![6](files/6.png)

### 2.3. Using your right dotnet version

![7](files/7.png)

### 2.4. Run project

```bash
cd Apis/WebAPI
dotnet run
```

![8](files/8.png)

![9](files/9.png)

access swagger:
<https://localhost:5001/swagger/index.html>
![10](files/10.png)

## 3. Host a database online

### 3.1. Access <https://somee.com>

`Create an account, very simple, confirmed`

Using Free .Net Hosting, click Learn More button
![11](files/11.png)

Click Order now button
![11](files/12.png)

Create an account
![11](files/14.png)

Click Checkout button
![11](files/13.png)

Open Email, get code, fill in email address verification
![11](files/15.png)

Site name: your site name, ASP.Net version: .Net Core(all versions), then click CREATE WEBSITE button
![11](files/16.png)

At tab MS SQL/Databases, Click (+) Create database button
![11](files/17.png)

Database name: your database name, MS SQL Server version: MS SQL 2022 Express, Click (+) CREATE DATABASE button
![11](files/18.png)

Click to copy Connection string
![11](files/19.png)

Update appsettings.Development.json

```bash
"UseInMemoryDatabase": false,
# add Encrypt=False to the end of the connection string
"DatabaseConnection": "YOUR_COPIED_CONNECTION_STRING;Encrypt=False;"
```

Migration database

```bash
dotnet ef database update -p Infrastructure -s WebAPI
```

![11](files/20.png)

migration success
![11](files/21.png)

Choose Run scripts tab, click Open T-SQL console
![11](files/22.png)

```sql
-- run this sql to check if the database is created
SELECT
  *
FROM
  INFORMATION_SCHEMA.TABLES;
```

![11](files/22.1.png)

Run the project again, access swagger, try some APIs
![11](files/23.png)

## 4. Deploy API to Azure

### 4.1. Access <https://portal.azure.com>

`Create an account, you may need to add a credit card with at least 2$ (they will return back 2$ later)`

### 4.2. Search for Web App, click Web App to create a new Web App

![11](files/24.png)

### 4.3. Basics tab

```bash
Subscription: your existed subscription
Resource group: your existed resource group or create a new one
# example: deployment-guidelines
Web App name: your web app name
# example: api-csharp-dovt2
Publish: Code
Run time stack: your project dotnet version
# example: .NET 7 (LTS)
Region: any region that support Free F1 pricing plan
# example: East US
Pricing plan: Free F1
```

![11](files/26.png)

click Next: Deployment >

### 4.4. Deployment tab

![11](files/27.png)

```note
you can config GitHub Actions settings Continuous deployment here
in this case we will disable it and config it later

Continuous deployment: Disable
```

click Next: Networking >

### 4.5. Networking tab

```note
Enable public access: Yes
```

![11](files/28.png)

click Next: Monitoring >

### 4.6. Monitoring tab

```bash
# we dont need Application Insights so we will disable it
Enable Application Insights: No
```

![11](files/29.png)

we dont need Tags so we will skip it

click Review + create

### 4.7. Review + create tab

![11](files/30.png)

click Create

### 4.8. When resource has been created successfully, click Go to resource

![11](files/31.png)

### 4.9. Update project

```bash
# Default domain is the domain your API will be hosted
Copy Default domain
```

![11](files/32.png)

update appsettings.Development.json using the copied domain

```bash
# axample:
"Issuer": "api-csharp-dovt2.azurewebsites.net",
"Audience": "api-csharp-dovt2.azurewebsites.net"
```

![11](files/33.png)

### 4.10. Delete .github and .gitlab folder if existed

### 4.11. Git commit and push to save all changes

### 4.12. Deployment Center tab

Here you can choose to deploy your API from GitHub, GitLab, Azure DevOps, Local Git

```bash
# example: using GitHub
Source: GitHub
Organization: DokuroGitHub
Repository: DeploymentGuidelines
Branch: master
Workflow Option: Add a workflow
```

![11](files/34.png)

```bash
# example: using GitLab
Source: External Git
Repository: https://gitlab.com/DokuroGitHub/DeploymentGuidelines
Branch: master
Repository Type: public
```

![11](files/35.png)

if you not using CI/CD you need to sync manually else you can skip this step
![11](files/36.png)

### 4.13. Configuration tab

At Application setting, click New application setting to add each key-value pair
![11](files/37.png)

or click Advanced edit to add all key-value pairs at once
![11](files/38.png)

```bash
# example:
[
  {
    "name": "DatabaseConnection",
    "value": "workstation id=dovt58GG.mssql.somee.com...;Encrypt=False;",
    "slotSetting": false
  },
  {
    "name": "Jwt:Audience",
    "value": "api-csharp-dovt2.azurewebsites.net",
    "slotSetting": false
  },
  {
    "name": "Jwt:ExpireDays",
    "value": "69",
    "slotSetting": false
  },
  {
    "name": "Jwt:Issuer",
    "value": "api-csharp-dovt2.azurewebsites.net",
    "slotSetting": false
  },
  {
    "name": "Jwt:Key",
    "value": "DokuroSaid:ThisIsMoreThan258bitsSecretKey",
    "slotSetting": false
  },
  {
    "name": "SeedOnInit",
    "value": "true",
    "slotSetting": false
  },
  {
    "name": "UseInMemoryDatabase",
    "value": "false",
    "slotSetting": false
  }
]
```

click Ok, Save, Continue
![11](files/39.png)

### 4.14. Now the API has been deployed, go to swagger and try some APIs

Swagger link: Default domain + "/swagger/index.html"

example: <https://api-csharp-dovt2.azurewebsites.net/swagger/index.html>

![11](files/40.png)
