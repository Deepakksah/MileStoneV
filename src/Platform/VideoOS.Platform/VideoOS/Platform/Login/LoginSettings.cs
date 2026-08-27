using System;
using System.Globalization;
using System.Net;
using System.Security.Principal;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.ImageServer;

namespace VideoOS.Platform.Login;

public class LoginSettings
{
	public readonly Guid InstanceGuid = Guid.NewGuid();

	public LoginServerProductInfo ServerProductInfo = new LoginServerProductInfo();

	private string _token = "";

	private DateTime _tokenTimeToLive = DateTime.MinValue;

	private UserIdentification _userIdentification;

	public Uri Uri { get; set; }

	public Guid Guid { get; set; }

	public string ServerType { get; set; }

	public string ServerWebType { get; set; }

	public string Name { get; set; }

	public IMipTokenCache IdentityTokenCache { get; set; }

	[Obsolete("From 2022 R1, this property may be null when the user is using OAuth 2.0 tokens. Try always to use the LoginSettings class and check the IsOAuthIdentity == false, before using the NetworkCredential")]
	public NetworkCredential NetworkCredential { get; set; }

	public CredentialCache CredentialCache { get; set; }

	public Uri UriCorporate
	{
		get
		{
			if (IdentityTokenCache != null)
			{
				return Uri;
			}
			if (IsBasicUser)
			{
				if (!Uri.IsDefaultPort)
				{
					return new UriBuilder("https:", Uri.Host, Uri.Port).Uri;
				}
				return new UriBuilder("https:", Uri.Host).Uri;
			}
			if (!Uri.IsDefaultPort)
			{
				return new UriBuilder("http:", Uri.Host, Uri.Port).Uri;
			}
			return new UriBuilder("http:", Uri.Host).Uri;
		}
	}

	public bool SecureOnly { get; }

	public virtual string Token
	{
		get
		{
			return _token;
		}
		set
		{
			_token = value;
			try
			{
				if (this.TokenChangedEvent != null)
				{
					this.TokenChangedEvent(this, new EventArgs());
				}
			}
			catch (Exception exception)
			{
				EnvironmentManager.Instance.ExceptionHandler("LoginSettings.Tokenset", exception);
			}
		}
	}

	public DateTime TokenTimeToLive
	{
		get
		{
			return _tokenTimeToLive;
		}
		public set
		{
			_tokenTimeToLive = value;
		}
	}

	public string UserIdentity => UserInformation.Sid;

	public string UserName => UserInformation.DisplayName;

	public string[] GroupMemberShip => UserInformation.MemberOf;

	public bool IsBasicUser
	{
		get
		{
			if (CredentialCache == null)
			{
				return false;
			}
			if (CredentialCache.GetCredential(new UriBuilder(Uri.UriSchemeHttps, Uri.Host, Uri.Port).Uri, "Basic") == null)
			{
				return CredentialCache.GetCredential(new UriBuilder(Uri.UriSchemeHttp, Uri.Host, Uri.Port).Uri, "Basic") != null;
			}
			return true;
		}
	}

	public bool IsOAuthConnection => IdentityTokenCache != null;

	public bool IsOAuthIdentity
	{
		get
		{
			if (IsOAuthConnection && NetworkCredential == null)
			{
				return !CredentialCacheAny();
			}
			return false;
		}
	}

	public UserIdentification UserInformation
	{
		get
		{
			if (_userIdentification == null)
			{
				if (ServerType == "XPCO")
				{
					try
					{
						if (IsOAuthIdentity)
						{
							string claimValue = IdentityTokenCache.GetClaimValue("name");
							string claimValue2 = IdentityTokenCache.GetClaimValue("sub");
							string claimValue3 = IdentityTokenCache.GetClaimValue("email");
							_userIdentification = new UserIdentification
							{
								MemberOf = new string[0],
								Sid = claimValue2,
								Name = claimValue,
								DisplayName = claimValue,
								Type = UserIdType.UserIDType.OAuthUser,
								Email = claimValue3
							};
						}
						else if (CredentialCache.GetCredential(Uri, "Basic") != null)
						{
							string userName = NetworkCredential.UserName;
							_userIdentification = SecurityAccess.GetUserIdentificationViaName("[BASIC]\\" + userName, this);
						}
						else if (string.IsNullOrEmpty(NetworkCredential.UserName))
						{
							string name = WindowsIdentity.GetCurrent().Name;
							_userIdentification = SecurityAccess.GetUserIdentificationViaName(name, this);
						}
						else
						{
							if (!string.IsNullOrEmpty(NetworkCredential.Domain))
							{
								_userIdentification = SecurityAccess.GetUserIdentificationViaName(NetworkCredential.Domain + "\\" + NetworkCredential.UserName, this);
							}
							else if (NetworkCredential.UserName.IndexOf('@') != -1)
							{
								string[] array = NetworkCredential.UserName.Split(new char[1] { '@' }, StringSplitOptions.RemoveEmptyEntries);
								if (array.Length == 2)
								{
									_userIdentification = SecurityAccess.GetUserIdentificationViaName(array[1] + "\\" + array[0], this);
								}
							}
							if (_userIdentification == null)
							{
								_userIdentification = SecurityAccess.GetUserIdentificationViaName(NetworkCredential.UserName, this);
							}
						}
					}
					catch (Exception)
					{
						EnvironmentManager.Instance.Log(error: false, "LoginSettings", "Unable to get UserIdentification filled");
					}
				}
				if (_userIdentification == null)
				{
					if (IsOAuthIdentity)
					{
						EnvironmentManager.Instance.Log(error: false, "LoginSettings", "OAuth: Unable to get UserIdentification filled");
					}
					string sid = string.Empty;
					string text = string.Empty;
					UserIdType.UserIDType type = UserIdType.UserIDType.WindowsUser;
					NetworkCredential credential = CredentialCache.GetCredential(Uri, "Basic");
					if (credential != null)
					{
						type = UserIdType.UserIDType.BasicUser;
						text = credential.UserName;
						sid = credential.UserName;
					}
					else
					{
						try
						{
							text = ((!string.IsNullOrEmpty(NetworkCredential.UserName)) ? NetworkCredential.UserName : WindowsIdentity.GetCurrent().Name);
							sid = ((SecurityIdentifier)new NTAccount(NetworkCredential.Domain, text).Translate(typeof(SecurityIdentifier))).ToString();
						}
						catch (Exception)
						{
						}
					}
					_userIdentification = new UserIdentification
					{
						MemberOf = Array.Empty<string>(),
						Sid = sid,
						DisplayName = text,
						Type = type
					};
				}
			}
			return _userIdentification;
		}
	}

	public string FullyQualifiedUserName
	{
		get
		{
			if (IsBasicUser || IsOAuthIdentity)
			{
				return "[BASIC]\\" + UserInformation.AccountName;
			}
			if (string.IsNullOrWhiteSpace(UserInformation.Domain))
			{
				return UserInformation.AccountName;
			}
			return UserInformation.Domain + "\\" + UserInformation.AccountName;
		}
	}

	public event EventHandler TokenChangedEvent;

	public event EventHandler RequestNewTokenEvent;

	public void FireRequestNewToken()
	{
		this.RequestNewTokenEvent?.Invoke(this, new EventArgs());
	}

	[Obsolete("This constructor is deprecated, please use LoginSettings(Uri, CredentialCache, Guid, String, bool) instead.")]
	public LoginSettings(Uri uri, CredentialCache credentialCache, Guid guid, string serverName)
		: this(uri, credentialCache, guid, serverName, secureOnly: false)
	{
	}

	[Obsolete("This constructor is deprecated, please use LoginSettings(Uri, CredentialCache, Guid, String, String, bool) instead.")]
	public LoginSettings(Uri uri, CredentialCache credentialCache, Guid guid, string serverName, string serverType)
		: this(uri, credentialCache, guid, serverName, serverType, secureOnly: false)
	{
	}

	[Obsolete("This constructor is deprecated, please use LoginSettings(Uri, NetworkCredential, Guid, String, bool) instead.")]
	public LoginSettings(Uri uri, NetworkCredential networkCredential, Guid guid, string serverName)
		: this(uri, networkCredential, guid, serverName, secureOnly: false)
	{
	}

	[Obsolete("This constructor is deprecated, please use LoginSettings(Uri, NetworkCredential, Guid, String, String, bool) instead.")]
	public LoginSettings(Uri uri, NetworkCredential networkCredential, Guid guid, string serverName, string serverType)
		: this(uri, networkCredential, guid, serverName, serverType, secureOnly: false)
	{
	}

	public LoginSettings(Uri uri, CredentialCache credentialCache, Guid guid, string serverName, bool secureOnly)
		: this(uri, credentialCache, null, guid, serverName, secureOnly)
	{
	}

	public LoginSettings(Uri uri, CredentialCache credentialCache, IMipTokenCache identityTokenCache, Guid guid, string serverName, bool secureOnly)
		: this(uri, credentialCache, identityTokenCache, guid, serverName, "XP", secureOnly)
	{
	}

	public LoginSettings(Uri uri, CredentialCache credentialCache, Guid guid, string serverName, string serverType, bool secureOnly)
		: this(uri, credentialCache, null, guid, serverName, serverType, secureOnly)
	{
	}

	public LoginSettings(Uri uri, CredentialCache credentialCache, IMipTokenCache identityTokenCache, Guid guid, string serverName, string serverType, bool secureOnly)
		: this(uri, credentialCache, null, identityTokenCache, guid, serverName, serverType, secureOnly)
	{
	}

	public LoginSettings(Uri uri, IMipTokenCache identityTokenCache, Guid guid, string serverName, bool secureOnly)
		: this(uri, null, null, identityTokenCache, guid, serverName, "XPCO", secureOnly)
	{
	}

	private LoginSettings(Uri uri, CredentialCache credentialCache, NetworkCredential networkCredential, IMipTokenCache identityTokenCache, Guid guid, string serverName, string serverType, bool secureOnly)
	{
		if (identityTokenCache != null && secureOnly && uri.Scheme != Uri.UriSchemeHttps)
		{
			throw new ArgumentMIPException("Only HTTPS scheme is allowed when secure only is true");
		}
		if (identityTokenCache == null && credentialCache == null && networkCredential == null)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginSettings", "identityTokenCache is not set and both credentialCache and networkCredential are null");
		}
		Uri = uri;
		Guid = guid;
		if (credentialCache != null)
		{
			CredentialCache = credentialCache;
			if (networkCredential == null)
			{
				NetworkCredential = null;
				try
				{
					NetworkCredential = credentialCache.GetCredential(uri, "Negotiate");
					if (NetworkCredential == null)
					{
						NetworkCredential = credentialCache.GetCredential(uri, "Basic");
					}
				}
				catch
				{
				}
			}
		}
		if (networkCredential != null)
		{
			NetworkCredential = networkCredential;
			if (credentialCache == null)
			{
				CredentialCache = new CredentialCache();
				CredentialCache.Add(uri, "Negotiate", networkCredential);
			}
		}
		IdentityTokenCache = identityTokenCache;
		Name = serverName;
		ServerType = serverType;
		SecureOnly = secureOnly;
		EnvironmentManager.Instance?.Log(error: false, "LoginSettings.ctor", string.Format(CultureInfo.InvariantCulture, "Constructed for instance={0}, IdentityTokenCache {1}", InstanceGuid, (identityTokenCache == null) ? "is null" : "Has value"));
	}

	public LoginSettings(Uri uri, NetworkCredential networkCredential, Guid guid, string serverName, bool secureOnly)
		: this(uri, networkCredential, guid, serverName, "XP", secureOnly)
	{
	}

	public LoginSettings(Uri uri, NetworkCredential networkCredential, Guid guid, string serverName, string serverType, bool secureOnly)
		: this(uri, null, networkCredential, null, guid, serverName, serverType, secureOnly)
	{
	}

	public static LoginSettings GetLoginSettings(ServerId serverId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		return ((serverId.UserContext != null) ? serverId.UserContext.GetLoginSettings(serverId) : LoginSettingsCache.GetLoginSettings(serverId)) ?? throw new ArgumentException("LoginSettings not found for serverId", "serverId");
	}

	private bool CredentialCacheAny()
	{
		if (CredentialCache == null)
		{
			return false;
		}
		return CredentialCache.GetEnumerator().MoveNext();
	}
}
