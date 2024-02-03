using System;
using System.IO.Compression;
using CliWrap;
using JetBrains.Annotations;
using Nuke.Common;
using Serilog;

partial class Build
{
    [UsedImplicitly]
    Target Compress => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            ZipFile.CreateFromDirectory(RootDirectory / "output" / "publish", RootDirectory / "publish.zip"); 
            Log.Information("Zip file created {Path}", RootDirectory / "publish.zip");
        });

    
    [Parameter("The id of the application to deploy to"), Secret]
    Guid? AzureApplicationId { get; set; }

    [Parameter("Tenant Id"), Secret]
    Guid? AzureTenantId { get; set; }

    [Parameter, Secret] string AzureDeploySecret { get; set; }
    
    [Parameter] string Artifact { get; set; }

    [UsedImplicitly]
    Target LoginToAzure => _ => _
        .Requires(() => AzureDeploySecret)
        .Requires(() => AzureApplicationId)
        .Requires(() => AzureTenantId)
        .Executes(async () =>
        {
            await Cli.Wrap("az")
                .WithArguments(args => args
                    .Add("login")
                    .Add("--service-principal")
                    .Add("--user").Add(AzureApplicationId!)
                    .Add("--password").Add(AzureDeploySecret)
                    .Add("--tenant").Add(AzureTenantId!))
                .WithStandardOutputPipe(PipeTarget.ToDelegate(x => Log.Information("{Msg}", x)))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(x => Log.Debug("{Msg}", x)))
                .ExecuteAsync();
        });
    
    [UsedImplicitly]
    Target DeployToAzure => _ => _
        .DependsOn(LoginToAzure)
        .Requires(() => Artifact)
        .Executes(async () =>
        {
            await Cli.Wrap("az")
                .WithArguments(args => args
                    .Add("webapp")
                    .Add("deploy")
                    .Add(["--resource-group", "bus1heroDevEnvSetup"])
                    .Add(["--name", "bus1hero"])
                    .Add(["--src-path", Artifact])
                )
                .WithStandardOutputPipe(PipeTarget.ToDelegate(x => Log.Information("{Msg}", x)))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(x => Log.Debug("{Msg}", x)))
                .ExecuteAsync();
        });
}