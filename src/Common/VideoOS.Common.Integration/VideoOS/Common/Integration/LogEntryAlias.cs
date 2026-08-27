using System;
using System.Globalization;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration;

public sealed class LogEntryAlias
{
	public enum AliasType
	{
		TriggerTime,
		EventName,
		DeviceName,
		RuleName,
		RecorderName
	}

	public const string TRIGGERTIME = "$TriggerTime$";

	public const string EVENTNAME = "$EventName$";

	public const string DEVICENAME = "$DeviceName$";

	public const string RULENAME = "$RuleName$";

	public const string RECORDERNAME = "$RecorderName$";

	private LogEntryAlias()
	{
	}

	public static string GetAliasAsInvariantCulture(AliasType alias)
	{
		return alias switch
		{
			AliasType.TriggerTime => "$TriggerTime$", 
			AliasType.EventName => "$EventName$", 
			AliasType.DeviceName => "$DeviceName$", 
			AliasType.RuleName => "$RuleName$", 
			AliasType.RecorderName => "$RecorderName$", 
			_ => string.Empty, 
		};
	}

	public static string GetAliasAsCurrentUICulture(AliasType alias)
	{
		return alias switch
		{
			AliasType.TriggerTime => AddAliasDelimitor(Resources.LogEntryAliasTypeTriggerTime), 
			AliasType.EventName => AddAliasDelimitor(Resources.LogEntryAliasTypeEventName), 
			AliasType.DeviceName => AddAliasDelimitor(Resources.LogEntryAliasTypeDeviceName), 
			AliasType.RuleName => AddAliasDelimitor(Resources.LogEntryAliasTypeRuleName), 
			AliasType.RecorderName => AddAliasDelimitor(Resources.LogEntryAliasTypeRecorderName), 
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

	private static string AddAliasDelimitor(string alias)
	{
		return string.Format(CultureInfo.InvariantCulture, "${0}$", alias);
	}
}
