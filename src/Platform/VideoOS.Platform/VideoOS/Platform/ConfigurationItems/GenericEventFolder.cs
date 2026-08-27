using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class GenericEventFolder : IConfigurationItem
{
	public ICollection<GenericEvent> GenericEvents
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<GenericEvent> list = new List<GenericEvent>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "GenericEvent")
				{
					list.Add(new GenericEvent(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public GenericEventFolder(ServerId serverId, ConfigurationItem genericEventFolder)
		: base(serverId, genericEventFolder)
	{
	}

	public GenericEventFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public GenericEventFolder(FQID fqid)
		: base(fqid)
	{
	}

	public GenericEventFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/GenericEventFolder")
	{
	}

	public ServerTask AddGenericEvent(string name, string dataSource, string expression, string expressionType, int priority)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddGenericEvent");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("DataSource", dataSource);
			configurationApiItemWrapper.SetProperty("Expression", expression);
			configurationApiItemWrapper.SetProperty("ExpressionType", expressionType);
			configurationApiItemWrapper.SetProperty("Priority", priority.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddGenericEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GenericEventFolder", "Unable to call method 'AddGenericEvent':" + ex.Message);
		}
		return null;
	}

	public AddGenericEventServerTask AddGenericEvent()
	{
		try
		{
			return new AddGenericEventServerTask(base.Wrapper.ExecuteMethod("AddGenericEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GenericEventFolder", "Unable to call method 'AddGenericEvent':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveGenericEvent(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveGenericEvent");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveGenericEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GenericEventFolder", "Unable to call method 'RemoveGenericEvent':" + ex.Message);
		}
		return null;
	}

	public RemoveGenericEventServerTask RemoveGenericEvent()
	{
		try
		{
			return new RemoveGenericEventServerTask(base.Wrapper.ExecuteMethod("RemoveGenericEvent"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GenericEventFolder", "Unable to call method 'RemoveGenericEvent':" + ex.Message);
		}
		return null;
	}
}
