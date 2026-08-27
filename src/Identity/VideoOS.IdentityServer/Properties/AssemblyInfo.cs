using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Razor.Hosting;
using VideoOS.IdentityServer.Pages;
using VideoOS.IdentityServer.Pages.Account;
using VideoOS.IdentityServer.Pages.Account.Login;
using VideoOS.IdentityServer.Pages.Account.Logout;
using VideoOS.IdentityServer.Pages.Account.Manage;
using VideoOS.IdentityServer.Pages.ExternalLogin;
using VideoOS.IdentityServer.Pages.Shared;

[assembly: ComVisible(false)]
[assembly: Guid("1d3e6cb9-6c2f-4839-9899-f99eb8bc25f6")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyCompany("Milestone Systems A/S")]
[assembly: AssemblyProduct("VideoOS")]
[assembly: AssemblyCopyright("Copyright © Milestone Systems A/S. All rights reserved")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyInformationalVersion("25.3.17737.1")]
[assembly: AssemblyFileVersion("25.3.17737.1")]
[assembly: AssemblyConfiguration("Release")]
[assembly: ApplicationPart("FluentValidation.AspNetCore")]
[assembly: ApplicationPart("Microsoft.FeatureManagement.AspNetCore")]
[assembly: ProvideApplicationPartFactory("Microsoft.AspNetCore.Mvc.ApplicationParts.ConsolidatedAssemblyApplicationPartFactory, Microsoft.AspNetCore.Mvc.Razor")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_Account_LoginSuccess), "mvc.1.0.razor-page", "/Areas/Identity/Pages/Account/LoginSuccess.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_Account_Login_Index), "mvc.1.0.razor-page", "/Areas/Identity/Pages/Account/Login/Index.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_Account_Logout_Index), "mvc.1.0.razor-page", "/Areas/Identity/Pages/Account/Logout/Index.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_Account_Logout_LoggedOut), "mvc.1.0.razor-page", "/Areas/Identity/Pages/Account/Logout/LoggedOut.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_Account_Manage_ChangePassword), "mvc.1.0.razor-page", "/Areas/Identity/Pages/Account/Manage/ChangePassword.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_Account_Manage_PasswordChanged), "mvc.1.0.razor-page", "/Areas/Identity/Pages/Account/Manage/PasswordChanged.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_Error), "mvc.1.0.razor-page", "/Areas/Identity/Pages/Error.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_ExternalLogin_Callback), "mvc.1.0.razor-page", "/Areas/Identity/Pages/ExternalLogin/Callback.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_ExternalLogin_Challenge), "mvc.1.0.razor-page", "/Areas/Identity/Pages/ExternalLogin/Challenge.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_ExternalLogin_WindowsChallenge), "mvc.1.0.razor-page", "/Areas/Identity/Pages/ExternalLogin/WindowsChallenge.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages_Shared__Layout), "mvc.1.0.view", "/Areas/Identity/Pages/Shared/_Layout.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages__ViewImports), "mvc.1.0.view", "/Areas/Identity/Pages/_ViewImports.cshtml")]
[assembly: RazorCompiledItem(typeof(Areas_Identity_Pages__ViewStart), "mvc.1.0.view", "/Areas/Identity/Pages/_ViewStart.cshtml")]
[assembly: AssemblyVersion("1.0.0.0")]
