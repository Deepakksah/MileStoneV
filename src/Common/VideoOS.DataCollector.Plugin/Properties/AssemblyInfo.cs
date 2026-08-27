using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

[assembly: SuppressMessage("Naming", "CA2204:Literals should be spelled correctly", Justification = "The name is spelled correctly", MessageId = "DataCollector", Scope = "member", Target = "VideoOS.DataCollector.Plugin.DataCollectorTaskInstall.#DoRun()")]
[assembly: SuppressMessage("Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "VideoOS.DataCollector.Plugin.DataCollectorTaskUninstall.#CheckPrequisite(System.String,System.Collections.Generic.List`1<VideoOS.Installer.Interfaces.IPackageParameter>)", Justification = "Legacy code")]
[assembly: SuppressMessage("Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "VideoOS.DataCollector.Plugin.DataCollectorTaskUninstall.#Finalize(System.Collections.Generic.List`1<VideoOS.Installer.Interfaces.IPackageParameter>)", Justification = "Legacy code")]
[assembly: SuppressMessage("Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "VideoOS.DataCollector.Plugin.DataCollectorTaskUninstall.#Initialize(System.Collections.Generic.List`1<VideoOS.Installer.Interfaces.IPackageParameter>)", Justification = "Legacy code")]
[assembly: SuppressMessage("Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "VideoOS.DataCollector.Plugin.DataCollectorTaskUninstall.#Install(System.String,System.Collections.Generic.List`1<VideoOS.Installer.Interfaces.IPackageParameter>)", Justification = "Legacy code")]
[assembly: SuppressMessage("Design", "CA1002:DoNotExposeGenericLists", Scope = "member", Target = "VideoOS.DataCollector.Plugin.DataCollectorTaskUninstall.#Uninstall(System.String,System.Collections.Generic.List`1<VideoOS.Installer.Interfaces.IPackageParameter>)", Justification = "Legacy code")]
[assembly: SuppressMessage("Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "VideoOS.DataCollector.Plugin.DataCollectorConfigurator.#RunUninstall()", Justification = "Legacy code")]
[assembly: SuppressMessage("Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "VideoOS.DataCollector.Plugin.OemResourcesHandler.#CreateOemResource(System.Xml.XmlDocument)", Justification = "Legacy code")]
[assembly: SuppressMessage("Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "VideoOS.DataCollector.Plugin.OemResourcesHandler.#LoadResources()", Justification = "Legacy code")]
[assembly: SuppressMessage(" Security", "CA3075:Unsafe overload of 'Load' method (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca3075)", Justification = "<Pending>", Scope = "member", Target = "~M:VideoOS.DataCollector.Plugin.OemResourcesHandler.LoadResources~System.Xml.XmlDocument")]
[assembly: SuppressMessage("Performance", "CA1852:Type 'DataCollectorUpdater' can be sealed because it has no subtypes in its containing assembly and is not externally visible (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1852)", Justification = "<Pending>", Scope = "type", Target = "~T:VideoOS.DataCollector.Plugin.DataCollectorUpdater")]
[assembly: SuppressMessage("Performance", "CA1852:Type 'OemResourcesHandler' can be sealed because it has no subtypes in its containing assembly and is not externally visible (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1852)", Justification = "<Pending>", Scope = "type", Target = "~T:VideoOS.DataCollector.Plugin.OemResourcesHandler")]
[assembly: SuppressMessage("Usage", "CA2201:Exception type System.Exception is not sufficiently specific (https://learn.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca2201)", Justification = "<Pending>", Scope = "member", Target = "~M:VideoOS.DataCollector.Plugin.DataCollectorConfigurator.RunInstall(System.Boolean,Newtonsoft.Json.Linq.JObject)")]
[assembly: AssemblyTitle("VideoOS.DataCollector.Plugin")]
[assembly: AssemblyDescription("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]
[assembly: Guid("ee2ad259-d240-49b8-93cb-8c585abd7ac1")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: AssemblyCompany("Milestone Systems A/S")]
[assembly: AssemblyProduct("VideoOS")]
[assembly: AssemblyCopyright("Copyright © Milestone Systems A/S. All rights reserved")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyInformationalVersion("25.3.17737.1")]
[assembly: AssemblyFileVersion("25.3.17737.1")]
[assembly: AssemblyConfiguration("Release")]
[assembly: AssemblyVersion("25.3.0.0")]
