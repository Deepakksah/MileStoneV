using System;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;

namespace VideoOS.DataCollector.Plugin;

public class DataCollectorUpdater
{
	private const string AppSettingsFile = "appsettings.json";

	public static void EnsureOldDataCollectorAppSettingsValues(string installDir, JObject oldFileAppsettings)
	{
		string appsettingsPathToFile = GetAppsettingsPathToFile(installDir);
		if (appsettingsPathToFile != null)
		{
			try
			{
				JObject newAppSettings = JObject.Parse(File.ReadAllText(appsettingsPathToFile));
				AppSettingsHelper.EnsureOldAppSettingsValues(oldFileAppsettings, newAppSettings, appsettingsPathToFile, "appsettings.json");
			}
			catch (Exception arg)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Exception occurred.\r\nException: {0}", arg));
			}
		}
	}

	public static string GetAppsettingsPathToFile(string installDir)
	{
		string text = Path.Combine(installDir, "appsettings.json");
		if (!File.Exists(text))
		{
			return null;
		}
		return text;
	}
}
