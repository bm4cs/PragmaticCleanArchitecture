var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution = "Bookify.sln";
var apiProject = "src/Bookify.Api/Bookify.Api.csproj";

Task("Clean")
    .Description("Cleans all bin and obj directories")
    .Does(() =>
    {
        DotNetClean(solution);
        
        var binDirs = GetDirectories("./**/bin");
        var objDirs = GetDirectories("./**/obj");
        
        CleanDirectories(binDirs);
        CleanDirectories(objDirs);
        
        Information("Cleaned solution and directories");
    });

Task("Restore")
    .Description("Restores NuGet packages")
    .Does(() =>
    {
        DotNetRestore(solution);
        Information("Restored NuGet packages");
    });

Task("Build")
    .Description("Builds the solution")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetBuild(solution, new DotNetBuildSettings
        {
            Configuration = configuration,
            NoRestore = true
        });
        Information($"Built solution in {configuration} mode");
    });

Task("Test")
    .Description("Runs all tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var testProjects = GetFiles("./**/*Test*.csproj") + GetFiles("./**/*.Tests.csproj");
        
        if (testProjects.Count == 0)
        {
            Warning("No test projects found");
            return;
        }
        
        foreach(var project in testProjects)
        {
            DotNetTest(project.FullPath, new DotNetTestSettings
            {
                Configuration = configuration,
                NoBuild = true,
                NoRestore = true
            });
        }
        Information("All tests completed");
    });

Task("Docker-Build")
    .Description("Builds Docker image for API")
    .Does(() =>
    {
        StartProcess("docker", new ProcessSettings
        {
            Arguments = "build -t bookifyapi:latest -f src/Bookify.Api/Dockerfile ."
        });
        Information("Docker image built: bookifyapi:latest");
    });

Task("Infra-Up")
    .Description("Starts containerized infrastructure (databases, etc.)")
    .Does(() =>
    {
        if (FileExists("docker-compose.yml"))
        {
            StartProcess("docker", new ProcessSettings
            {
                Arguments = "compose up -d"
            });
            Information("Infrastructure containers started");
        }
        else
        {
            Warning("No docker-compose.yml found - skipping infrastructure startup");
        }
    });

Task("Infra-Down")
    .Description("Stops containerized infrastructure")
    .Does(() =>
    {
        if (FileExists("docker-compose.yml"))
        {
            StartProcess("docker", new ProcessSettings
            {
                Arguments = "compose down"
            });
            Information("Infrastructure containers stopped");
        }
        else
        {
            Warning("No docker-compose.yml found - skipping infrastructure shutdown");
        }
    });

Task("Dev-Certs-Generate")
    .Description("Generates HTTPS development certificate")
    .Does(() =>
    {
        var userProfile = EnvironmentVariable("USERPROFILE") ?? EnvironmentVariable("HOME");
        var certPath = $"{userProfile}/.aspnet/https/aspnetapp.pfx";
        
        StartProcess("dotnet", new ProcessSettings
        {
            Arguments = $"dev-certs https -ep \"{certPath}\" -p changeme"
        });
        Information($"Generated dev certificate at: {certPath}");
    });

Task("Dev-Certs-Trust")
    .Description("Trusts HTTPS development certificates")
    .Does(() =>
    {
        StartProcess("dotnet", new ProcessSettings
        {
            Arguments = "dev-certs https --trust"
        });
        Information("Trusted HTTPS development certificates");
    });

Task("Dev-Certs-Setup")
    .Description("Sets up HTTPS development certificates (generate + trust)")
    .IsDependentOn("Dev-Certs-Generate")
    .IsDependentOn("Dev-Certs-Trust");

Task("Add-Migration")
    .Description("Creates a new Entity Framework migration")
    .Does(() =>
    {
        var migrationName = Argument("name", "");
        if (string.IsNullOrEmpty(migrationName))
        {
            throw new Exception("Migration name is required. Use: --name=MigrationName");
        }
        
        DotNetTool($"ef migrations add {migrationName}", new DotNetToolSettings
        {
            WorkingDirectory = "./src/Bookify.Api"
        });
        Information($"Created migration: {migrationName}");
    });

Task("Migrate")
    .Description("Runs Entity Framework database migrations")
    .Does(() =>
    {
        DotNetTool("ef database update", new DotNetToolSettings
        {
            WorkingDirectory = "./src/Bookify.Api"
        });
        Information("Database migrations applied");
    });

Task("Run-Api")
    .Description("Runs the API project")
    .Does(() =>
    {
        DotNetRun(apiProject, new DotNetRunSettings
        {
            Configuration = "Development"
        });
    });

Task("Publish")
    .Description("Publishes the API project")
    .IsDependentOn("Build")
    .Does(() =>
    {
        DotNetPublish(apiProject, new DotNetPublishSettings
        {
            Configuration = configuration,
            OutputDirectory = "./publish",
            NoRestore = true,
            NoBuild = true
        });
        Information("Published API to ./publish");
    });

Task("Full-Build")
    .Description("Complete build pipeline with tests")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("Deploy-Local")
    .Description("Full local deployment with infrastructure")
    .IsDependentOn("Infra-Up")
    .IsDependentOn("Docker-Build")
    .Does(() =>
    {
        Information("Local deployment complete");
        Information("API Docker image: bookify-api:latest");
        Information("Infrastructure: Running via docker-compose");
    });

Task("Default")
    .Description("Default task - runs full build")
    .IsDependentOn("Full-Build");

Task("Help")
    .Description("Shows available tasks")
    .Does(() =>
    {
        Information("Available Cake tasks:");
        Information("  Clean           - Cleans bin/obj directories");
        Information("  Restore         - Restores NuGet packages");
        Information("  Build           - Builds the solution");
        Information("  Test            - Runs all tests");
        Information("  Full-Build      - Build + Test");
        Information("  Docker-Build    - Builds API Docker image");
        Information("  Infra-Up        - Starts infrastructure containers");
        Information("  Infra-Down      - Stops infrastructure containers");
        Information("  Add-Migration   - Creates new EF migration (--name=MigrationName)");
        Information("  Migrate         - Runs EF database migrations");
        Information("  Publish         - Publishes API project");
        Information("  Deploy-Local    - Full local deployment");
        Information("  Help            - Shows this help");
        Information("");
        Information("Usage: dotnet cake [--target=TaskName] [--configuration=Debug|Release]");
    });

RunTarget(target);