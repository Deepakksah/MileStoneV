using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class InputEventFolder : IConfigurationItem
{
	public ICollection<InputEvent> InputEvents
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<InputEvent> list = new List<InputEvent>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "InputEvent")
				{
					list.Add(new InputEvent(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public InputEventFolder(ServerId serverId, ConfigurationItem inputEventFolder)
		: base(serverId, inputEventFolder)
	{
	}

	public InputEventFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public InputEventFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddDeviceGroupMember(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddDeviceGroupMember");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddDeviceGroupMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "InputEventFolder", "Unable to call method 'AddDeviceGroupMember':" + ex.Message);
		}
		return null;
	}

	public AddDeviceGroupMemberServerTask AddDeviceGroupMember()
	{
		try
		{
			return new AddDeviceGroupMemberServerTask(base.Wrapper.ExecuteMethod("AddDeviceGroupMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "InputEventFolder", "Unable to call method 'AddDeviceGroupMember':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveDeviceGroupMember(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveDeviceGroupMember");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveDeviceGroupMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "InputEventFolder", "Unable to call method 'RemoveDeviceGroupMember':" + ex.Message);
		}
		return null;
	}

	public RemoveDeviceGroupMemberServerTask RemoveDeviceGroupMember()
	{
		try
		{
			return new RemoveDeviceGroupMemberServerTask(base.Wrapper.ExecuteMethod("RemoveDeviceGroupMember"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "InputEventFolder", "Unable to call method 'RemoveDeviceGroupMember':" + ex.Message);
		}
		return null;
	}
}
