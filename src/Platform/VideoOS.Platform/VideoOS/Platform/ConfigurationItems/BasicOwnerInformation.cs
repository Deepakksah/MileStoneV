using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class BasicOwnerInformation : IConfigurationItem
{
	private ConfigurationApiProperties _properties;

	public ConfigurationApiProperties Properties => _properties;

	public BasicOwnerInformation(ServerId serverId, ConfigurationItem basicOwnerInformation)
		: base(serverId, basicOwnerInformation)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public BasicOwnerInformation(ServerId serverId, string path)
		: base(serverId, path)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public BasicOwnerInformation(FQID fqid)
		: base(fqid)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public ServerTask AddBasicOwnerInfo(string tagType, string tagValue)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddBasicOwnerInfo");
			configurationApiItemWrapper.SetProperty("TagType", tagType);
			configurationApiItemWrapper.SetProperty("TagValue", tagValue);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddBasicOwnerInfo"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicOwnerInformation", "Unable to call method 'AddBasicOwnerInfo':" + ex.Message);
		}
		return null;
	}

	public AddBasicOwnerInfoServerTask AddBasicOwnerInfo()
	{
		try
		{
			return new AddBasicOwnerInfoServerTask(base.Wrapper.ExecuteMethod("AddBasicOwnerInfo"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicOwnerInformation", "Unable to call method 'AddBasicOwnerInfo':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveBasicOwnerInfo(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveBasicOwnerInfo");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveBasicOwnerInfo"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicOwnerInformation", "Unable to call method 'RemoveBasicOwnerInfo':" + ex.Message);
		}
		return null;
	}

	public RemoveBasicOwnerInfoServerTask RemoveBasicOwnerInfo()
	{
		try
		{
			return new RemoveBasicOwnerInfoServerTask(base.Wrapper.ExecuteMethod("RemoveBasicOwnerInfo"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicOwnerInformation", "Unable to call method 'RemoveBasicOwnerInfo':" + ex.Message);
		}
		return null;
	}
}
