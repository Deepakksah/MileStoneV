using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RecordingServerFailoverPropertyDefinition
{
	public const string PropertyPrimaryFailoverGroupKey = "PrimaryFailoverGroup";

	public const string PropertySecondaryFailoverGroupKey = "SecondaryFailoverGroup";

	public const string PropertyHotStandbyKey = "HotStandby";

	public const string PropertyFailoverPortKey = "FailoverPort";

	public static Property PrimaryFailoverGroupProperty(string value, FailoverServerInformation failoverInfo)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add(ConfigurationItemPath.CreateItemPath("FailoverGroup", Guid.Empty.ToString()).Text, ConfigurationApiResources.GroupNotUsed);
		foreach (ServerGroup failoverGroup in failoverInfo.FailoverGroups)
		{
			dictionary.Add(ConfigurationItemPath.CreateItemPath("FailoverGroup", failoverGroup.Id).Text, failoverGroup.Name);
		}
		return GeneralPropertyDefinitions.GetEnumProperty("PrimaryFailoverGroup", ConfigurationItemPath.CreateItemPath("FailoverGroup", value).Text, ConfigurationApiResources.PropertyPrimaryFailoverGroup, "PropertyPrimaryFailoverGroup", "", dictionary);
	}

	public static Property SecondaryFailoverGroupProperty(string value, FailoverServerInformation failoverInfo)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add(ConfigurationItemPath.CreateItemPath("FailoverGroup", Guid.Empty.ToString()).Text, ConfigurationApiResources.GroupNotUsed);
		foreach (ServerGroup failoverGroup in failoverInfo.FailoverGroups)
		{
			dictionary.Add(ConfigurationItemPath.CreateItemPath("FailoverGroup", failoverGroup.Id).Text, failoverGroup.Name);
		}
		return GeneralPropertyDefinitions.GetEnumProperty("SecondaryFailoverGroup", ConfigurationItemPath.CreateItemPath("FailoverGroup", value).Text, ConfigurationApiResources.PropertySecondaryFailoverGroup, "PropertySecondaryFailoverGroup", "", dictionary);
	}

	public static Property HotStandbyProperty(FailoverServerInformation failoverInfo)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("", ConfigurationApiResources.GroupNotUsed);
		foreach (FailoverServer unusedFailoverServer in failoverInfo.UnusedFailoverServers)
		{
			dictionary.Add(ConfigurationItemPath.CreateItemPath("FailoverRecorder", unusedFailoverServer.Id).Text, unusedFailoverServer.Name);
		}
		string value = "";
		if (failoverInfo.HotStandbyForCurrent != null)
		{
			value = ConfigurationItemPath.CreateItemPath("FailoverRecorder", failoverInfo.HotStandbyForCurrent.Id).Text;
		}
		return GeneralPropertyDefinitions.GetEnumProperty("HotStandby", value, ConfigurationApiResources.PropertyHotStandby, "PropertyHotStandby", "", dictionary);
	}

	public static Property FailoverPortProperty(int value)
	{
		return new Property("FailoverPort", PropertyTypesHelper.GetValue(value))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyFailoverPort,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFailoverPort",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}
}
