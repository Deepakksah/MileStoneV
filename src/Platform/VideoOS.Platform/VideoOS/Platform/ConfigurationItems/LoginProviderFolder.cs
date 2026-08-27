using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LoginProviderFolder : IConfigurationItem
{
	public ICollection<LoginProvider> LoginProviders
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<LoginProvider> list = new List<LoginProvider>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "LoginProvider")
				{
					list.Add(new LoginProvider(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public LoginProviderFolder(ServerId serverId, ConfigurationItem loginProviderFolder)
		: base(serverId, loginProviderFolder)
	{
	}

	public LoginProviderFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LoginProviderFolder(FQID fqid)
		: base(fqid)
	{
	}

	public LoginProviderFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/LoginProviderFolder")
	{
	}

	public ServerTask AddLoginProvider(string id, string name, string clientId, string clientSecret, string callbackPath, string authority, string userNameClaimType, Collection<string> scopes, bool promptForLogin, bool enabled)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddLoginProvider");
			configurationApiItemWrapper.SetProperty("Id", id);
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("ClientId", clientId);
			configurationApiItemWrapper.SetProperty("ClientSecret", clientSecret);
			configurationApiItemWrapper.SetProperty("CallbackPath", callbackPath);
			configurationApiItemWrapper.SetProperty("Authority", authority);
			configurationApiItemWrapper.SetProperty("UserNameClaimType", userNameClaimType);
			configurationApiItemWrapper.SetProperty("Scopes", scopes);
			configurationApiItemWrapper.SetProperty("PromptForLogin", promptForLogin.ToString());
			configurationApiItemWrapper.SetProperty("Enabled", enabled.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddLoginProvider"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginProviderFolder", "Unable to call method 'AddLoginProvider':" + ex.Message);
		}
		return null;
	}

	public AddLoginProviderServerTask AddLoginProvider()
	{
		try
		{
			return new AddLoginProviderServerTask(base.Wrapper.ExecuteMethod("AddLoginProvider"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginProviderFolder", "Unable to call method 'AddLoginProvider':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveLoginProvider(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveLoginProvider");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveLoginProvider"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginProviderFolder", "Unable to call method 'RemoveLoginProvider':" + ex.Message);
		}
		return null;
	}

	public RemoveLoginProviderServerTask RemoveLoginProvider()
	{
		try
		{
			return new RemoveLoginProviderServerTask(base.Wrapper.ExecuteMethod("RemoveLoginProvider"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginProviderFolder", "Unable to call method 'RemoveLoginProvider':" + ex.Message);
		}
		return null;
	}

	[Obsolete("This version is deprecated. Please use version taking scopes as collection instead.")]
	public ServerTask AddLoginProvider(string id, string name, string clientId, string clientSecret, string callbackPath, string authority, string userNameClaimType, string scopes, bool promptForLogin, bool enabled)
	{
		Collection<string> scopes2 = ((scopes != null) ? new Collection<string>(scopes.Split(',')) : null);
		return AddLoginProvider(id, name, clientId, clientSecret, callbackPath, authority, userNameClaimType, scopes2, promptForLogin, enabled);
	}
}
