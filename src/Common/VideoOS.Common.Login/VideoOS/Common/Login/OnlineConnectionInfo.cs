using System;
using System.Net;
using System.Security.Principal;

namespace VideoOS.Common.Login;

public class OnlineConnectionInfo : IOnlineConnectionInfo, IConnectionInfo
{
	private string _domainName;

	private string _userName;

	public Guid ServerTypeId { get; public set; }

	public string ServerTypeName { get; set; }

	public Uri ServerAddress { get; public set; }

	public AuthenticationType AuthenticationType { get; private set; }

	public string LoginProviderId { get; }

	public string DomainName => _domainName;

	public string UserName => _userName;

	public string EncryptedPassword { get; private set; }

	public string Password
	{
		get
		{
			if (string.IsNullOrEmpty(EncryptedPassword))
			{
				return string.Empty;
			}
			try
			{
				return DataProtection.Unprotect(EncryptedPassword);
			}
			catch
			{
				return string.Empty;
			}
		}
		public set
		{
			try
			{
				if (!string.IsNullOrEmpty(value))
				{
					EncryptedPassword = DataProtection.Protect(value);
				}
				else
				{
					EncryptedPassword = string.Empty;
				}
			}
			catch
			{
			}
		}
	}

	public CredentialCache Credentials { get; private set; }

	public OnlineConnectionInfo(Guid serverTypeId, Uri serverAddress, AuthenticationType authenticationType, ICredentials credentials)
	{
		ServerTypeId = serverTypeId;
		ServerAddress = serverAddress;
		SetAuthentication(authenticationType, credentials);
	}

	public OnlineConnectionInfo(UIConnectionInfo uiConnectionInfo)
	{
		ServerTypeId = uiConnectionInfo.ServerTypeId;
		ServerAddress = uiConnectionInfo.ServerAddress;
		SetAuthentication(uiConnectionInfo.AuthenticationType, uiConnectionInfo.UserName, uiConnectionInfo.Password, "");
		LoginProviderId = uiConnectionInfo.LoginProviderId;
	}

	private void SetWindowsAuthentication()
	{
		_userName = WindowsIdentity.GetCurrent().Name;
		EncryptedPassword = string.Empty;
		if (Credentials == null)
		{
			Credentials = new CredentialCache();
		}
		if (Credentials.GetCredential(ServerAddress, "Negotiate") != null)
		{
			Credentials.Remove(ServerAddress, "Negotiate");
		}
		Credentials.Add(ServerAddress, "Negotiate", CredentialCache.DefaultNetworkCredentials);
	}

	private void SetAuthentication(AuthenticationType authenticationType, ICredentials credentials)
	{
		if (credentials is NetworkCredential networkCredential)
		{
			SetAuthentication(authenticationType, networkCredential.UserName, networkCredential.Password, networkCredential.Domain);
		}
	}

	private void SetAuthentication(AuthenticationType authenticationType, string userName, string password, string domain)
	{
		_userName = userName;
		Password = password;
		switch (authenticationType)
		{
		case AuthenticationType.WindowsDefault:
			SetWindowsAuthentication();
			break;
		case AuthenticationType.Windows:
			_domainName = domain;
			if (userName.Contains("\\"))
			{
				SplitDomainAndUserName(userName, out _domainName, out _userName);
				if (_domainName.Equals("."))
				{
					_domainName = Environment.MachineName;
				}
			}
			Credentials = new CredentialCache();
			Credentials.Add(ServerAddress, "Negotiate", new NetworkCredential(_userName, Password, _domainName));
			break;
		case AuthenticationType.Simple:
			Credentials = new CredentialCache();
			Credentials.Add(ServerAddress, "Basic", new NetworkCredential(_userName, Password, "[BASIC]"));
			break;
		default:
			throw new ArgumentOutOfRangeException("authenticationType");
		case AuthenticationType.External:
			break;
		}
		AuthenticationType = authenticationType;
	}

	public static void SplitDomainAndUserName(string combinedUserName, out string domainName, out string userName)
	{
		string[] array = combinedUserName.Split('\\');
		if (array.Length == 1)
		{
			userName = array[0];
			domainName = string.Empty;
			return;
		}
		if (array.Length == 2)
		{
			domainName = array[0];
			userName = array[1];
			return;
		}
		throw new UIConnectionInfoException(Resources.LoginInvalidUsername);
	}
}
