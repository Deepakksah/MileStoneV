using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AccessControlUnit : IConfigurationItem
{
	public AccessControlUnitFolder AccessControlUnitFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "AccessControlUnitFolder")
				{
					return new AccessControlUnitFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string RuleGroup => base.Wrapper.GetPropertyValue("RuleGroup");

	public AccessControlUnit(ServerId serverId, ConfigurationItem accessControlUnit)
		: base(serverId, accessControlUnit)
	{
	}

	public AccessControlUnit(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AccessControlUnit(FQID fqid)
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
			EnvironmentManager.Instance.Log(error: false, "AccessControlUnit", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
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
			EnvironmentManager.Instance.Log(error: false, "AccessControlUnit", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}
}
