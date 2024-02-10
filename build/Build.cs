using System.Collections.Generic;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    public static int Main () 
        => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - " +
               "Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild 
        ? Configuration.Debug 
        : Configuration.Release;

    readonly AbsolutePath SolutionFile = RootDirectory / "ExpenseManager.sln";

    [UsedImplicitly]
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean(_ => _
                .EnableNoLogo()
                .SetProject(SolutionFile)
                .SetConfiguration(Configuration));
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(SolutionFile));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .EnableNoRestore()
                .EnableNoLogo()
                .SetProjectFile(SolutionFile)
                .SetConfiguration(Configuration));
        });

    [UsedImplicitly]
    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .EnableNoBuild()
                .EnableNoRestore()
                .EnableNoLogo()
                .SetFilter("Category!=Acceptance")
                .SetConfiguration(Configuration));
        });

    [UsedImplicitly]
    Target Start => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetRun(_ => _
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore()
                .SetProjectFile(RootDirectory / "ExpenseManager.Api"));
        });

    List<string> PublishableProjects { get; set; } =
    [
        "ExpenseManager.Api",
        "ExpenseManager",
    ];
    
    [UsedImplicitly]
    Target Publish => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            foreach (var project in PublishableProjects)
            {
                DotNetPublish(_ => _
                    .EnableNoBuild()
                    .EnableNoRestore()
                    .SetConfiguration(Configuration)
                    .SetOutput(RootDirectory / "output" / project)
                    .SetProject(RootDirectory / project));
            }
        });

    [UsedImplicitly]
    Target RunAcceptanceTests => _ => _
        .Executes(() =>
        {   
            DotNetTest(_ => _
                .SetConfiguration(Configuration)
                .SetProjectFile(RootDirectory / "ExpenseManager.AcceptanceTests"));
        });
}