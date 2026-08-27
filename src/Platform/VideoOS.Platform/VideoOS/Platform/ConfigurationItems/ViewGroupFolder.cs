using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ViewGroupFolder : IConfigurationItem
{
	public ICollection<ViewGroup> ViewGroups
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ViewGroup> list = new List<ViewGroup>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "ViewGroup")
				{
					list.Add(new ViewGroup(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public ViewGroupFolder(ServerId serverId, ConfigurationItem viewGroupFolder)
		: base(serverId, viewGroupFolder)
	{
	}

	public ViewGroupFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ViewGroupFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ViewGroupFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/ViewGroupFolder")
	{
	}

	public ServerTask AddViewGroup(string id, string name, string description, string viewGroupDataXml)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddViewGroup");
			configurationApiItemWrapper.SetProperty("Id", id);
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("ViewGroupDataXml", viewGroupDataXml);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddViewGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewGroupFolder", "Unable to call method 'AddViewGroup':" + ex.Message);
		}
		return null;
	}

	public AddViewGroupServerTask AddViewGroup()
	{
		try
		{
			return new AddViewGroupServerTask(base.Wrapper.ExecuteMethod("AddViewGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewGroupFolder", "Unable to call method 'AddViewGroup':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveViewGroup(bool removeMembers, string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveViewGroup");
			configurationApiItemWrapper.SetProperty("RemoveMembers", removeMembers.ToString());
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveViewGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewGroupFolder", "Unable to call method 'RemoveViewGroup':" + ex.Message);
		}
		return null;
	}

	public RemoveViewGroupServerTask RemoveViewGroup()
	{
		try
		{
			return new RemoveViewGroupServerTask(base.Wrapper.ExecuteMethod("RemoveViewGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewGroupFolder", "Unable to call method 'RemoveViewGroup':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddViewGroup(string name, string description)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddViewGroup");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddViewGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewGroupFolder", "Unable to call method 'AddViewGroup':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddViewGroup(string name, string description, string viewGroupDataXml)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddViewGroup");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("ViewGroupDataXml", viewGroupDataXml);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddViewGroup"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewGroupFolder", "Unable to call method 'AddViewGroup':" + ex.Message);
		}
		return null;
	}
}
