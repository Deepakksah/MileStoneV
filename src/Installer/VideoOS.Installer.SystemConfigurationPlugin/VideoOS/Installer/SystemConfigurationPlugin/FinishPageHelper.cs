using System;
using System.Net;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public static class FinishPageHelper
{
	public static string LookupWebClientAddress(IInstallerManager installerManager)
	{
		if (installerManager == null)
		{
			throw new ArgumentNullException("installerManager");
		}
		string url = LookupManagementServerAddress(installerManager);
		string value = installerManager.Parameters["HTTPPORT"];
		return UriHandling.CreateHttpUri(url, UriHandling.ConvertStringToPort(value)).AbsoluteUri;
	}

	public static string LookupMobileClientAddress(IInstallerManager installerManager)
	{
		return UriHandling.CreateHttpUri(LookupManagementServerAddress(installerManager), useDefaultPort: false).AbsoluteUri;
	}

	private static string LookupManagementServerAddress(IInstallerManager installerManager)
	{
		string text = installerManager.Parameters["MANAGEMENTAPPNAME"];
		if (Uri.TryCreate(text, UriKind.Absolute, out var result))
		{
			text = ((string.Compare(result.Host, "localhost", StringComparison.OrdinalIgnoreCase) != 0) ? result.Host : Dns.GetHostEntry("localhost").HostName);
		}
		return text;
	}
}
