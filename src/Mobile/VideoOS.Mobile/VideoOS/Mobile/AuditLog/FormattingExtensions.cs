using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.AuditLog;

public static class FormattingExtensions
{
	private const string ListSeparator = ", ";

	private const string OptionAvailable = "(x)";

	private const string OptionMissing = "(-)";

	public static string Format(this bool value)
	{
		if (!value)
		{
			return "(-)";
		}
		return "(x)";
	}

	public static string Format(this DateTime date)
	{
		return date.ToString();
	}

	public static string Format(this IEnumerable<object> source)
	{
		return string.Join(", ", source);
	}

	public static string Format(this ExportType type)
	{
		return type switch
		{
			ExportType.Db => Resources.ExportTypeDb, 
			ExportType.Avi => Resources.ExportTypeAvi, 
			ExportType.Mkv => Resources.ExportTypeMkv, 
			_ => string.Empty, 
		};
	}

	public static string Format(this InvestigationState state)
	{
		return state switch
		{
			InvestigationState.InProgress => Resources.InvestigationStateOngoing, 
			InvestigationState.Completed => Resources.InvestigationStateCompleted, 
			_ => string.Empty, 
		};
	}
}
