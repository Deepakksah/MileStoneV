using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ViewFolder : IConfigurationItem
{
	public ICollection<View> Views
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<View> list = new List<View>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "View")
				{
					list.Add(new View(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public ViewFolder(ServerId serverId, ConfigurationItem viewFolder)
		: base(serverId, viewFolder)
	{
	}

	public ViewFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ViewFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ClassCollectionServerTask AddView(string id, string name, string shortcut, string viewLayoutType, string layoutCustomId, string layoutIcon, string layoutViewItems, string clientId)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddView");
			configurationApiItemWrapper.SetProperty("LayoutViewItems", layoutViewItems);
			ConfigurationApiItemWrapper configurationApiItemWrapper2 = configurationApiItemWrapper.ExecuteMethod("AddView");
			configurationApiItemWrapper2.SetProperty("Id", id);
			configurationApiItemWrapper2.SetProperty("Name", name);
			configurationApiItemWrapper2.SetProperty("Shortcut", shortcut);
			configurationApiItemWrapper2.SetProperty("ViewLayoutType", viewLayoutType);
			configurationApiItemWrapper2.SetProperty("LayoutCustomId", layoutCustomId);
			configurationApiItemWrapper2.SetProperty("LayoutIcon", layoutIcon);
			configurationApiItemWrapper2.SetProperty("LayoutViewItems", layoutViewItems);
			configurationApiItemWrapper2.SetProperty("ClientId", clientId);
			return new ClassCollectionServerTask(configurationApiItemWrapper2.ExecuteMethod("AddView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewFolder", "Unable to call method 'AddView':" + ex.Message);
		}
		return null;
	}

	public AddViewServerTask AddView()
	{
		try
		{
			return new AddViewServerTask(base.Wrapper.ExecuteMethod("AddView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewFolder", "Unable to call method 'AddView':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveView(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveView");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewFolder", "Unable to call method 'RemoveView':" + ex.Message);
		}
		return null;
	}

	public RemoveViewServerTask RemoveView()
	{
		try
		{
			return new RemoveViewServerTask(base.Wrapper.ExecuteMethod("RemoveView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewFolder", "Unable to call method 'RemoveView':" + ex.Message);
		}
		return null;
	}

	public AddViewServerTask AddView(string layoutViewItems)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddView");
			configurationApiItemWrapper.SetProperty("LayoutViewItems", layoutViewItems);
			return new AddViewServerTask(configurationApiItemWrapper.ExecuteMethod("AddView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewFolder", "Unable to call method 'AddView':" + ex.Message);
		}
		return null;
	}

	public ClassCollectionServerTask AddView(string name, string shortcut, string viewLayoutType, string layoutCustomId, string layoutIcon, string layoutViewItems)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddView");
			configurationApiItemWrapper.SetProperty("LayoutViewItems", layoutViewItems);
			ConfigurationApiItemWrapper configurationApiItemWrapper2 = configurationApiItemWrapper.ExecuteMethod("AddView");
			configurationApiItemWrapper2.SetProperty("Name", name);
			configurationApiItemWrapper2.SetProperty("Shortcut", shortcut);
			configurationApiItemWrapper2.SetProperty("ViewLayoutType", viewLayoutType);
			configurationApiItemWrapper2.SetProperty("LayoutCustomId", layoutCustomId);
			configurationApiItemWrapper2.SetProperty("LayoutIcon", layoutIcon);
			configurationApiItemWrapper2.SetProperty("LayoutViewItems", layoutViewItems);
			return new ClassCollectionServerTask(configurationApiItemWrapper2.ExecuteMethod("AddView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewFolder", "Unable to call method 'AddView':" + ex.Message);
		}
		return null;
	}

	public ClassCollectionServerTask AddView(string id, string name, string shortcut, string viewLayoutType, string layoutCustomId, string layoutIcon, string layoutViewItems)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddView");
			configurationApiItemWrapper.SetProperty("LayoutViewItems", layoutViewItems);
			ConfigurationApiItemWrapper configurationApiItemWrapper2 = configurationApiItemWrapper.ExecuteMethod("AddView");
			configurationApiItemWrapper2.SetProperty("Id", id);
			configurationApiItemWrapper2.SetProperty("Name", name);
			configurationApiItemWrapper2.SetProperty("Shortcut", shortcut);
			configurationApiItemWrapper2.SetProperty("ViewLayoutType", viewLayoutType);
			configurationApiItemWrapper2.SetProperty("LayoutCustomId", layoutCustomId);
			configurationApiItemWrapper2.SetProperty("LayoutIcon", layoutIcon);
			configurationApiItemWrapper2.SetProperty("LayoutViewItems", layoutViewItems);
			return new ClassCollectionServerTask(configurationApiItemWrapper2.ExecuteMethod("AddView"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ViewFolder", "Unable to call method 'AddView':" + ex.Message);
		}
		return null;
	}
}
