using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ClaimFolder : IConfigurationItem
{
	public ICollection<ClaimChildItem> ClaimChildItems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<ClaimChildItem> list = new List<ClaimChildItem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "Claim")
				{
					list.Add(new ClaimChildItem(base.ServerId, configurationItem, base.Wrapper));
				}
			}
			return list;
		}
	}

	public ClaimFolder(ServerId serverId, ConfigurationItem claimFolder)
		: base(serverId, claimFolder)
	{
	}

	public ClaimFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ClaimFolder(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddRoleClaim(string claimProvider, string claimName, string claimValue)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddRoleClaim");
			configurationApiItemWrapper.SetProperty("ClaimProvider", claimProvider);
			configurationApiItemWrapper.SetProperty("ClaimName", claimName);
			configurationApiItemWrapper.SetProperty("ClaimValue", claimValue);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddRoleClaim"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClaimFolder", "Unable to call method 'AddRoleClaim':" + ex.Message);
		}
		return null;
	}

	public AddRoleClaimServerTask AddRoleClaim()
	{
		try
		{
			return new AddRoleClaimServerTask(base.Wrapper.ExecuteMethod("AddRoleClaim"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClaimFolder", "Unable to call method 'AddRoleClaim':" + ex.Message);
		}
		return null;
	}

	public ServerTask RemoveRoleClaim(string claimProvider, string claimName, string claimValue)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("RemoveRoleClaim");
			configurationApiItemWrapper.SetProperty("ClaimProvider", claimProvider);
			configurationApiItemWrapper.SetProperty("ClaimName", claimName);
			configurationApiItemWrapper.SetProperty("ClaimValue", claimValue);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("RemoveRoleClaim"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClaimFolder", "Unable to call method 'RemoveRoleClaim':" + ex.Message);
		}
		return null;
	}

	public RemoveRoleClaimServerTask RemoveRoleClaim()
	{
		try
		{
			return new RemoveRoleClaimServerTask(base.Wrapper.ExecuteMethod("RemoveRoleClaim"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ClaimFolder", "Unable to call method 'RemoveRoleClaim':" + ex.Message);
		}
		return null;
	}
}
