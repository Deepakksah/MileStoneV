using System;
using System.Globalization;
using VideoOS.CertificateRegistration.Common;
using VideoOS.Installer.Logging;
using VideoOS.Utilities.Windows.Http;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public static class ServerHttpSysConfigurator
{
	public static void RemoveServerHttpSysConfigurationAndReservation(ushort port)
	{
		LogManager.Write("Removing HTTP.SYS configuration for server service...");
		try
		{
			new HttpSysSettings(WellKnownAppIds.ServerApiService, port).DeleteSettings();
			LogManager.Write("HTTP.SYS configuration for server service removed...");
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Exception occurred.\r\nException: {0}", arg));
		}
	}
}
