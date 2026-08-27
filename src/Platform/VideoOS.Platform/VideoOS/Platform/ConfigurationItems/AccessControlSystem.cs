using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AccessControlSystem : IConfigurationItem
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

	public AccessControlSystem(ServerId serverId, ConfigurationItem accessControlSystem)
		: base(serverId, accessControlSystem)
	{
	}

	public AccessControlSystem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AccessControlSystem(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask GetInternalUnitPath(string externalId)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("GetInternalUnitPath");
			configurationApiItemWrapper.SetProperty("ExternalId", externalId);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("GetInternalUnitPath"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlSystem", "Unable to call method 'GetInternalUnitPath':" + ex.Message);
		}
		return null;
	}

	public GetInternalUnitPathServerTask GetInternalUnitPath()
	{
		try
		{
			return new GetInternalUnitPathServerTask(base.Wrapper.ExecuteMethod("GetInternalUnitPath"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "AccessControlSystem", "Unable to call method 'GetInternalUnitPath':" + ex.Message);
		}
		return null;
	}
}
