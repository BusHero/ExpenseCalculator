// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServerAspNetIdentity.Pages.Device;

public static class DeviceOptions
{
    public readonly static bool EnableOfflineAccess = true;
    public readonly static string OfflineAccessDisplayName = "Offline Access";
    public readonly static string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

    public readonly static string InvalidUserCode = "Invalid user code";
    public readonly static string MustChooseOneErrorMessage = "You must pick at least one permission";
    public readonly static string InvalidSelectionErrorMessage = "Invalid selection";
}