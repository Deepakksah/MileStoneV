using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AlarmDefinitionFolder : IConfigurationItem
{
	public ICollection<AlarmDefinition> AlarmDefinitions
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<AlarmDefinition> list = new List<AlarmDefinition>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "AlarmDefinition")
				{
					list.Add(new AlarmDefinition(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public AlarmDefinitionFolder(ServerId serverId, ConfigurationItem alarmDefinitionFolder)
		: base(serverId, alarmDefinitionFolder)
	{
	}

	public AlarmDefinitionFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AlarmDefinitionFolder(FQID fqid)
		: base(fqid)
	{
	}

	public AlarmDefinitionFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/AlarmDefinitionFolder")
	{
	}

	public ServerTask AddAlarmDefinition(string name, string description, string eventTypeGroup, string eventType, string sourceList, string enableRule, string timeProfile, string enableEventList, string disableEventList, string managementTimeoutTime, string managementTimeoutEventList, string relatedCameraList, string mapType, string relatedMap, string owner, string priority, string category, string triggerEventlist, bool assignableToAdmins)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddAlarmDefinition");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("EventTypeGroup", eventTypeGroup);
			configurationApiItemWrapper.SetProperty("EventType", eventType);
			configurationApiItemWrapper.SetProperty("SourceList", sourceList);
			configurationApiItemWrapper.SetProperty("EnableRule", enableRule);
			configurationApiItemWrapper.SetProperty("TimeProfile", timeProfile);
			configurationApiItemWrapper.SetProperty("EnableEventList", enableEventList);
			configurationApiItemWrapper.SetProperty("DisableEventList", disableEventList);
			configurationApiItemWrapper.SetProperty("ManagementTimeoutTime", managementTimeoutTime);
			configurationApiItemWrapper.SetProperty("ManagementTimeoutEventList", managementTimeoutEventList);
			configurationApiItemWrapper.SetProperty("RelatedCameraList", relatedCameraList);
			configurationApiItemWrapper.SetProperty("MapType", mapType);
			configurationApiItemWrapper.SetProperty("RelatedMap", relatedMap);
			configurationApiItemWrapper.SetProperty("Owner", owner);
			configurationApiItemWrapper.SetProperty("Priority", priority);
			configurationApiItemWrapper.SetProperty("Category", category);
			configurationApiItemWrapper.SetProperty("TriggerEventlist", triggerEventlist);
			configurationApiItemWrapper.SetProperty("AssignableToAdmins", assignableToAdmins.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddAlarmDefinition"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AlarmDefinitionFolder", "Unable to call method 'AddAlarmDefinition':" + ex.Message);
		}
		return null;
	}

	public AddAlarmDefinitionServerTask AddAlarmDefinition()
	{
		try
		{
			return new AddAlarmDefinitionServerTask(base.Wrapper.ExecuteMethod("AddAlarmDefinition"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AlarmDefinitionFolder", "Unable to call method 'AddAlarmDefinition':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveAlarmDefinition(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveAlarmDefinition");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveAlarmDefinition"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AlarmDefinitionFolder", "Unable to call method 'RemoveAlarmDefinition':" + ex.Message);
		}
		return null;
	}

	public RemoveAlarmDefinitionServerTask RemoveAlarmDefinition()
	{
		try
		{
			return new RemoveAlarmDefinitionServerTask(base.Wrapper.ExecuteMethod("RemoveAlarmDefinition"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AlarmDefinitionFolder", "Unable to call method 'RemoveAlarmDefinition':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddAlarmDefinition(string name, string description, string eventTypeGroup, string eventType, string sourceList, string enableRule, string timeProfile, string enableEventList, string disableEventList, string managementTimeoutTime, string managementTimeoutEventList, string relatedCameraList, string relatedMap, string owner, string priority, string category, string triggerEventlist)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddAlarmDefinition");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("EventTypeGroup", eventTypeGroup);
			configurationApiItemWrapper.SetProperty("EventType", eventType);
			configurationApiItemWrapper.SetProperty("SourceList", sourceList);
			configurationApiItemWrapper.SetProperty("EnableRule", enableRule);
			configurationApiItemWrapper.SetProperty("TimeProfile", timeProfile);
			configurationApiItemWrapper.SetProperty("EnableEventList", enableEventList);
			configurationApiItemWrapper.SetProperty("DisableEventList", disableEventList);
			configurationApiItemWrapper.SetProperty("ManagementTimeoutTime", managementTimeoutTime);
			configurationApiItemWrapper.SetProperty("ManagementTimeoutEventList", managementTimeoutEventList);
			configurationApiItemWrapper.SetProperty("RelatedCameraList", relatedCameraList);
			configurationApiItemWrapper.SetProperty("RelatedMap", relatedMap);
			configurationApiItemWrapper.SetProperty("Owner", owner);
			configurationApiItemWrapper.SetProperty("Priority", priority);
			configurationApiItemWrapper.SetProperty("Category", category);
			configurationApiItemWrapper.SetProperty("TriggerEventlist", triggerEventlist);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddAlarmDefinition"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AlarmDefinitionFolder", "Unable to call method 'AddAlarmDefinition':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddAlarmDefinition(string name, string description, string eventTypeGroup, string eventType, string sourceList, string enableRule, string timeProfile, string enableEventList, string disableEventList, string managementTimeoutTime, string managementTimeoutEventList, string relatedCameraList, string mapType, string relatedMap, string owner, string priority, string category, string triggerEventlist)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddAlarmDefinition");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("EventTypeGroup", eventTypeGroup);
			configurationApiItemWrapper.SetProperty("EventType", eventType);
			configurationApiItemWrapper.SetProperty("SourceList", sourceList);
			configurationApiItemWrapper.SetProperty("EnableRule", enableRule);
			configurationApiItemWrapper.SetProperty("TimeProfile", timeProfile);
			configurationApiItemWrapper.SetProperty("EnableEventList", enableEventList);
			configurationApiItemWrapper.SetProperty("DisableEventList", disableEventList);
			configurationApiItemWrapper.SetProperty("ManagementTimeoutTime", managementTimeoutTime);
			configurationApiItemWrapper.SetProperty("ManagementTimeoutEventList", managementTimeoutEventList);
			configurationApiItemWrapper.SetProperty("RelatedCameraList", relatedCameraList);
			configurationApiItemWrapper.SetProperty("MapType", mapType);
			configurationApiItemWrapper.SetProperty("RelatedMap", relatedMap);
			configurationApiItemWrapper.SetProperty("Owner", owner);
			configurationApiItemWrapper.SetProperty("Priority", priority);
			configurationApiItemWrapper.SetProperty("Category", category);
			configurationApiItemWrapper.SetProperty("TriggerEventlist", triggerEventlist);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddAlarmDefinition"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AlarmDefinitionFolder", "Unable to call method 'AddAlarmDefinition':" + ex.Message);
		}
		return null;
	}
}
