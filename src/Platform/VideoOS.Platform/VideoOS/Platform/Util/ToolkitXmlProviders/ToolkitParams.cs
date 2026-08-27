using System.Collections.Generic;

namespace VideoOS.Platform.Util.ToolkitXmlProviders;

public static class ToolkitParams
{
	public const string Unknown = "unknown";

	public static readonly Dictionary<ToolkitTypes, string> ToolkitTypesNames = new Dictionary<ToolkitTypes, string>
	{
		{
			ToolkitTypes.Unknown,
			"unknown"
		},
		{
			ToolkitTypes.Source,
			"source"
		},
		{
			ToolkitTypes.Export,
			"export"
		},
		{
			ToolkitTypes.Generic,
			"generic"
		}
	};

	public static readonly Dictionary<ToolkitProviders, string> ToolkitProvidersNames = new Dictionary<ToolkitProviders, string>
	{
		{
			ToolkitProviders.Unknown,
			"unknown"
		},
		{
			ToolkitProviders.IsSource,
			"is"
		},
		{
			ToolkitProviders.Fork,
			"mfork"
		},
		{
			ToolkitProviders.MediaProcessing,
			"mmp"
		},
		{
			ToolkitProviders.MetaData,
			"mtd"
		},
		{
			ToolkitProviders.Media025,
			"mm025"
		},
		{
			ToolkitProviders.MediaServer,
			"mpms"
		}
	};

	private static string FindName<T>(T key, Dictionary<T, string> dictionary)
	{
		if (dictionary.ContainsKey(key))
		{
			return dictionary[key];
		}
		return "unknown";
	}

	public static string NameFromTypes(ToolkitTypes toolkitTypes)
	{
		return FindName(toolkitTypes, ToolkitTypesNames);
	}

	public static string NameFromProviders(ToolkitProviders toolkitProviders)
	{
		return FindName(toolkitProviders, ToolkitProvidersNames);
	}
}
