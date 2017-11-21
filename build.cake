#tool "nuget:?package=xunit.runner.console&version=2.3.0-beta4-build3742"

#addin "nuget:?package=NuGet.Core"
#addin "nuget:?package=Cake.ExtendedNuGet"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var projectName = "Stove";
var solution = "./" + projectName + ".sln";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var toolpath = Argument("toolpath", @"tools");
var branch = Argument("branch", EnvironmentVariable("APPVEYOR_REPO_BRANCH"));
var nugetApiKey = EnvironmentVariable("nugetApiKey");
var isRelease = EnvironmentVariable("APPVEYOR_REPO_TAG") == "true";
var isPR = EnvironmentVariable("APPVEYOR_PULL_REQUEST_TITLE") != string.Empty;

var testProjects = new List<Tuple<string, string[]>>
                {
                    new Tuple<string, string[]>("Stove.EntityFramework.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Stove.EntityFrameworkCore.Dapper.Tests", new[] { "netcoreapp2.0" }),
                    new Tuple<string, string[]>("Stove.Mapster.Tests", new[] { "netcoreapp2.0" }),
                    new Tuple<string, string[]>("Stove.NLog.Tests", new[] { "netcoreapp2.0" }),
                    new Tuple<string, string[]>("Stove.RabbitMQ.Tests", new[] { "netcoreapp2.0" }),
                    new Tuple<string, string[]>("Stove.RavenDB.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Stove.Redis.Tests", new[] { "netcoreapp2.0" }),
                    new Tuple<string, string[]>("Stove.Tests", new[] { "netcoreapp2.0" }),
                    new Tuple<string, string[]>("Stove.Tests.SampleApplication", new[] { "net461" }),
                    new Tuple<string, string[]>("Stove.Dapper.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Stove.Hangfire.Tests", new[] { "netcoreapp2.0" }),
                    new Tuple<string, string[]>("Stove.NHibernate.Tests", new[] { "net461" }),
                    new Tuple<string, string[]>("Stove.EntityFrameworkCore.Tests", new[] { "netcoreapp2.0" }),
                    new Tuple<string, string[]>("Stove.Serilog.Tests", new[] { "netcoreapp2.0" })
                };
                      

var nupkgPath = "nupkg";
var nupkgRegex = $"**/{projectName}*.nupkg";
var nugetPath = toolpath + "/nuget.exe";
var nugetQueryUrl = "https://www.nuget.org/api/v2/";
var nugetPushUrl = "https://www.nuget.org/api/v2/package";
var NUGET_PUSH_SETTINGS = new NuGetPushSettings
                          {
                              ToolPath = File(nugetPath),
                              Source = nugetPushUrl,
                              ApiKey = nugetApiKey
                          };

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        Information("Current Branch is:" + EnvironmentVariable("APPVEYOR_REPO_BRANCH"));
        Information("Current Branch is:" + EnvironmentVariable("APPVEYOR_PULL_REQUEST_TITLE"));
        Information($"IsRelase: {isRelease}");
        CleanDirectories("./src/**/bin");
        CleanDirectories("./src/**/obj");
        CleanDirectory(nupkgPath);
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreRestore(solution);
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        MSBuild(solution, new MSBuildSettings(){Configuration = configuration}
                                               .WithProperty("SourceLinkCreate","true"));
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        foreach (Tuple<string, string[]> testProject in testProjects)
        {
            foreach (string targetFramework in testProject.Item2)
            {
                 if(targetFramework == "net461")
                 {
                      var testFile = GetFiles($"**/bin/{configuration}/{targetFramework}/{testProject.Item1}*.dll").First();
                      Information(testFile);
                      XUnit2(testFile.ToString(), new XUnit2Settings { });
                 }
                 else
                 {
                    var testProj = GetFiles($"./test/**/*{testProject.Item1}.csproj").First();
                    DotNetCoreTest(testProj.FullPath, new DotNetCoreTestSettings { Configuration = "Release", Framework = targetFramework });
                 }             
            }
        }
    });
    
Task("Pack")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
    {
        var nupkgFiles = GetFiles(nupkgRegex);
        MoveFiles(nupkgFiles, nupkgPath);
    });

Task("NugetPublish")
    .IsDependentOn("Pack") 
    .Does(()=>
    {
        foreach(var nupkgFile in GetFiles(nupkgRegex))
        {
          if(!IsNuGetPublished(nupkgFile, nugetQueryUrl))
          {
             Information("Publishing... " + nupkgFile);
             NuGetPush(nupkgFile, NUGET_PUSH_SETTINGS);
          }
          else
          {
             Information("Already published, skipping... " + nupkgFile);
          }
        }
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Pack")
    .IsDependentOn("NugetPublish");
    
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
