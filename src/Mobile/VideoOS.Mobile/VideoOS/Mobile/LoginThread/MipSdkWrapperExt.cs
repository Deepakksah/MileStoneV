using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VideoOS.IdentityServer.Common.Utilities.Tokens;
using VideoOS.Mobile.AuditLog;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Platform;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.SDK;

namespace VideoOS.Mobile.LoginThread;

public static class MipSdkWrapperExt
{
	public delegate UserContext CreateUserContext();

	public delegate UserContext LoginMultiUserContext(string username, string password, bool isAdUser, string clientType, Uri serverAddress, out Exception exception);

	public static bool UseSingleServerContext { get; set; }

	public static LoginMultiUserContext LoginMipMultiUserContextDelegate
	{
		get
		{
			if (!UseSingleServerContext)
			{
				return LoginMipMultiUserContextWrapper;
			}
			return LoginMipMultiUserSingleServerContext;
		}
	}

	private static UserContext LoginMipMultiUserContextWrapper(string username, string password, bool isAdUser, string clientType, Uri address, out Exception exception)
	{
		return LoginMipMultiUserContext(username, password, isAdUser, clientType, out exception);
	}

	public static UserContext LoginMipMultiUserContext(string username, string password, bool isAdUser, string clientType, out Exception exception)
	{
		return LoginMipMultiUserContextExt(out exception, () => MultiUserEnvironment.CreateUserContext(username, password, isAdUser), clientType);
	}

	public static UserContext LoginMipMultiUserContext(string clientId, IIdpTokens tokens, string clientType, out Exception exception)
	{
		Log.Trace("LoginMipMultiUserContext", "Login with tokens");
		return LoginMipMultiUserContextExt(out exception, () => MultiUserEnvironment.CreateUserContext(clientId, tokens), clientType);
	}

	public static UserContext LoginMipMultiUserContext(IMipTokenCache mipTokenCache, string clientType, out Exception exception)
	{
		Log.Trace("LoginMipMultiUserContext", "Login with IMipTokenCache");
		return LoginMipMultiUserContextExt(out exception, () => MultiUserEnvironment.CreateUserContext(mipTokenCache), clientType);
	}

	public static UserContext LoginMipMultiUserSingleServerContext(string username, string password, bool isAdUser, string clientType, Uri serverAddress, out Exception exception)
	{
		Log.Trace("LoginMipMultiUserSingleServerContext", "Login using single server context");
		return LoginMipMultiUserContextExt(out exception, () => MultiUserEnvironment.CreateSingleServerUserContext(secureOnly: false, username, password, isAdUser, serverAddress), clientType);
	}

	private static UserContext LoginMipMultiUserContextExt(out Exception exception, CreateUserContext userContextCreator, string clientType)
	{
		UserContext userContext = null;
		exception = null;
		try
		{
			userContext = userContextCreator();
			if (clientType == null)
			{
				MultiUserEnvironment.LoginUserContext(userContext);
			}
			else
			{
				MultiUserEnvironment.LoginUserContext(userContext, clientType);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
			ClearUserContext(userContext);
			userContext = null;
			exception = ex;
		}
		return userContext;
	}

	public static void ClearUserContext(UserContext userContext)
	{
		if (userContext != null)
		{
			try
			{
				MultiUserEnvironment.RemoveUserContext(userContext);
				AuditLogManager.RemoveUserLog(userContext);
			}
			catch (Exception ex)
			{
				Log.Error("MipSdkWrapperExt.ClearUserContext", ex.Message);
			}
			try
			{
				userContext.Clear();
			}
			catch (Exception ex2)
			{
				Log.Error("MipSdkWrapperExt.ClearUserContext", ex2.Message);
			}
		}
	}

	public static void ClearUserContext(ref UserContext userContext)
	{
		ClearUserContext(userContext);
		userContext = null;
	}

	public static void SetEnvironmentOption(string name, string value)
	{
		try
		{
			EnvironmentManager.Instance.EnvironmentOptions[name] = value;
		}
		catch (Exception ex)
		{
			Log.Error("MipSdkWrapper.SetEnvironmentOption", ex.Message);
		}
	}

	public static Item GetItem(this VideoOS.Platform.Configuration config, Guid itemId, List<Guid> kindOptions = null)
	{
		if (kindOptions == null)
		{
			kindOptions = new List<Guid>
			{
				Kind.Camera,
				Kind.Speaker,
				Kind.Microphone,
				Kind.Metadata
			};
		}
		if (kindOptions.Count == 1)
		{
			return config.GetItem(itemId, kindOptions.First());
		}
		foreach (Guid kindOption in kindOptions)
		{
			Item item = config.GetItem(itemId, kindOption);
			if (item != null)
			{
				return item;
			}
		}
		return null;
	}

	public static void SaveUpdatedItemConfiguration(Guid pluginId, Item item, bool updateConfigurationId = false)
	{
		if (updateConfigurationId)
		{
			item.Properties["ConfigurationId"] = Guid.NewGuid().ToString();
		}
		VideoOS.Platform.Configuration.Instance.SaveItemConfiguration(pluginId, item);
	}

	public static void SetEncrypt(this Item item, bool value)
	{
		PropertyInfo property = item.GetType().GetProperty("Encrypt", BindingFlags.Instance | BindingFlags.Public);
		if (null != property && property.PropertyType == typeof(bool) && property.CanWrite)
		{
			property.SetValue(item, value);
		}
	}

	public static bool GetEncrypt(this Item item)
	{
		PropertyInfo property = item.GetType().GetProperty("Encrypt", BindingFlags.Instance | BindingFlags.Public);
		if (null != property && property.PropertyType == typeof(bool) && property.CanRead)
		{
			return (bool)property.GetValue(item);
		}
		return false;
	}
}
