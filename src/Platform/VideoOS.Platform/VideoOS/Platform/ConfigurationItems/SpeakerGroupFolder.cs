using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class SpeakerGroupFolder : IConfigurationItem
{
	public ICollection<SpeakerGroup> SpeakerGroups
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<SpeakerGroup> list = new List<SpeakerGroup>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "SpeakerGroup")
				{
					list.Add(new SpeakerGroup(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public SpeakerGroupFolder(ServerId serverId, ConfigurationItem speakerGroupFolder)
		: base(serverId, speakerGroupFolder)
	{
	}

	public SpeakerGroupFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public SpeakerGroupFolder(FQID fqid)
		: base(fqid)
	{
	}

	public SpeakerGroupFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/SpeakerGroupFolder")
	{
	}

	public ServerTask AddDeviceGroup(string groupName, string groupDescription)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddDeviceGroup");
			configurationApiItemWrapper.SetProperty("GroupName", groupName);
			configurationApiItemWrapper.SetProperty("GroupDescription", groupDescription);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddDeviceGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SpeakerGroupFolder", "Unable to call method 'AddDeviceGroup':" + ex.Message);
		}
		return null;
	}

	public AddDeviceGroupServerTask AddDeviceGroup()
	{
		try
		{
			return new AddDeviceGroupServerTask(base.Wrapper.ExecuteMethod("AddDeviceGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SpeakerGroupFolder", "Unable to call method 'AddDeviceGroup':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveDeviceGroup(bool removeMembers, string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveDeviceGroup");
			configurationApiItemWrapper.SetProperty("RemoveMembers", removeMembers.ToString());
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveDeviceGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SpeakerGroupFolder", "Unable to call method 'RemoveDeviceGroup':" + ex.Message);
		}
		return null;
	}

	public RemoveDeviceGroupServerTask RemoveDeviceGroup()
	{
		try
		{
			return new RemoveDeviceGroupServerTask(base.Wrapper.ExecuteMethod("RemoveDeviceGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SpeakerGroupFolder", "Unable to call method 'RemoveDeviceGroup':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveDeviceGroup(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveDeviceGroup");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveDeviceGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "SpeakerGroupFolder", "Unable to call method 'RemoveDeviceGroup':" + ex.Message);
		}
		return null;
	}
}
