using System;
using System.Globalization;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration;

public sealed class BookmarkEntryAlias
{
	public enum AliasType
	{
		TriggerTime,
		EventName,
		DeviceName,
		RuleName,
		RecorderName
	}

	public const string TriggerTime = "$TriggerTime$";

	public const string EventName = "$EventName$";

	public const string DeviceName = "$DeviceName$";

	public const string RuleName = "$RuleName$";

	public const string RecorderName = "$RecorderName$";

	private BookmarkEntryAlias()
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
			AliasType.TriggerTime => AddAliasDelimitor(Resources.BookmarkEntryAliasTriggerTime), 
			AliasType.EventName => AddAliasDelimitor(Resources.BookmarkEntryAliasEventName), 
			AliasType.DeviceName => AddAliasDelimitor(Resources.BookmarkEntryAliasDeviceName), 
			AliasType.RuleName => AddAliasDelimitor(Resources.BookmarkEntryAliasRuleName), 
			AliasType.RecorderName => AddAliasDelimitor(Resources.BookmarkEntryAliasRecorderName), 
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
