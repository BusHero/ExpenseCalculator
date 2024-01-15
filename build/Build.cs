using Nuke.Common;

class Build : NukeBuild
{
    public static int Main () 
        => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration 
        = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Clean => x => x
        .Before(Restore)
        .Executes(() =>
        {
        });

    Target Restore => x => x
        .Executes(() =>
        {
        });

    Target Compile => x => x
        .DependsOn(Restore)
        .Executes(() =>
        {
        });

}
