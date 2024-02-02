using System;
using System.IO.Compression;
using CliWrap;
using JetBrains.Annotations;
using Nuke.Common;
using Serilog;

partial class Build
{
    Target Compress => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            ZipFile.CreateFromDirectory(RootDirectory / "output" / "publish", RootDirectory / "publish.zip"); 
            Log.Information("Zip file created {Path}", RootDirectory / "publish.zip");
        });

    
    [Parameter("The id of the application to deploy to"), Secret]
    Guid ApplicationId { get; set; }

    [Parameter("Tenant Id"), Secret]
    Guid TenantId { get; set; }

    [Parameter, Secret]
    string Password { get; set; }

    [UsedImplicitly]
    Target LoginToAzure => _ => _
        .Executes(async () =>
        {
            await Cli.Wrap("az")
                .WithArguments(args => args
                    .Add("login")
                    .Add("--service-principal")
                    .Add("--user").Add(ApplicationId)
                    .Add("--password").Add(Password)
                    .Add("--tenant").Add(TenantId))
                .WithStandardOutputPipe(PipeTarget.ToDelegate(x => Log.Information("{MSG}", x)))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(x => Log.Debug("{MSG}", x)))
                .ExecuteAsync();
        });
    
    [UsedImplicitly]
    Target DeployToAzure => _ => _
        .DependsOn(LoginToAzure)
        .Executes(async () =>
        {
            await Cli.Wrap("az")
                .WithArguments(args => args
                    .Add("webapp")
                    .Add("deploy")
                    .Add(["--resource-group", "bus1heroDevEnvSetup"])
                    .Add(["--name", "bus1hero"])
                    .Add(["--src-path", RootDirectory / "publish.zip"])
                )
                .WithStandardOutputPipe(PipeTarget.ToDelegate(x => Log.Information("{MSG}", x)))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(x => Log.Debug("{MSG}", x)))
                .ExecuteAsync();
        });
}