using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class GenericEventDataSourceFolder : IConfigurationItem
{
	public ICollection<GenericEventDataSource> GenericEventDataSources
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<GenericEventDataSource> list = new List<GenericEventDataSource>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "GenericEventDataSource")
				{
					list.Add(new GenericEventDataSource(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public GenericEventDataSourceFolder(ServerId serverId, ConfigurationItem genericEventDataSourceFolder)
		: base(serverId, genericEventDataSourceFolder)
	{
	}

	public GenericEventDataSourceFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public GenericEventDataSourceFolder(FQID fqid)
		: base(fqid)
	{
	}

	public GenericEventDataSourceFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/GenericEventDataSourceFolder")
	{
	}

	public ServerTask AddGenericEventDataSource(string name, int dataSourcePort, string dataSourceSeparator, int dataSourceEncoding, bool dataSourceLog, string dataSourceEcho, string dataSourceAllowed, string dataSourceAllowed6, string dataSourceProtocol, string dataSourceAddressFamily)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddGenericEventDataSource");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("DataSourcePort", dataSourcePort.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("DataSourceSeparator", dataSourceSeparator);
			configurationApiItemWrapper.SetProperty("DataSourceEncoding", dataSourceEncoding.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("DataSourceLog", dataSourceLog.ToString());
			configurationApiItemWrapper.SetProperty("DataSourceEcho", dataSourceEcho);
			configurationApiItemWrapper.SetProperty("DataSourceAllowed", dataSourceAllowed);
			configurationApiItemWrapper.SetProperty("DataSourceAllowed6", dataSourceAllowed6);
			configurationApiItemWrapper.SetProperty("DataSourceProtocol", dataSourceProtocol);
			configurationApiItemWrapper.SetProperty("DataSourceAddressFamily", dataSourceAddressFamily);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddGenericEventDataSource"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GenericEventDataSourceFolder", "Unable to call method 'AddGenericEventDataSource':" + ex.Message);
		}
		return null;
	}

	public AddGenericEventDataSourceServerTask AddGenericEventDataSource()
	{
		try
		{
			return new AddGenericEventDataSourceServerTask(base.Wrapper.ExecuteMethod("AddGenericEventDataSource"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GenericEventDataSourceFolder", "Unable to call method 'AddGenericEventDataSource':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveGenericEventDataSource(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveGenericEventDataSource");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveGenericEventDataSource"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GenericEventDataSourceFolder", "Unable to call method 'RemoveGenericEventDataSource':" + ex.Message);
		}
		return null;
	}

	public RemoveGenericEventDataSourceServerTask RemoveGenericEventDataSource()
	{
		try
		{
			return new RemoveGenericEventDataSourceServerTask(base.Wrapper.ExecuteMethod("RemoveGenericEventDataSource"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GenericEventDataSourceFolder", "Unable to call method 'RemoveGenericEventDataSource':" + ex.Message);
		}
		return null;
	}
}
