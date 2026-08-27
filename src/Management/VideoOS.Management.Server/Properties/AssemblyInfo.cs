using System;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Owin;
using VideoOS.Management.Server.RestApi;

[assembly: OwinStartup(typeof(StartupRestApi))]
[assembly: AssemblyTitle("Management Server")]
[assembly: AssemblyDescription("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]
[assembly: Guid("aa4cb6fc-45e0-41ce-b28c-4841bd00367e")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyCompany("Milestone Systems A/S")]
[assembly: AssemblyProduct("VideoOS")]
[assembly: AssemblyCopyright("Copyright © Milestone Systems A/S. All rights reserved")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyInformationalVersion("25.3.17737.1")]
[assembly: AssemblyFileVersion("25.3.17737.1")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyVersion("25.3.0.0")]
