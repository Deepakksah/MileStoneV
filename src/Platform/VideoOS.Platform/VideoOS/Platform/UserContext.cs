using System;
using System.Collections.Generic;
using System.Net;
using VideoOS.Platform.Log;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.Platform;

public class UserContext
{
	private const string CameraNamespaceId = "623D03F8-C5D5-46BC-A2F4-4C03562D4F85";

	private List<LoginSettings> _loginSettings = new List<LoginSettings>();

	private bool _privacyMaskLifted;

	public EventHandler PrivacyMaskLiftChanged = delegate
	{
	};

	public Guid InstanceGuid = Guid.NewGuid();

	public CredentialCache CredentialCache { get; set; }

	public Configuration Configuration { get; set; }

	public string ClientType { get; set; }

	public LogClient LogClient { get; set; }

	public bool PrivacyMaskLifted => _privacyMaskLifted;

	public List<LoginSettings> LoginSettings => _loginSettings;

	public bool SetPrivacyMaskLifted(bool liftMask)
	{
		bool flag = false;
		if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
		{
			flag = InternalConfigService.Instance.IsPrivacyMaskLiftingApproved;
		}
		if (flag || SecurityAccess.HasOverallPermission(Configuration.ServerFQID.ServerId, "623D03F8-C5D5-46BC-A2F4-4C03562D4F85", "LIFT_PRIVACY_MASKING"))
		{
			_privacyMaskLifted = liftMask;
			PrivacyMaskLiftChanged(this, new EventArgs());
			return true;
		}
		return false;
	}

	public UserContext()
	{
		CredentialCache = new CredentialCache();
		ClientType = "";
	}

	public void AddLoginSettings(LoginSettings loginSettings)
	{
		lock (_loginSettings)
		{
			LoginSettings loginSettings2 = GetLoginSettings(loginSettings.Uri.Host, loginSettings.Uri.Port);
			if (loginSettings2 != null && loginSettings.Uri.Port == loginSettings2.Uri.Port && !string.IsNullOrEmpty(loginSettings.Uri.Host))
			{
				if (loginSettings2 != loginSettings)
				{
					EnvironmentManager.Instance.Log(error: false, "UserContext.Add", "Duplicate Entry on Hostname:+" + loginSettings.Uri.Host, null);
				}
			}
			else
			{
				_loginSettings.Add(loginSettings);
			}
		}
	}

	public void RemoveLoginSettings(LoginSettings loginSettings)
	{
		lock (_loginSettings)
		{
			if (_loginSettings.Contains(loginSettings))
			{
				_loginSettings.Remove(loginSettings);
			}
		}
	}

	public void Clear()
	{
		if (_loginSettings == null)
		{
			return;
		}
		lock (_loginSettings)
		{
			EnvironmentManager.Instance?.Log(error: false, "UserContext.Clear", "All LoginSettings and IdentityCache being cleared");
			foreach (LoginSettings loginSetting in _loginSettings)
			{
				SecurityAccess.Logout(loginSetting);
				loginSetting.IdentityTokenCache?.Dispose();
				loginSetting.IdentityTokenCache = null;
			}
			_loginSettings = new List<LoginSettings>();
		}
	}

	public LoginSettings GetLoginSettings(Guid serverGuid)
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

	public LoginSettings GetLoginSettings(ServerId serverId)
	{
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

	public LoginSettings GetLoginSettings(string hostName, int port = 0)
	{
		string hostName2 = VideoOS.Platform.Login.Util.GetHostName(hostName);
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
			if (VideoOS.Platform.Login.Util.GetHostName(loginSetting.Uri.Host) == hostName2)
			{
				return loginSetting;
			}
			if (VideoOS.Platform.Login.Util.GetHostNameWithoutDomain(loginSetting.Uri.Host) == hostName2)
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

	public LoginSettings GetLoginSettings(FQID fqid)
	{
		if (fqid != null)
		{
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
			return GetLoginSettings(fqid.GetParent());
		}
		return null;
	}

	public NetworkCredential GetNetworkCredential(ServerId serverId)
	{
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
