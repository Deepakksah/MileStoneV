using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class BasicUser : IConfigurationItem
{
	public ClaimFolder ClaimFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ClaimFolder")
				{
					return new ClaimFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime CreatedDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("CreatedDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

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

	public string Sid => base.Wrapper.GetPropertyValue("Sid");

	public bool CanChangePassword
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CanChangePassword");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CanChangePassword", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ForcePasswordChange
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ForcePasswordChange");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ForcePasswordChange", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> StatusValues => base.Wrapper.GetEnumValues("Status");

	public string Status
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Status");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Status", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime LockoutEnd
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LockoutEnd"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public bool IsExternal
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("IsExternal");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public BasicUser(ServerId serverId, ConfigurationItem basicUser)
		: base(serverId, basicUser)
	{
	}

	public BasicUser(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public BasicUser(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ChangePasswordBasicUser(SecureString password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangePasswordBasicUser");
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangePasswordBasicUser"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicUser", "Unable to call method 'ChangePasswordBasicUser':" + ex.Message);
		}
		return null;
	}

	public ChangePasswordBasicUserServerTask ChangePasswordBasicUser()
	{
		try
		{
			return new ChangePasswordBasicUserServerTask(base.Wrapper.ExecuteMethod("ChangePasswordBasicUser"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicUser", "Unable to call method 'ChangePasswordBasicUser':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangePasswordBasicUser(string password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangePasswordBasicUser");
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangePasswordBasicUser"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "BasicUser", "Unable to call method 'ChangePasswordBasicUser':" + ex.Message);
		}
		return null;
	}
}
