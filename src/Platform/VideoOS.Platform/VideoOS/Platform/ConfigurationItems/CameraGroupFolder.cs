using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class CameraGroupFolder : IConfigurationItem
{
	public ICollection<CameraGroup> CameraGroups
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<CameraGroup> list = new List<CameraGroup>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "CameraGroup")
				{
					list.Add(new CameraGroup(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public CameraGroupFolder(ServerId serverId, ConfigurationItem cameraGroupFolder)
		: base(serverId, cameraGroupFolder)
	{
	}

	public CameraGroupFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public CameraGroupFolder(FQID fqid)
		: base(fqid)
	{
	}

	public CameraGroupFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/CameraGroupFolder")
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
			EnvironmentManager.Instance.Log(error: false, "CameraGroupFolder", "Unable to call method 'AddDeviceGroup':" + ex.Message);
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
			EnvironmentManager.Instance.Log(error: false, "CameraGroupFolder", "Unable to call method 'AddDeviceGroup':" + ex.Message);
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
			EnvironmentManager.Instance.Log(error: false, "CameraGroupFolder", "Unable to call method 'RemoveDeviceGroup':" + ex.Message);
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
			EnvironmentManager.Instance.Log(error: false, "CameraGroupFolder", "Unable to call method 'RemoveDeviceGroup':" + ex.Message);
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
			EnvironmentManager.Instance.Log(error: false, "CameraGroupFolder", "Unable to call method 'RemoveDeviceGroup':" + ex.Message);
		}
		return null;
	}
}
