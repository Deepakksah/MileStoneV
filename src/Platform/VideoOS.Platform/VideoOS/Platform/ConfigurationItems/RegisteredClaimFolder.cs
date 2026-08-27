using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RegisteredClaimFolder : IConfigurationItem
{
	public ICollection<RegisteredClaim> RegisteredClaims
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<RegisteredClaim> list = new List<RegisteredClaim>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "RegisteredClaim")
				{
					list.Add(new RegisteredClaim(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public RegisteredClaimFolder(ServerId serverId, ConfigurationItem registeredClaimFolder)
		: base(serverId, registeredClaimFolder)
	{
	}

	public RegisteredClaimFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public RegisteredClaimFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddRegisteredClaim(string name, string displayName, bool caseSensitive)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddRegisteredClaim");
			configurationApiItemWrapper.SetProperty("Name", name);
			configurationApiItemWrapper.SetProperty("DisplayName", displayName);
			configurationApiItemWrapper.SetProperty("CaseSensitive", caseSensitive.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddRegisteredClaim"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RegisteredClaimFolder", "Unable to call method 'AddRegisteredClaim':" + ex.Message);
		}
		return null;
	}

	public AddRegisteredClaimServerTask AddRegisteredClaim()
	{
		try
		{
			return new AddRegisteredClaimServerTask(base.Wrapper.ExecuteMethod("AddRegisteredClaim"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RegisteredClaimFolder", "Unable to call method 'AddRegisteredClaim':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveRegisteredClaim(string itemSelection)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveRegisteredClaim");
			configurationApiItemWrapper.SetProperty("ItemSelection", itemSelection);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveRegisteredClaim"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RegisteredClaimFolder", "Unable to call method 'RemoveRegisteredClaim':" + ex.Message);
		}
		return null;
	}

	public RemoveRegisteredClaimServerTask RemoveRegisteredClaim()
	{
		try
		{
			return new RemoveRegisteredClaimServerTask(base.Wrapper.ExecuteMethod("RemoveRegisteredClaim"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RegisteredClaimFolder", "Unable to call method 'RemoveRegisteredClaim':" + ex.Message);
		}
		return null;
	}
}
