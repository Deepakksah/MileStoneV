using System;
using System.Collections.Generic;
using System.Net;

namespace VideoOS.Platform.Login;

public sealed class LoginSettingsCache
{
	private static List<LoginSettings> _loginSettings = new List<LoginSettings>();

	public static List<LoginSettings> LoginSettings => _loginSettings;

	private LoginSettingsCache()
	{
	}

	public static void AddLoginSettings(LoginSettings loginSettings)
	{
		LoginSettings loginSettings2 = GetLoginSettings(loginSettings.Uri.Host, loginSettings.Uri.Port);
		if (loginSettings2 != null && loginSettings.Uri.Port == loginSettings2.Uri.Port && !string.IsNullOrEmpty(loginSettings.Uri.Host))
		{
			if (loginSettings2 != loginSettings)
			{
				EnvironmentManager.Instance.Log(error: false, "LoginSettingsCache.Add", "Duplicate Entry on Hostname:+" + loginSettings.Uri.Host, null);
				if (string.IsNullOrEmpty(loginSettings2.Token))
				{
					_loginSettings.Remove(loginSettings2);
					_loginSettings.Add(loginSettings);
				}
			}
		}
		else
		{
			_loginSettings.Add(loginSettings);
		}
	}

	public static void RemoveLoginSettings(LoginSettings loginSettings)
	{
		lock (_loginSettings)
		{
			if (_loginSettings.Contains(loginSettings))
			{
				_loginSettings.Remove(loginSettings);
			}
		}
	}

	public static void Clear()
	{
		if (Configuration.Instance != null)
		{
			Configuration.Instance._hasMIPItemSupportBeenChecked = false;
		}
		_loginSettings = new List<LoginSettings>();
	}

	public static LoginSettings GetLoginSettings(Guid serverGuid)
	{
		foreach (LoginSettings loginSetting in _loginSettings)
		{
			if (loginSetting.Guid == serverGuid)
			{
				return loginSetting;
			}
		}
		return null;
	}

	public static LoginSettings GetLoginSettings(ServerId serverId)
	{
		if (EnvironmentManager.UsingUserContext)
		{
			return serverId.UserContext.GetLoginSettings(serverId);
		}
		if (serverId != null)
		{
			LoginSettings loginSettings = GetLoginSettings(serverId.Id);
			if (loginSettings != null)
			{
				return loginSettings;
			}
			return GetLoginSettings(serverId.ServerHostname, serverId.Serverport);
		}
		return null;
	}

	public static LoginSettings GetLoginSettings(string hostName)
	{
		return GetLoginSettings(hostName, 0);
	}

	public static LoginSettings GetLoginSettings(string hostName, int port)
	{
		string hostName2 = Util.GetHostName(hostName);
		IPAddress address = null;
		IPAddress[] array = null;
		if (IPAddress.TryParse(hostName2, out address))
		{
			array = new IPAddress[1] { address };
		}
		else
		{
			try
			{
				array = Dns.GetHostAddresses(hostName);
			}
			catch (Exception)
			{
			}
		}
		foreach (LoginSettings loginSetting in _loginSettings)
		{
			if (port != 0 && loginSetting.Uri.Port != port)
			{
				continue;
			}
			if (Util.GetHostName(loginSetting.Uri.Host) == hostName2)
			{
				return loginSetting;
			}
			if (Util.GetHostNameWithoutDomain(loginSetting.Uri.Host) == hostName2)
			{
				return loginSetting;
			}
			if (array == null || !IPAddress.TryParse(loginSetting.Uri.Authority, out var address2))
			{
				continue;
			}
			IPAddress[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				if (array2[i].Equals(address2))
				{
					return loginSetting;
				}
			}
		}
		return null;
	}

	public static LoginSettings GetLoginSettings(FQID fqid)
	{
		if (fqid != null)
		{
			if (EnvironmentManager.UsingUserContext)
			{
				return fqid.ServerId.UserContext.GetLoginSettings(fqid);
			}
			LoginSettings loginSettings = GetLoginSettings(fqid.ServerId.Id);
			if (loginSettings != null)
			{
				return loginSettings;
			}
			loginSettings = GetLoginSettings(fqid.ParentId);
			if (loginSettings != null)
			{
				return loginSettings;
			}
			loginSettings = GetLoginSettings(fqid.ServerId.ServerHostname, fqid.ServerId.Serverport);
			if (loginSettings != null)
			{
				return loginSettings;
			}
			if (fqid.EqualGuids(fqid.GetParent()))
			{
				return null;
			}
			return GetLoginSettings(fqid.GetParent());
		}
		return null;
	}

	public static NetworkCredential GetNetworkCredential(ServerId serverId)
	{
		if (EnvironmentManager.UsingUserContext)
		{
			return serverId.UserContext.GetNetworkCredential(serverId);
		}
		LoginSettings loginSettings = GetLoginSettings(serverId.Id);
		if (loginSettings == null)
		{
			loginSettings = GetLoginSettings(serverId.ServerHostname, serverId.Serverport);
		}
		NetworkCredential networkCredential = null;
		try
		{
			networkCredential = loginSettings.CredentialCache.GetCredential(loginSettings.Uri, "Negotiate");
			if (networkCredential == null)
			{
				networkCredential = loginSettings.CredentialCache.GetCredential(loginSettings.Uri, "Basic");
			}
		}
		catch
		{
		}
		return networkCredential;
	}
}
