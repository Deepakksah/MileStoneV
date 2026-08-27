using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.Utilities;

public static class AppSettingsHelper
{
	public static void EnsureOldAppSettingsValues(string oldConfigPath, string newConfigPath, string appSettingsFile)
	{
		try
		{
			string text = File.ReadAllText(oldConfigPath);
			string text2 = File.ReadAllText(newConfigPath);
			JObject oldAppSettings = JObject.Parse(text);
			JObject newAppSettings = JObject.Parse(text2);
			EnsureOldAppSettingsValues(oldAppSettings, newAppSettings, oldConfigPath, appSettingsFile);
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Failed during parsing file: {0}. Exception occurred.\r\nException: {1}", appSettingsFile, arg));
		}
	}

	public static void EnsureOldAppSettingsValues(JObject oldAppSettings, JObject newAppSettings, string destinationPath, string appSettingsFile)
	{
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Started exchanging old apsettings values with new ones."));
		try
		{
			if (oldAppSettings != null && newAppSettings != null && destinationPath != null && appSettingsFile != null)
			{
				SetOldValues((IJEnumerable<JToken>)(object)oldAppSettings, (IJEnumerable<JToken>)(object)newAppSettings);
				File.WriteAllText(destinationPath, ((JToken)newAppSettings).ToString((Formatting)1, Array.Empty<JsonConverter>()));
			}
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Failed to save old values in {0}. Exception occurred.\r\nException: {1}", appSettingsFile, arg));
		}
	}

	private static void SetOldValues(IJEnumerable<JToken> oldValues, IJEnumerable<JToken> newValues)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (oldValues == null || newValues == null)
		{
			return;
		}
		foreach (JToken oldValue in (IEnumerable<JToken>)oldValues)
		{
			JToken val = ((IEnumerable<JToken>)newValues).FirstOrDefault((JToken j) => j.Path == oldValue.Path);
			if (val != null)
			{
				if (oldValue.HasValues)
				{
					SetOldValues((IJEnumerable<JToken>)(object)oldValue.Children(), Extensions.Children<JToken>((IEnumerable<JToken>)newValues));
				}
				else if (oldValue.Type == val.Type)
				{
					val.Replace(oldValue);
				}
			}
		}
	}
}
