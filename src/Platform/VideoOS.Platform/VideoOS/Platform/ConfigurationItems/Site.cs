using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Site : IConfigurationItem
{
	public SiteAddressFolder SiteAddressFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "SiteAddressFolder")
				{
					return new SiteAddressFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public SiteFolder SiteFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "SiteFolder")
				{
					return new SiteFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public string ConnectionState => base.Wrapper.GetPropertyValue("ConnectionState");

	public string Version => base.Wrapper.GetPropertyValue("Version");

	public string ServiceAccount => base.Wrapper.GetPropertyValue("ServiceAccount");

	public int SynchronizationStatus
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("SynchronizationStatus");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public Site(ServerId serverId, ConfigurationItem site)
		: base(serverId, site)
	{
	}

	public Site(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Site(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ChangeSecurityPermissions(string userPath)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangeSecurityPermissions");
			configurationApiItemWrapper.SetProperty("UserPath", userPath);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangeSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Site", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}

	public ChangeSecurityPermissionsServerTask ChangeSecurityPermissions()
	{
		try
		{
			return new ChangeSecurityPermissionsServerTask(base.Wrapper.ExecuteMethod("ChangeSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Site", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}
}
