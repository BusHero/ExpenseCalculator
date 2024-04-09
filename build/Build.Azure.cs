using System.IO.Compression;
using CliWrap;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;
using Serilog;

sealed partial class Build
{
    [Parameter("The id of the application to deploy to"), Secret]
    Guid? AzureApplicationId { get; set; }

    [Parameter("Tenant Id"), Secret]
    Guid? AzureTenantId { get; set; }

    [Parameter, Secret] string AzureDeploySecret { get; set; } = null!;

    [Parameter, Secret] string ResourceGroup { get; set; } = null!;
    [Parameter, Secret] string AppName { get; set; } = null!;

    [Parameter] string ArtifactsPath { get; set; } = null!;

    AbsolutePath? Artifact { get; set; }

    [UsedImplicitly]
    Target Compress => _ => _
        .Requires(() => ArtifactsPath)
        .Executes(() =>
        {
            Artifact = TemporaryDirectory / $"{Guid.NewGuid()}.zip";
            ZipFile.CreateFromDirectory(RootDirectory / ArtifactsPath, Artifact);
            Log.Information("Zip file created {Path}", Artifact);
        });

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
        .DependsOn(Compress, LoginToAzure)
        .Requires(() => ResourceGroup)
        .Requires(() => AppName)
        .Executes(async () =>
        {
            await Cli.Wrap("az")
                .WithArguments(args => args
                    .Add("webapp")
                    .Add("deploy")
                    .Add("--resource-group").Add(ResourceGroup)
                    .Add("--name").Add(AppName)
                    .Add("--src-path").Add(Artifact?.ToString() ?? ""))
                .WithStandardOutputPipe(PipeTarget.ToDelegate(x => Log.Information("{Msg}", x)))
                .WithStandardErrorPipe(PipeTarget.ToDelegate(x => Log.Debug("{Msg}", x)))
                .ExecuteAsync();
        });
}
