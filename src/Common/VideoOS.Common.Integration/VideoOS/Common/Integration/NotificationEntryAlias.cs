using System;
using System.Globalization;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration;

public sealed class NotificationEntryAlias
{
	public enum AliasType
	{
		TriggerTime,
		DeviceName,
		HardwareName,
		RuleName,
		RecorderName,
		AccessUnitName
	}

	public const string TRIGGERTIME = "$TriggerTime$";

	public const string DEVICENAME = "$DeviceName$";

	public const string HARDWARENAME = "$HardwareName$";

	public const string RULENAME = "$RuleName$";

	public const string RECORDERNAME = "$RecorderName$";

	public const string ACCCESSUNITNAME = "$AccessUnitName$";

	private NotificationEntryAlias()
	{
	}

	public static string GetAliasAsInvariantCulture(AliasType alias)
	{
		return alias switch
		{
			AliasType.TriggerTime => "$TriggerTime$", 
			AliasType.DeviceName => "$DeviceName$", 
			AliasType.HardwareName => "$HardwareName$", 
			AliasType.RuleName => "$RuleName$", 
			AliasType.RecorderName => "$RecorderName$", 
			AliasType.AccessUnitName => "$AccessUnitName$", 
			_ => string.Empty, 
		};
	}

	public static string GetAliasAsCurrentUICulture(AliasType alias)
	{
		return alias switch
		{
			AliasType.TriggerTime => AddAliasDelimiter(Resources.NotificationEntryAliasTriggerTime), 
			AliasType.DeviceName => AddAliasDelimiter(Resources.NotificationEntryAliasDeviceName), 
			AliasType.HardwareName => AddAliasDelimiter(Resources.NotificationEntryAliasHardwareName), 
			AliasType.RuleName => AddAliasDelimiter(Resources.NotificationEntryAliasRuleName), 
			AliasType.RecorderName => AddAliasDelimiter(Resources.NotificationEntryAliasRecorderName), 
			AliasType.AccessUnitName => AddAliasDelimiter(Resources.NotificationEntryAliasAccessUnitName), 
			_ => string.Empty, 
		};
	}

	public static string ConvertToInvariantCulture(string currentUICultureValue)
	{
		string text = currentUICultureValue;
		foreach (AliasType value in Enum.GetValues(typeof(AliasType)))
		{
			text = text.Replace(GetAliasAsCurrentUICulture(value), GetAliasAsInvariantCulture(value));
		}
		return text;
	}

	public static string ConvertToCurrentUICulture(string invariantCultureValue)
	{
		string text = invariantCultureValue;
		foreach (AliasType value in Enum.GetValues(typeof(AliasType)))
		{
			text = text.Replace(GetAliasAsInvariantCulture(value), GetAliasAsCurrentUICulture(value));
		}
		return text;
	}

	private static string AddAliasDelimiter(string alias)
	{
		return string.Format(CultureInfo.InvariantCulture, "${0}$", alias);
	}
}
