using System;
using VideoOS.Common.Integration;
using VideoOS.Installer.Logging;
using VideoOS.Utilities.Telemetry.Configuration;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public static class TelemetrySettingsUpdater
{
	public static void CreateSettingsFile()
	{
		LogManager.Write("CreateSettingsFile(): Starting...");
		try
		{
			TelemetrySettingsConfigurator.CreateSettingsFile(OemResources.Instance.App.CompanyName);
		}
		catch (Exception inner)
		{
			throw new ServerConfigFileUpdaterFailedException("CreateSettingsFile(): Creating telemetry settings file failed", inner);
		}
		LogManager.Write("CreateSettingsFile(): File created");
	}
}
