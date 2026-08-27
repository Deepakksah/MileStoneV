using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LayoutFolder : IConfigurationItem
{
	public ICollection<Layout> Layouts
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<Layout> list = new List<Layout>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Layout")
				{
					list.Add(new Layout(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public LayoutFolder(ServerId serverId, ConfigurationItem layoutFolder)
		: base(serverId, layoutFolder)
	{
	}

	public LayoutFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LayoutFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddLayout(string name, string description, string definitionXml)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddLayout");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("DefinitionXml", definitionXml);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LayoutFolder", "Unable to call method 'AddLayout':" + ex.Message);
		}
		return null;
	}

	public AddLayoutServerTask AddLayout()
	{
		try
		{
			return new AddLayoutServerTask(base.Wrapper.ExecuteMethod("AddLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LayoutFolder", "Unable to call method 'AddLayout':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveLayout(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveLayout");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LayoutFolder", "Unable to call method 'RemoveLayout':" + ex.Message);
		}
		return null;
	}

	public RemoveLayoutServerTask RemoveLayout()
	{
		try
		{
			return new RemoveLayoutServerTask(base.Wrapper.ExecuteMethod("RemoveLayout"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LayoutFolder", "Unable to call method 'RemoveLayout':" + ex.Message);
		}
		return null;
	}
}
