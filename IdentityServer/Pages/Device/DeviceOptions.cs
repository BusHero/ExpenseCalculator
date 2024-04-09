// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Device;

public static class DeviceOptions
{
    public const bool EnableOfflineAccess = true;
    public const string OfflineAccessDisplayName = "Offline Access";
    public const string OfflineAccessDescription = "Access to your applications and resources, even when you are offline";

    public const string InvalidUserCode = "Invalid user code";
    public readonly static string MustChooseOneErrorMessage = "You must pick at least one permission";
    public readonly static string InvalidSelectionErrorMessage = "Invalid selection";
}