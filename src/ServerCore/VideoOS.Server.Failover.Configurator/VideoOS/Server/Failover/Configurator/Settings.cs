using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VideoOS.Utilities.Validation;

namespace VideoOS.Server.Failover.Configurator;

public class Settings
{
	private static string SettingsPath => Path.Combine(ProductLocationHelper.CommonApplicationDataFolder, "failoverwizard.json");

	private ILogger Log { get; set; }

	[JsonProperty]
	public bool MaintenanceMode { get; set; }

	[JsonProperty]
	public bool IsHttps { get; set; }

	[JsonProperty]
	public bool IsPrimary { get; set; }

	[JsonProperty]
	public bool IsExternalSqlSetup { get; set; }

	public static Settings Load(ILogger log)
	{
		log.NullCheck("", "Load", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Server.Failover.Configurator\\Settings.cs", 29);
		if (!File.Exists(SettingsPath))
		{
			return new Settings();
		}
		try
		{
			Settings? settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsPath));
			settings.Log = log;
			return settings;
		}
		catch (Exception ex)
		{
			log.LogError("Error loading wizard configuration. {ExceptionMessage}", ex.Message);
			throw;
		}
	}

	private Settings()
	{
	}

	public void Save()
	{
		try
		{
			string contents = JsonConvert.SerializeObject(this);
			File.WriteAllText(SettingsPath, contents);
		}
		catch (Exception ex)
		{
			Log.LogError("Error saving wizard configuration. {ExceptionMessage}", ex.Message);
			throw;
		}
	}
}
