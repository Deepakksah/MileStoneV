using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using VideoOS.Installer.Common;
using VideoOS.Installer.UI.Framework.IISSetup;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public static class WebsiteHelper
{
	public const string PrimaryRegistrySubKey = "SOFTWARE\\VideoOS\\Installation\\Server";

	public const string PrimaryRegistrySubKey64 = "SOFTWARE\\Wow6432Node\\VideoOS\\Installation\\Server";

	[SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "This rule has been depricated")]
	public static void UpdateManagementAppSettings(int websiteHttpPort, int websiteHttpsPort, [CallerMemberName] string memberName = "")
	{
		if (websiteHttpPort == 0)
		{
			throw new ArgumentOutOfRangeException("websiteHttpPort");
		}
		if (websiteHttpsPort == 0)
		{
			throw new ArgumentOutOfRangeException("websiteHttpsPort");
		}
		string hostName = Dns.GetHostEntry("localhost").HostName;
		Uri uri = new UriBuilder(Uri.UriSchemeHttp, hostName, websiteHttpPort).Uri;
		Uri uri2 = new UriBuilder(Uri.UriSchemeHttps, hostName, websiteHttpsPort).Uri;
		GlobalData.SetValue("MANAGEMENTAPPNAME", uri.ToString(), memberName);
		GlobalData.SetValue("MANAGEMENTAPPNAMESECURE", uri2.ToString(), memberName);
	}

	public static void CreateRegistryEntries(SiteInfo websiteToUse)
	{
		using (RegistryKey localMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
		{
			CreateRegistryEntries(websiteToUse, localMachineKey);
		}
		using RegistryKey localMachineKey2 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
		CreateRegistryEntries(websiteToUse, localMachineKey2);
	}

	private static void CreateRegistryEntries(SiteInfo websiteToUse, RegistryKey localMachineKey)
	{
		RegistryKey registryKey = localMachineKey.OpenSubKey("SOFTWARE\\VideoOS\\Installation\\Server", writable: true);
		if (registryKey == null)
		{
			registryKey = localMachineKey.CreateSubKey("SOFTWARE\\VideoOS\\Installation\\Server");
		}
		if (registryKey != null)
		{
			registryKey.SetValue("WebSiteId", websiteToUse.Id, RegistryValueKind.String);
			registryKey.SetValue("WebSiteName", websiteToUse.Name, RegistryValueKind.String);
			registryKey.SetValue("WebSiteAddress", websiteToUse.Address, RegistryValueKind.String);
			registryKey.SetValue("WebSitePort", websiteToUse.HttpPort, RegistryValueKind.DWord);
			registryKey.SetValue("WebSiteSecurePort", websiteToUse.HttpsPort, RegistryValueKind.DWord);
			registryKey.Close();
		}
	}

	public static void CreateRegistryEntries(Dictionary<string, string> settings)
	{
		using (RegistryKey localMachineKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
		{
			CreateRegistryEntries(settings, localMachineKey);
		}
		using RegistryKey localMachineKey2 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
		CreateRegistryEntries(settings, localMachineKey2);
	}

	private static void CreateRegistryEntries(Dictionary<string, string> settings, RegistryKey localMachineKey)
	{
		RegistryKey registryKey = localMachineKey.OpenSubKey("SOFTWARE\\VideoOS\\Installation\\Server", writable: true);
		if (registryKey == null)
		{
			registryKey = localMachineKey.CreateSubKey("SOFTWARE\\VideoOS\\Installation\\Server");
		}
		if (registryKey != null)
		{
			registryKey.SetValue("WebSiteId", settings["WebSiteId"], RegistryValueKind.String);
			registryKey.SetValue("WebSiteName", settings["WebSiteName"], RegistryValueKind.String);
			registryKey.SetValue("WebSiteAddress", settings["WebSiteAddress"], RegistryValueKind.String);
			registryKey.SetValue("WebSitePort", settings["WebSitePort"], RegistryValueKind.DWord);
			registryKey.SetValue("WebSiteSecurePort", settings["WebSiteSecurePort"], RegistryValueKind.DWord);
			registryKey.Close();
		}
	}
}
