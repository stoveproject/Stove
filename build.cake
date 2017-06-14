#tool "nuget:?package=Cake.CoreCLR";
#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"
#tool "nuget:?package=JetBrains.dotCover.CommandLineTools"
#tool "nuget:?package=NuGet.CommandLine"

#addin "Cake.Json"
#addin "Cake.FileHelpers"

#l "common.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var branch = Argument("branch","dev");
var toolpath = Argument("toolpath", @"tools");

var projectName = "Stove";
var solution = "./" + projectName + ".sln";
var targetTestFramework = "net461";

var testFileRegex = $"**/bin/{configuration}/{targetTestFramework}/*Tests*.dll";
var testProjectNames = new List<string>()
                      {
                          //"Stove.Dapper.Tests",
                          "Stove.EntityFramework.Tests",
                          //"Stove.Hangfire.Tests",
                          "Stove.Mapster.Tests",
                          //"Stove.NHibernate.Tests",
                          "Stove.NLog.Tests",
                          "Stove.RabbitMQ.Tests",
                          "Stove.RavenDB.Tests",
                          "Stove.Redis.Tests",
                          "Stove.Tests",
                          "Stove.Tests.SampleApplication"
                        };

var nupkgPath = "nupkg";
var nupkgRegex = $"**/{projectName}*.nupkg";
var nugetPath = toolpath + "/NuGet.CommandLine/tools/nuget.exe";
var nugetApiKey = EnvironmentVariable("nugetApiKey");
var NUGET_PUSH_SETTINGS = new NuGetPushSettings
                          {
                              ToolPath = File(nugetPath),
                              Source = "https://www.nuget.org/api/v2/package",
                              ApiKey = nugetApiKey
                          };

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
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
        //  XUnit2("./test/One.Tests/**/bin/" + configuration + "/" + targetFramework + "/One.Tests.dll", new XUnit2Settings { });
    });

Task("Coverage")
    .IsDependentOn("Run-Unit-Tests")
    .Does(()=>
    {
      Information("Coverage...");
      /*var coverSettings = new DotCoverCoverSettings()
                              .WithFilter("-:*Tests");

      var coverageResultSsV4 = new FilePath("./dotcover/dotcoverSsV4.data");

      DotCoverCover(ctx =>
                    ctx.XUnit2("./SsV4Test/NodaTime.Serialization.ServiceStackText.UnitTests.dll"),
                        coverageResultSsV4,
                        coverSettings
                   );

      var htmlReportFile = new FilePath("./dotcover/dotcover.html");
      var reportSettings = new DotCoverReportSettings { ReportType = DotCoverReportType.HTML};
      DotCoverReport(mergedData, htmlReportFile, reportSettings);
      StartProcess("powershell", "start file:///" + MakeAbsolute(htmlReportFile));*/
    });

Task("Analyse")
    .IsDependentOn("Coverage")
    .Does(()=>
    {
        Information("Sonar running!...");
        /*var settings = new SonarBeginSettings()
        {
    			Url = sonarQubeServerUrl,
    			Key = sonarQubeKey
  		  };

  		Sonar(
  			ctx => {
  				ctx.MSBuild(solution);
  			}, settings
  		);*/
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
          Information("Publishing... " + nupkgFile);
          NuGetPush(nupkgFile, NUGET_PUSH_SETTINGS);
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
