using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Builders;

public abstract class RuleModelBuilderBase
{
	private static readonly HashSet<string> GroupItemTypes = new HashSet<string> { "CameraGroup", "MicrophoneGroup", "SpeakerGroup", "InputEventGroup", "OutputGroup", "MetadataGroup", "AccessControlRuleGroup", "MIPKind" };

	protected static IEnumerable<ConfigurationItemPath> ExtractPaths(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return new ConfigurationItemPath[0];
		}
		return (from v in value.Split(';')
			select TryCreatePath(v.Trim())).ToArray();
	}

	protected static IEnumerable<ConfigurationItemPath> ExtractPaths(ConfigurationItem item, string propertyName)
	{
		return ExtractPaths(item.Properties.FirstOrDefault((Property x) => x.Key == propertyName)?.Value ?? string.Empty);
	}

	protected static ConfigurationItemPath TryCreatePath(string pathCandidate)
	{
		try
		{
			return ConfigurationItemPath.CreatePath(pathCandidate);
		}
		catch (ArgumentException)
		{
			return new ConfigurationItemPath(isFolder: false, "", "", pathCandidate, "");
		}
	}

	protected static bool IsGroup(string itemType)
	{
		if (string.IsNullOrEmpty(itemType))
		{
			return false;
		}
		return GroupItemTypes.Contains(itemType);
	}
}
