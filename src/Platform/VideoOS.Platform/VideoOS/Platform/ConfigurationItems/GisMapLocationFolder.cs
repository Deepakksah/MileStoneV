using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class GisMapLocationFolder : IConfigurationItem
{
	public ICollection<GisMapLocation> GisMapLocations
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<GisMapLocation> list = new List<GisMapLocation>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "GisMapLocation")
				{
					list.Add(new GisMapLocation(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public GisMapLocationFolder(ServerId serverId, ConfigurationItem gisMapLocationFolder)
		: base(serverId, gisMapLocationFolder)
	{
	}

	public GisMapLocationFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public GisMapLocationFolder(FQID fqid)
		: base(fqid)
	{
	}

	public GisMapLocationFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/GisMapLocationFolder")
	{
	}

	public ServerTask AddGisMapLocation(string name, double scale, string color, double positionX, double positionY)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddGisMapLocation");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("Scale", scale.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("Color", color);
			configurationApiItemWrapper.SetProperty("PositionX", positionX.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("PositionY", positionY.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddGisMapLocation"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GisMapLocationFolder", "Unable to call method 'AddGisMapLocation':" + ex.Message);
		}
		return null;
	}

	public AddGisMapLocationServerTask AddGisMapLocation()
	{
		try
		{
			return new AddGisMapLocationServerTask(base.Wrapper.ExecuteMethod("AddGisMapLocation"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GisMapLocationFolder", "Unable to call method 'AddGisMapLocation':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveGisMapLocation(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveGisMapLocation");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveGisMapLocation"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GisMapLocationFolder", "Unable to call method 'RemoveGisMapLocation':" + ex.Message);
		}
		return null;
	}

	public RemoveGisMapLocationServerTask RemoveGisMapLocation()
	{
		try
		{
			return new RemoveGisMapLocationServerTask(base.Wrapper.ExecuteMethod("RemoveGisMapLocation"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "GisMapLocationFolder", "Unable to call method 'RemoveGisMapLocation':" + ex.Message);
		}
		return null;
	}
}
