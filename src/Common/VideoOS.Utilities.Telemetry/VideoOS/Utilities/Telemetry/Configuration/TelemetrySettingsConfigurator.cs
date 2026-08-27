using System;
using System.IO;
using System.Reflection;

namespace VideoOS.Utilities.Telemetry.Configuration;

public static class TelemetrySettingsConfigurator
{
	private const string DEFAULT_FILE_NAME = "TelemetrySettings.json";

	private const string DEFAULT_FOLDER_NAME = "TelemetrySettings";

	public static void CreateSettingsFile(string companyName, string sharedDataFolder = null)
	{
		string text = sharedDataFolder ?? Path.GetFullPath(GetSettingsFolderPath(companyName));
		string path = Path.Combine(text, "TelemetrySettings.json");
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		if (!File.Exists(path))
		{
			File.WriteAllText(path, ReadEmbeddedSettingsFile());
		}
	}

	public static string GetSettingsFilePath(string companyName)
	{
		return Path.GetFullPath(Path.Combine(GetSettingsFolderPath(companyName), "TelemetrySettings.json"));
	}

	private static string GetSettingsFolderPath(string companyName)
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), companyName, "TelemetrySettings");
	}

	private static string ReadEmbeddedSettingsFile()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		string text = "VideoOS.Utilities.Telemetry.TelemetrySettings.json";
		using StreamReader streamReader = new StreamReader(executingAssembly.GetManifestResourceStream(text) ?? throw new FileNotFoundException("Embedded resource '" + text + "' not found in assembly."));
		return streamReader.ReadToEnd();
	}
}
