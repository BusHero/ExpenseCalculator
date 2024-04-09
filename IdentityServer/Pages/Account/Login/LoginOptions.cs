// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

namespace IdentityServer.Pages.Account.Login;

public static class LoginOptions
{
    public const bool AllowLocalLogin = true;
    public const bool AllowRememberLogin = true;
    public readonly static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
    public const string InvalidCredentialsErrorMessage = "Invalid username or password";
}