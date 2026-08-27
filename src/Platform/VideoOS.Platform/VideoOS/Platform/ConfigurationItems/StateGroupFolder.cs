using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class StateGroupFolder : IConfigurationItem
{
	public ICollection<StateGroup> StateGroups
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<StateGroup> list = new List<StateGroup>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "StateGroup")
				{
					list.Add(new StateGroup(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public StateGroupFolder(ServerId serverId, ConfigurationItem stateGroupFolder)
		: base(serverId, stateGroupFolder)
	{
	}

	public StateGroupFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public StateGroupFolder(FQID fqid)
		: base(fqid)
	{
	}

	public StateGroupFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/StateGroupFolder")
	{
	}

	public ServerTask AddStateGroup(string id, string name, Collection<string> states)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddStateGroup");
			configurationApiItemWrapper.SetProperty("Id", id);
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("States", states);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddStateGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StateGroupFolder", "Unable to call method 'AddStateGroup':" + ex.Message);
		}
		return null;
	}

	public AddStateGroupServerTask AddStateGroup()
	{
		try
		{
			return new AddStateGroupServerTask(base.Wrapper.ExecuteMethod("AddStateGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StateGroupFolder", "Unable to call method 'AddStateGroup':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveStateGroup(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveStateGroup");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveStateGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StateGroupFolder", "Unable to call method 'RemoveStateGroup':" + ex.Message);
		}
		return null;
	}

	public RemoveStateGroupServerTask RemoveStateGroup()
	{
		try
		{
			return new RemoveStateGroupServerTask(base.Wrapper.ExecuteMethod("RemoveStateGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "StateGroupFolder", "Unable to call method 'RemoveStateGroup':" + ex.Message);
		}
		return null;
	}
}
