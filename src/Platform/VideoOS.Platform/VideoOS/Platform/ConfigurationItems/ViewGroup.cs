using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ViewGroup : IConfigurationItem
{
	public ViewFolder ViewFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ViewFolder")
				{
					return new ViewFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public ViewGroupFolder ViewGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ViewGroupFolder")
				{
					return new ViewGroupFolder(base.ServerId, configurationItem);
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

	public string ViewGroupDataXml
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ViewGroupDataXml");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ViewGroupDataXml", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ViewGroup(ServerId serverId, ConfigurationItem viewGroup)
		: base(serverId, viewGroup)
	{
	}

	public ViewGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ViewGroup(FQID fqid)
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
			EnvironmentManager.Instance.Log(error: false, "ViewGroup", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
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
			EnvironmentManager.Instance.Log(error: false, "ViewGroup", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}
}
