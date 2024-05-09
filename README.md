# Checkmarx.API

The goal of this project is to provide an API Wrapper for the Checkmarx SAST (REST, SOAP and OData API) for .NET Core to works in a transparent way between the different the Checkmarx versions (8.9 or Higher)

It currently already supports the Checkmarx SCA (Software Composition Analysis),

Future Support: (Access Control & IAST) support.


## Running the Unit Tests

Before running the unit tests please make sure to configure the needed credentials using the [Safe storage of app secrets in development in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)

* [Quick access using Visual Studio](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows#manage-user-secrets-with-visual-studio-1)


# How to use the SDK



## Connection to SAST or SCA

A CxClient provider access to SAST and an SCAClient provides access to SCA:

```csharp
// create a SAST client to interact with SAST/OSA and the Access Control (AC)
CxClient sastClient = new CxClient(new Uri("https://sast.server.com"),
                        "my_user",
                        "mypassword");

// create a SCA client to interact with SCA and the Access Control (AC)
SCAClient scaClient = new SCAClient(Tenant, Username, Password);
```
Check the version of Checkmarx Product

```csharp
Console.WriteLine(sastClient.Version);
```

Check the version of Checkmarx Product without authentication

```csharp
Console.WriteLine(CxClient.GetVersionWithoutConnecting("https://sastserver"));
```
Get the Access Control from SAST

```csharp
AccessControlClient accessControlClient = sastClient.AC;
```
Get the Access Control from SCA

```csharp
AccessControlClient accessControlClient = scaClient.AC;
```

## SAST (Security Application Security Testing)

Check the Checkmarx.API.Tests.CxClientUnitTests.cs for a lot of code snippets on how to use the API.

### List projects

```csharp
foreach (var item in sastClient.GetProjects())
{
    Trace.WriteLine(item.Value);
}    
```

### Create Project

```csharp
sastClient.SASTClient.ProjectsManagement_PostByprojectAsync(new SaveProjectDto {
                IsPublic = true, 
                Name = "ProjectName",
                OwningTeam = "34"
}).Wait();

```

### Branch Project

```csharp
sastClient.SASTClient.BranchProjects_BranchByidprojectAsync(123, new BranchProjectDto
            {
                Name = "New Branch Name"
            }).Wait();
```

### Run Scan

```csharp
client.RunSASTScan(projectId, null, true, sourceCodeZipFile);
```

### Presets


### Reports


## SCA (Software Compostion Analysis)

Check the Checkmarx.API.Tests.SCAClientUnitTests.cs for a lot of code snippets on how to use the API.

### List projects

```csharp
foreach (var project in scaClient.ClientSCA.GetProjectsAsync().Result)
{
    Console.WriteLine(project.Id + "  " + project.Name);
}
```

### Create Project

```csharp
 var scaProject = scaClient.ClientSCA.CreateProjectAsync(new API.SCA.CreateProject
 {
     Name = scaProjectName,
     AssignedTeams = new string[] { teamFullPath }
 }).Result;
```

### Trigger Scan with Zip Source Code

```csharp
scaClient.ScanWithSourceCode(scaProject.Id, zipPath);
```

## Access Control API

### Get/List All Users
```csharp
foreach (var user in accessControlClient.GetAllUsersDetailsAsync().Result)
{
     Console.WriteLine(user.Email + string.Join(";", user.TeamIds.Select(x => teamsx].FullName)) +" "  user.LastLoginDate);

     foreach (var role in user.RoleIds.Select(x => roles[x].Name))
     {
         Console.WriteLine("+ " + role);
     }
}
```
### Create a new user

```csharp
ICollection<int> cxTamRoles = new int[] {
    accessControlClient.RolesAllAsync().Result.First(x => x.Name == "SAST Admin").Id
};

ICollection<int> cxTeamIds = new int[] {
    accessControlClient.TeamsAllAsync().Result.First(x => x.FullName == "/CxServer").Id
};

int localeID = accessControlClient.SystemLocalesAsync().Result.First(x => x.Code == "enUS").Id;

CreateUserModel user = new CreateUserModel
{
    FirstName = "firstname",
    LastName = "lastname",
    UserName = "email@checkmarx.com",
    Email = "email@checkmarx.com",
    Password = "******",
    ExpirationDate = DateTimeOffset.UtcNow + TimeSpan.FromDays(1000),
    Active = true,

    Country = "Portugal",
    JobTitle = "The World Greatest",

    AuthenticationProviderId = accessControlClient.AuthenticationProvidersAsyn().Result.First(X =>X.Name == "Application").Id, // Application User

    LocaleId = localeID,
    RoleIds = cxTamRoles,
    TeamIds = cxTeamIds,

};

accessControlClient.CreatesNewUser(user).Wait();
```

### Get/List Teams

```csharp
foreach (var item in accessControlClient.TeamsAllAsync().Result)
{
    Console.WriteLine($"{item.Id} = {item.FullName}");
}
```

### Get or Create Team
```csharp
scaClient.AC.GetOrCreateTeam(teamFullPath);
```