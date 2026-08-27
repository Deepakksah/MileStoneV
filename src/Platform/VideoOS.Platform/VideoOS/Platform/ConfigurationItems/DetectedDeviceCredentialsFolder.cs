using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class DetectedDeviceCredentialsFolder : IConfigurationItem
{
	public DetectedDeviceCredentialsFolder(ServerId serverId, ConfigurationItem detectedDeviceCredentialsFolder)
		: base(serverId, detectedDeviceCredentialsFolder)
	{
	}

	public DetectedDeviceCredentialsFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public DetectedDeviceCredentialsFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddDetectedDeviceCredential(string userName, SecureString password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddDetectedDeviceCredential");
			configurationApiItemWrapper.SetProperty("UserName", userName);
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddDetectedDeviceCredential"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "DetectedDeviceCredentialsFolder", "Unable to call method 'AddDetectedDeviceCredential':" + ex.Message);
		}
		return null;
	}

	public AddDetectedDeviceCredentialServerTask AddDetectedDeviceCredential()
	{
		try
		{
			return new AddDetectedDeviceCredentialServerTask(base.Wrapper.ExecuteMethod("AddDetectedDeviceCredential"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "DetectedDeviceCredentialsFolder", "Unable to call method 'AddDetectedDeviceCredential':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveDetectedDeviceCredential(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveDetectedDeviceCredential");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveDetectedDeviceCredential"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "DetectedDeviceCredentialsFolder", "Unable to call method 'RemoveDetectedDeviceCredential':" + ex.Message);
		}
		return null;
	}

	public RemoveDetectedDeviceCredentialServerTask RemoveDetectedDeviceCredential()
	{
		try
		{
			return new RemoveDetectedDeviceCredentialServerTask(base.Wrapper.ExecuteMethod("RemoveDetectedDeviceCredential"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "DetectedDeviceCredentialsFolder", "Unable to call method 'RemoveDetectedDeviceCredential':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddDetectedDeviceCredential(string userName, string password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddDetectedDeviceCredential");
			configurationApiItemWrapper.SetProperty("UserName", userName);
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddDetectedDeviceCredential"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "DetectedDeviceCredentialsFolder", "Unable to call method 'AddDetectedDeviceCredential':" + ex.Message);
		}
		return null;
	}
}
