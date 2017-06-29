#tool "nuget:?package=Cake.CoreCLR";
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"
#tool "nuget:?package=JetBrains.dotCover.CommandLineTools"
#tool "nuget:?package=NuGet.CommandLine"

#addin "Cake.Json"
#addin "Cake.FileHelpers"
#addin "nuget:?package=NuGet.Core"
#addin "nuget:?package=Cake.ExtendedNuGet"

#l "common.cake"

using NuGet;

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

var targetTestFramework = "net461";
var testFileRegex = $"**/bin/{configuration}/{targetTestFramework}/*Tests*.dll";
var testProjectNames = new List<string>()
                      {
                          "Stove.EntityFramework.Tests",
                          "Stove.Mapster.Tests",
                          "Stove.NLog.Tests",
                          "Stove.RabbitMQ.Tests",
                          "Stove.RavenDB.Tests",
                          "Stove.Redis.Tests",
                          "Stove.Tests",
                          "Stove.Tests.SampleApplication"
                          //"Stove.Dapper.Tests",
                          //"Stove.Hangfire.Tests",
                          //"Stove.NHibernate.Tests",
                        };

var nupkgPath = "nupkg";
var nupkgRegex = $"**/{projectName}*.nupkg";
var nugetPath = toolpath + "/NuGet.CommandLine/tools/nuget.exe";
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
        CleanDirectories("./src/**/bin");
        CleanDirectories("./src/**/obj");
        CleanDirectory(nupkgPath);
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore(solution, new NuGetRestoreSettings
                  	{
                  		NoCache = true,
                  		Verbosity = NuGetVerbosity.Detailed,
                  		ToolPath = nugetPath
                  	});
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        DotNetBuild(solution, c=> c.Configuration = configuration);
    });

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        foreach(var testProject in testProjectNames)
        {
           var testFile = GetFiles($"**/bin/{configuration}/{targetTestFramework}/{testProject}*.dll").First();
           Information(testFile);
           XUnit2(testFile.ToString(), new XUnit2Settings { });
        }
    });

Task("Coverage")
    .IsDependentOn("Run-Unit-Tests")
    .Does(()=>
    {
      Information("Coverage...");
    });

Task("Analyse")
    .IsDependentOn("Coverage")
    .Does(()=>
    {
        Information("Sonar running!...");
    });

Task("Pack")
    .IsDependentOn("Analyse")
    .Does(() =>
    {
        var nupkgFiles = GetFiles(nupkgRegex);
        MoveFiles(nupkgFiles, nupkgPath);
    });

Task("NugetPublish")
    .IsDependentOn("Pack")
    .WithCriteria(() => branch == "master")
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
