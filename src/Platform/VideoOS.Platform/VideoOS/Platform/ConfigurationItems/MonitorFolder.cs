using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MonitorFolder : IConfigurationItem
{
	public ICollection<Monitor> Monitors
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<Monitor> list = new List<Monitor>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Monitor")
				{
					list.Add(new Monitor(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public MonitorFolder(ServerId serverId, ConfigurationItem monitorFolder)
		: base(serverId, monitorFolder)
	{
	}

	public MonitorFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MonitorFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddMonitor(string name, string description, string aspectRatio, string emptyViewItems, string insertionMethod, double locationX, double locationY, double monitorSize, string noLayout)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddMonitor");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Description", description);
			configurationApiItemWrapper.SetProperty("AspectRatio", aspectRatio);
			configurationApiItemWrapper.SetProperty("EmptyViewItems", emptyViewItems);
			configurationApiItemWrapper.SetProperty("InsertionMethod", insertionMethod);
			configurationApiItemWrapper.SetProperty("LocationX", locationX.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("LocationY", locationY.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("MonitorSize", monitorSize.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("NoLayout", noLayout);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddMonitor"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MonitorFolder", "Unable to call method 'AddMonitor':" + ex.Message);
		}
		return null;
	}

	public AddMonitorServerTask AddMonitor()
	{
		try
		{
			return new AddMonitorServerTask(base.Wrapper.ExecuteMethod("AddMonitor"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MonitorFolder", "Unable to call method 'AddMonitor':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveMonitor(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveMonitor");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveMonitor"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MonitorFolder", "Unable to call method 'RemoveMonitor':" + ex.Message);
		}
		return null;
	}

	public RemoveMonitorServerTask RemoveMonitor()
	{
		try
		{
			return new RemoveMonitorServerTask(base.Wrapper.ExecuteMethod("RemoveMonitor"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "MonitorFolder", "Unable to call method 'RemoveMonitor':" + ex.Message);
		}
		return null;
	}
}
