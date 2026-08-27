using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.License;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Proxy;
using VideoOS.Platform.SDK.Server;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK.Config;

public class SiteInfoManager
{
	private Dictionary<Guid, SiteInfo> _siteInfoDictionary = new Dictionary<Guid, SiteInfo>();

	private DateTime _lastRefresh = DateTime.MinValue;

	private object _lockObject = new object();

	private int _cacheTimeout = 1;

	private object _recursiveLock = new object();

	private readonly ConnectionCheck _connectionCheck = new ConnectionCheck();

	public SiteInfoManager()
	{
	}

	public SiteInfoManager(ConnectionCheck connectionCheck)
	{
		_connectionCheck = connectionCheck;
	}

	private void FlushCache()
	{
		lock (_lockObject)
		{
			_siteInfoDictionary = new Dictionary<Guid, SiteInfo>();
		}
	}

	private void CheckFlush()
	{
		if (_lastRefresh + TimeSpan.FromMinutes(_cacheTimeout) < DateTime.UtcNow)
		{
			FlushCache();
			_lastRefresh = DateTime.MinValue;
		}
	}

	public SiteInfo GetSite(Guid id)
	{
		return GetSite(id, null);
	}

	public SiteInfo GetSite(Guid id, UserContext userContext)
	{
		CheckFlush();
		lock (_lockObject)
		{
			if (_siteInfoDictionary.TryGetValue(id, out var value))
			{
				return value;
			}
		}
		LoginSettings[] array = new LoginSettings[LoginSettingsCache.LoginSettings.Count];
		LoginSettingsCache.LoginSettings.CopyTo(array);
		LoginSettings[] array2 = array;
		foreach (LoginSettings loginSettings in array2)
		{
			if (!(loginSettings.Guid == id))
			{
				continue;
			}
			BuildSiteInfo(loginSettings.Uri, loginSettings.ServerType, loginSettings, userContext);
			lock (_lockObject)
			{
				if (_siteInfoDictionary.TryGetValue(id, out var value2))
				{
					return value2;
				}
			}
			return null;
		}
		array2 = array;
		foreach (LoginSettings loginSettings2 in array2)
		{
			BuildSiteInfo(loginSettings2.Uri, loginSettings2.ServerType, loginSettings2, userContext);
		}
		lock (_lockObject)
		{
			if (_siteInfoDictionary.TryGetValue(id, out var value3))
			{
				return value3;
			}
		}
		return null;
	}

	public Guid BuildSiteInfo(Uri serverAddress, string serverType, LoginSettings loginSettings, UserContext userContext)
	{
		if (loginSettings == null)
		{
			EnvironmentManager.Instance.Log(error: true, "BuildSiteInfo", "Parameter loginSettings not set");
			return Guid.Empty;
		}
		if (EnvironmentManager.UsingUserContext && userContext == null)
		{
			EnvironmentManager.Instance.Log(error: true, "BuildSiteInfo", "Parameter userContext not set");
			return Guid.Empty;
		}
		try
		{
			if (!loginSettings.IsOAuthConnection)
			{
				return BuildSiteInfoLegacy(serverAddress, serverType, loginSettings, userContext);
			}
			return BuildSiteInfo(loginSettings, userContext);
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "BuildSiteInfo", ex.Message);
			return Guid.Empty;
		}
	}

	private Guid BuildSiteInfo(LoginSettings loginSettings, UserContext userContext)
	{
		ServerId serverId = new ServerId(loginSettings.ServerType, loginSettings.Uri.Scheme, loginSettings.Uri.Host, loginSettings.Uri.Port, loginSettings.Guid)
		{
			UserContext = userContext
		};
		SiteInfo siteInfo = ((!EnvironmentManager.UsingUserContext) ? GetSiteInfoUsingLoginSettings(loginSettings, serverId, "/") : GetSiteInfoUsingUserContext(userContext, loginSettings, serverId, "/"));
		return siteInfo.Id;
	}

	private SiteInfo GetSiteInfoUsingUserContext(UserContext userContext, LoginSettings loginSettings, ServerId serverId, string path)
	{
		bool flag = false;
		if (userContext.GetLoginSettings(serverId) == null)
		{
			userContext.AddLoginSettings(loginSettings);
			flag = true;
		}
		Site site = new Site(serverId, path);
		SiteInfo siteInfoFromSiteItem = GetSiteInfoFromSiteItem(site, null, loginSettings.Uri);
		if (flag)
		{
			userContext.RemoveLoginSettings(loginSettings);
		}
		return siteInfoFromSiteItem;
	}

	private SiteInfo GetSiteInfoUsingLoginSettings(LoginSettings loginSettings, ServerId serverId, string path)
	{
		bool flag = false;
		if (LoginSettingsCache.GetLoginSettings(serverId) == null)
		{
			LoginSettingsCache.AddLoginSettings(loginSettings);
			flag = true;
		}
		Site site = new Site(serverId, path);
		SiteInfo siteInfoFromSiteItem = GetSiteInfoFromSiteItem(site, null, loginSettings.Uri);
		if (flag)
		{
			LoginSettingsCache.RemoveLoginSettings(loginSettings);
		}
		return siteInfoFromSiteItem;
	}

	private SiteInfo GetSiteInfoFromSiteItem(Site site, SiteInfo parentSiteInfo, Uri serverAddress)
	{
		SiteInfo siteInfo = new SiteInfo
		{
			Name = ((!string.IsNullOrEmpty(site.Name)) ? site.Name : (site.DisplayName ?? "")),
			Id = new Guid(site.Id),
			ServerType = (site.ServerId.ServerType ?? "XP"),
			Description = (site.Description ?? ""),
			LastModified = site.LastModified,
			ConnectionState = (site.ConnectionState ?? ""),
			ServerVersion = (site.Version ?? ""),
			ServiceAccount = (site.ServiceAccount ?? ""),
			SyncStatus = site.SynchronizationStatus,
			Parent = (parentSiteInfo?.Id ?? Guid.Empty),
			ParentEncrypted = (parentSiteInfo != null && parentSiteInfo.ServerUri.Scheme == Uri.UriSchemeHttps)
		};
		int productCode = 600;
		ISystemInfo systemInfo = ((SDKConfiguration)Configuration.Instance).GetSystemInfo(siteInfo.Id);
		if (systemInfo != null)
		{
			productCode = new LicenseManager(systemInfo.ServerId.UserContext).ProductCode;
		}
		siteInfo.ServerSubType = productCode.ToString();
		siteInfo.ProductCode = productCode;
		if (serverAddress != null)
		{
			siteInfo.AddressCollection.Add(new Address
			{
				Type = (siteInfo.ParentEncrypted ? "Internal" : "Server"),
				Url = serverAddress.ToString()
			});
		}
		if (site.SiteAddressFolder?.SiteAddressChildItems != null)
		{
			foreach (SiteAddressChildItem siteAddressChildItem in site.SiteAddressFolder.SiteAddressChildItems)
			{
				siteInfo.AddressCollection.Add(new Address
				{
					Type = siteAddressChildItem.AddressType,
					Url = siteAddressChildItem.Url
				});
			}
		}
		lock (_lockObject)
		{
			if (_siteInfoDictionary.ContainsKey(siteInfo.Id))
			{
				EnvironmentManager.Instance.Log(error: true, "SiteInfoManager:GetSiteInfoFromSiteItem", "Site already added - new add ignored");
			}
			else
			{
				_siteInfoDictionary.Add(siteInfo.Id, siteInfo);
			}
		}
		_lastRefresh = DateTime.UtcNow;
		if (site.SiteFolder?.Sites != null)
		{
			foreach (Site site2 in site.SiteFolder.Sites)
			{
				SiteInfo siteInfoFromSiteItem = GetSiteInfoFromSiteItem(site2, siteInfo, null);
				siteInfo.Children.Add(siteInfoFromSiteItem.Id);
			}
		}
		return siteInfo;
	}

	private Guid BuildSiteInfoLegacy(Uri serverAddress, string serverType, LoginSettings loginSettings, UserContext userContext)
	{
		try
		{
			CredentialCache credentialCache = loginSettings.CredentialCache;
			if (serverAddress.Scheme == Uri.UriSchemeHttps)
			{
				Guid guid = UnpackSiteInfoFromUri(serverAddress);
				if (guid != Guid.Empty)
				{
					return guid;
				}
			}
			if (loginSettings.IsBasicUser && serverType == "XPCO")
			{
				return BuildSiteInfoViaServerClient(loginSettings);
			}
			Guid guid2 = Guid.NewGuid();
			ServerId serverId = new ServerId("XPCO", serverAddress.Host, serverAddress.Port, guid2)
			{
				UserContext = userContext
			};
			LoginSettings loginSettings2 = new LoginSettings(serverAddress, credentialCache, guid2, "tempName", loginSettings.SecureOnly);
			LoginSettingsCache.AddLoginSettings(loginSettings2);
			ManagementServer managementServer = new ManagementServer(serverId);
			SiteInfo siteInfo = BuildSiteInfoViaManagementServer(managementServer);
			if (managementServer.SiteFolder != null)
			{
				foreach (Site site in managementServer.SiteFolder.Sites)
				{
					SiteInfo siteInfo2 = BuildSiteInfoViaSite(site, siteInfo.Id);
					siteInfo.Children.Add(siteInfo2.Id);
				}
				LoginSettingsCache.RemoveLoginSettings(loginSettings2);
				return siteInfo.Id;
			}
			LoginSettingsCache.RemoveLoginSettings(loginSettings2);
			XmlDocument siteInfoDocument = GetSiteInfoDocument(serverAddress, isBasic: false, credentialCache);
			if (siteInfoDocument != null)
			{
				XmlNode xmlNode = siteInfoDocument.SelectSingleNode("/methodresponse/siteinfo");
				if (xmlNode != null)
				{
					return UnpackSiteInfo(serverAddress, xmlNode, Guid.Empty, credentialCache);
				}
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "BuildSiteInfo", ex.Message);
		}
		return Guid.Empty;
	}

	private Guid UnpackSiteInfo(Uri serverAddress, XmlNode xmlNode, Guid parentId, CredentialCache credentialCache)
	{
		XmlNode xmlNode2 = null;
		if (xmlNode.Name != "site")
		{
			xmlNode2 = ((xmlNode.Name == "engine") ? xmlNode : xmlNode.SelectSingleNode("site"));
		}
		if (xmlNode2 == null)
		{
			xmlNode2 = xmlNode;
		}
		SiteInfo siteInfo = new SiteInfo();
		siteInfo.Name = GetString(xmlNode2, "name");
		siteInfo.Id = GetGuid(xmlNode2, "id");
		siteInfo.ServerSubType = GetString(xmlNode2, "server-type");
		siteInfo.ServerType = "XPCO";
		siteInfo.Description = GetString(xmlNode2, "description");
		siteInfo.LastModified = GetDateTime(xmlNode2, "lastmodified");
		siteInfo.ConnectionState = GetString(xmlNode2, "connectionstate");
		siteInfo.ServerVersion = GetString(xmlNode2, "serverversion");
		siteInfo.ServiceAccount = GetString(xmlNode2, "serviceaccount");
		siteInfo.SyncStatus = GetInt(xmlNode2, "synchronizationstatus");
		int result = 600;
		ISystemInfo systemInfo = ((SDKConfiguration)Configuration.Instance).GetSystemInfo(siteInfo.Id);
		if (systemInfo != null)
		{
			if (systemInfo.Properties.TryGetValue("ServerType", out var value))
			{
				int.TryParse(value, out result);
			}
			else
			{
				result = new LicenseManager(systemInfo.ServerId.UserContext).ProductCode;
			}
		}
		siteInfo.ServerSubType = result.ToString() ?? "";
		siteInfo.ProductCode = result;
		if (serverAddress != null)
		{
			siteInfo.AddressCollection.Add(new Address
			{
				Type = "Server",
				Url = serverAddress.ToString()
			});
		}
		XmlNode xmlNode3 = xmlNode2.SelectSingleNode("addresses");
		if (xmlNode3 != null)
		{
			FillAddresses(siteInfo, xmlNode3);
		}
		else
		{
			FillAddresses(siteInfo, xmlNode2);
		}
		XmlNode xmlNode4 = xmlNode.SelectSingleNode("parent/site");
		if (xmlNode4 != null)
		{
			parentId = GetGuid(xmlNode4, "id");
		}
		siteInfo.Parent = parentId;
		lock (_lockObject)
		{
			if (_siteInfoDictionary.ContainsKey(siteInfo.Id))
			{
				EnvironmentManager.Instance.Log(error: true, "SiteInfoManager:SiteInfo", "Site already added - new add ignored");
			}
			else
			{
				_siteInfoDictionary.Add(siteInfo.Id, siteInfo);
			}
		}
		_lastRefresh = DateTime.UtcNow;
		XmlNode xmlNode5 = xmlNode.SelectSingleNode("childs");
		if (xmlNode5 != null)
		{
			foreach (XmlNode item3 in xmlNode5.SelectNodes("//childs/site"))
			{
				Guid item = UnpackSiteInfo(null, item3, siteInfo.Id, credentialCache);
				siteInfo.Children.Add(item);
			}
		}
		else
		{
			foreach (XmlNode item4 in xmlNode.SelectNodes("site"))
			{
				Guid item2 = UnpackSiteInfo(null, item4, siteInfo.Id, credentialCache);
				siteInfo.Children.Add(item2);
			}
		}
		return siteInfo.Id;
	}

	private Guid UnpackSiteInfoFromUri(Uri uri)
	{
		try
		{
			ISystemInfo systemInfo = ((ISDKConfiguration)Configuration.Instance).GetSystemInfo(uri);
			if (systemInfo != null)
			{
				LicenseManager licenseManager = new LicenseManager(systemInfo.ServerId.UserContext);
				SiteInfo siteInfo = new SiteInfo();
				siteInfo.Name = systemInfo.Name;
				siteInfo.Id = systemInfo.Id;
				siteInfo.ServerSubType = licenseManager.ProductCode.ToString() ?? "";
				siteInfo.ServerType = "XPCO";
				siteInfo.Description = "";
				siteInfo.LastModified = new DateTime(long.Parse(systemInfo.Properties["LastModified"]));
				siteInfo.ConnectionState = "";
				siteInfo.ServerVersion = systemInfo.Properties["ProductVersion"];
				siteInfo.ServiceAccount = "";
				siteInfo.SyncStatus = 0;
				int result = 600;
				int.TryParse(siteInfo.ServerSubType, out result);
				siteInfo.ProductCode = result;
				siteInfo.AddressCollection.Add(new Address
				{
					Type = "Server",
					Url = systemInfo.Url
				});
				lock (_lockObject)
				{
					if (_siteInfoDictionary.ContainsKey(siteInfo.Id))
					{
						EnvironmentManager.Instance.Log(error: true, "SiteInfoManager:SiteInfo", "Site already added - new add ignored");
					}
					else
					{
						_siteInfoDictionary.Add(siteInfo.Id, siteInfo);
					}
				}
				_lastRefresh = DateTime.UtcNow;
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "GetSiteInfo", "Unable to construct siteinfo:" + ex.Message);
		}
		return Guid.Empty;
	}

	private static void FillAddresses(SiteInfo siteInfo, XmlNode node)
	{
		XmlNodeList xmlNodeList = node.SelectNodes("address");
		if (xmlNodeList == null)
		{
			return;
		}
		foreach (XmlNode item in xmlNodeList)
		{
			XmlNode xmlNode2 = item.SelectSingleNode("uri");
			if (xmlNode2 != null)
			{
				if (siteInfo.AddressCollection.Any((Address a) => a.Type == "Server"))
				{
					siteInfo.AddressCollection.Add(new Address
					{
						Type = "Server1",
						Url = xmlNode2.InnerText
					});
				}
				else
				{
					siteInfo.AddressCollection.Add(new Address
					{
						Type = "Server",
						Url = xmlNode2.InnerText
					});
				}
			}
			else if (item.Attributes != null)
			{
				XmlAttribute xmlAttribute = item.Attributes["url"];
				XmlAttribute xmlAttribute2 = item.Attributes["type"];
				if (xmlAttribute != null && xmlAttribute2 != null)
				{
					siteInfo.AddressCollection.Add(new Address
					{
						Type = xmlAttribute2.InnerText,
						Url = xmlAttribute.InnerText
					});
				}
			}
		}
	}

	public List<SiteInfo> FindSiteUriForUser(Uri topSite, bool basicUser, ICredentials credentials, UserContext userContext, bool secureOnly, out bool unauthorized)
	{
		if (Task.Run(() => _connectionCheck.IsOAuthServerOnline(topSite.ToString())).GetAwaiter().GetResult())
		{
			return FindSiteUriForUserOauth(topSite, basicUser, credentials, userContext, secureOnly, out unauthorized);
		}
		return FindSiteUriForUserLegacy(topSite, basicUser, credentials, out unauthorized);
	}

	public List<SiteInfo> FindSiteUriWithOAuthToken(LoginSettings loginSettings, UserContext userContext, bool secureOnly, out bool unauthorized)
	{
		unauthorized = false;
		if (loginSettings == null)
		{
			EnvironmentManager.Instance.Log(error: true, "BuildSiteInfo", "Parameter loginSettings not set");
			return new List<SiteInfo>();
		}
		if (EnvironmentManager.UsingUserContext && userContext == null)
		{
			EnvironmentManager.Instance.Log(error: true, "BuildSiteInfo", "Parameter userContext not set");
			return new List<SiteInfo>();
		}
		if (!loginSettings.IsOAuthConnection)
		{
			EnvironmentManager.Instance.Log(error: true, "BuildSiteInfo", "FindSiteUriWithOAuthToken can only be used for OAuth connection");
			return new List<SiteInfo>();
		}
		if (loginSettings.Guid == Guid.Empty)
		{
			try
			{
				loginSettings.Guid = LoginSettingsUtil.GetServerId(loginSettings);
			}
			catch (InvalidOperationException)
			{
				unauthorized = true;
				return new List<SiteInfo>();
			}
		}
		Guid guid = BuildSiteInfo(loginSettings.Uri, loginSettings.ServerType, loginSettings, userContext);
		List<SiteInfo> list = new List<SiteInfo>();
		PopulateSiteInfos(guid, list, new List<Guid>());
		if (guid == Guid.Empty)
		{
			unauthorized = true;
			return new List<SiteInfo>();
		}
		return list;
	}

	private List<SiteInfo> FindSiteUriForUserOauth(Uri topSite, bool basicUser, ICredentials credentials, UserContext userContext, bool secureOnly, out bool unauthorized)
	{
		unauthorized = false;
		List<SiteInfo> list = new List<SiteInfo>();
		string text = (basicUser ? "Basic" : "Negotiate");
		NetworkCredential credential = credentials.GetCredential(topSite, text);
		string userName = credential.Domain + "\\" + credential.UserName;
		if (basicUser || string.IsNullOrEmpty(credential.Domain))
		{
			userName = credential.UserName;
		}
		Guid guid = Guid.Empty;
		CredentialCache credentialCache = VideoOS.Platform.Login.Util.BuildCredentialCache(topSite, userName, credential.SecurePassword, text);
		bool flag = false;
		LoginSettings loginSettings = userContext.GetLoginSettings(topSite.Host, topSite.Port);
		if (loginSettings == null)
		{
			loginSettings = LoginSettingsUtil.LoginOAuth(topSite, credentialCache, secureOnly);
			if (loginSettings != null)
			{
				flag = true;
			}
		}
		if (loginSettings != null)
		{
			guid = BuildSiteInfo(loginSettings.Uri, loginSettings.ServerType, loginSettings, userContext);
		}
		if (flag)
		{
			EnvironmentManager.Instance.Log(error: false, "SiteInfoManager", $"Setting IdentityToken = null for instance={loginSettings.InstanceGuid}");
			loginSettings.IdentityTokenCache?.Dispose();
			loginSettings.IdentityTokenCache = null;
		}
		if (guid == Guid.Empty)
		{
			unauthorized = true;
			return new List<SiteInfo>();
		}
		PopulateSiteInfos(guid, list, new List<Guid>());
		return list;
	}

	private void PopulateSiteInfos(Guid siteId, List<SiteInfo> siteInfos, List<Guid> addedIds)
	{
		if (_siteInfoDictionary.TryGetValue(siteId, out var value) && !addedIds.Contains(siteId))
		{
			lock (_recursiveLock)
			{
				addedIds.Add(siteId);
				siteInfos.Add(value);
			}
		}
		if (!_siteInfoDictionary.TryGetValue(siteId, out var value2) || value2.Children.Count == 0)
		{
			return;
		}
		foreach (Guid child in _siteInfoDictionary[siteId].Children)
		{
			PopulateSiteInfos(child, siteInfos, addedIds);
		}
	}

	private static List<SiteInfo> FindSiteUriForUserLegacy(Uri topSite, bool basicUser, ICredentials credentials, out bool unauthorized)
	{
		List<SiteInfo> result = new List<SiteInfo>();
		XmlDocument siteInfoDocument = GetSiteInfoDocument(topSite, basicUser, credentials, out unauthorized);
		if (unauthorized)
		{
			return result;
		}
		if (siteInfoDocument != null)
		{
			XmlNode xmlNode = siteInfoDocument.SelectSingleNode("/methodresponse/siteinfo");
			if (xmlNode != null)
			{
				UnpackSiteUri(xmlNode, result);
			}
		}
		return result;
	}

	private static void UnpackSiteUri(XmlNode xmlNode, List<SiteInfo> result)
	{
		XmlNode xmlNode2 = null;
		if (xmlNode.Name != "site")
		{
			xmlNode2 = ((xmlNode.Name == "engine") ? xmlNode : xmlNode.SelectSingleNode("site"));
		}
		if (xmlNode2 == null)
		{
			xmlNode2 = xmlNode;
		}
		XmlNode xmlNode3 = xmlNode2.SelectSingleNode("addresses");
		List<string> list = ((xmlNode3 == null) ? FindAddresses(xmlNode2) : FindAddresses(xmlNode3));
		if (list != null && list.Count > 0)
		{
			SiteInfo siteInfo = new SiteInfo();
			siteInfo.AddressCollection = new Collection<Address>();
			foreach (string item in list)
			{
				siteInfo.AddressCollection.Add(new Address(item));
			}
			result.Add(siteInfo);
		}
		XmlNode xmlNode4 = xmlNode.SelectSingleNode("childs");
		if (xmlNode4 != null)
		{
			foreach (XmlNode item2 in xmlNode4.SelectNodes("//childs/site"))
			{
				UnpackSiteUri(item2, result);
			}
			return;
		}
		foreach (XmlNode item3 in xmlNode.SelectNodes("site"))
		{
			UnpackSiteUri(item3, result);
		}
	}

	private static List<string> FindAddresses(XmlNode node)
	{
		List<string> list = new List<string>();
		XmlNodeList xmlNodeList = node.SelectNodes("address");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				XmlNode xmlNode2 = item.SelectSingleNode("uri");
				if (xmlNode2 != null)
				{
					list.Add(xmlNode2.InnerText);
				}
				if (item.Attributes != null)
				{
					XmlAttribute xmlAttribute = item.Attributes["url"];
					XmlAttribute xmlAttribute2 = item.Attributes["type"];
					if (xmlAttribute != null && xmlAttribute2 != null)
					{
						list.Add(xmlAttribute.InnerText);
					}
				}
			}
		}
		return list;
	}

	private static string GetString(XmlNode node, string nodename)
	{
		XmlNode xmlNode = node.SelectSingleNode(nodename);
		if (xmlNode != null)
		{
			return xmlNode.InnerText;
		}
		return "";
	}

	private static int GetInt(XmlNode node, string nodename)
	{
		XmlNode xmlNode = node.SelectSingleNode(nodename);
		if (xmlNode != null && int.TryParse(xmlNode.InnerText, out var result))
		{
			return result;
		}
		return 0;
	}

	private static DateTime GetDateTime(XmlNode node, string nodename)
	{
		XmlNode xmlNode = node.SelectSingleNode(nodename);
		if (xmlNode != null && DateTime.TryParse(xmlNode.InnerText, out var result))
		{
			return result;
		}
		return DateTime.MinValue;
	}

	private static Guid GetGuid(XmlNode node, string nodename)
	{
		XmlNode xmlNode = node.SelectSingleNode(nodename);
		if (xmlNode != null)
		{
			try
			{
				return new Guid(xmlNode.InnerText);
			}
			catch
			{
			}
		}
		return Guid.Empty;
	}

	public static XmlDocument GetSiteInfoDocument(Uri serverAddress, bool isBasic, ICredentials credentials)
	{
		bool unauthorized;
		return GetSiteInfoDocument(serverAddress, isBasic, credentials, out unauthorized);
	}

	public static XmlDocument GetSiteInfoDocument(Uri serverAddress, bool isBasic, ICredentials credentials, out bool unauthorized)
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		unauthorized = true;
		Uri uri = ((!isBasic) ? new UriBuilder("http", serverAddress.Host, serverAddress.Port, "/RCServer/SiteInfo.xml").Uri : new UriBuilder("https", serverAddress.Host, 443, "/RCServer/SiteInfo.xml").Uri);
		string text = null;
		if (isBasic)
		{
			NetworkCredential credential = credentials.GetCredential(uri, "Basic");
			if (credential != null)
			{
				text = credential.UserName;
			}
		}
		else
		{
			NetworkCredential credential2 = credentials.GetCredential(uri, "Negotiate");
			if (credential2 != null)
			{
				text = credential2.UserName;
			}
		}
		SiteInfoCacheElement siteInfoCacheElement = SiteInfoCache.Lookup(serverAddress, text, isBasic);
		if (siteInfoCacheElement != null)
		{
			ServerTrace.AfterCall("SiteInfoManager", "GetSiteInfoDocument (Cache) Response ", uri.ToString());
			unauthorized = siteInfoCacheElement.Unauthorized;
			return siteInfoCacheElement.XmlDocument;
		}
		XmlDocument xmlDocument = null;
		new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
		HttpClientHandler val = new HttpClientHandler();
		try
		{
			val.Credentials = credentials;
			val.PreAuthenticate = true;
			HttpClient val2 = new HttpClient((HttpMessageHandler)(object)val);
			try
			{
				val2.BaseAddress = uri;
				val2.Timeout = TimeSpan.FromSeconds(15.0);
				if (isBasic)
				{
					NetworkCredential credential3 = credentials.GetCredential(serverAddress, "Basic");
					if (credential3 != null)
					{
						string text2 = Convert.ToBase64String(Encoding.ASCII.GetBytes(credential3.UserName + ":" + credential3.Password));
						((HttpHeaders)val2.DefaultRequestHeaders).Add("Authorization", "Basic " + text2);
						val.ServerCertificateCustomValidationCallback = (HttpRequestMessage sender, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors SslPolicyErrors) => true;
					}
				}
				bool flag = false;
				try
				{
					ServerTrace.BeforeCall("SiteInfoManager", "GetSiteInfoDocument", uri.ToString());
					xmlDocument = GetXmlDocument(val2);
					if (xmlDocument != null)
					{
						flag = xmlDocument.SelectSingleNode("/methodresponse/siteinfo") != null;
					}
					unauthorized = !flag;
				}
				catch (WebException ex)
				{
					if (ex.Status == WebExceptionStatus.ProtocolError)
					{
						if (ex.Response is HttpWebResponse httpWebResponse)
						{
							if (httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
							{
								unauthorized = true;
							}
							else
							{
								unauthorized = false;
							}
						}
						else if (ex.Message.Contains("401"))
						{
							unauthorized = true;
						}
						else
						{
							unauthorized = false;
						}
					}
				}
				catch (Exception)
				{
					flag = false;
				}
				finally
				{
					ServerTrace.AfterCall("SiteInfoManager", "GetSiteInfoDocument", uri.ToString() + " docloaded=" + flag);
				}
				if (text != null)
				{
					SiteInfoCache.Store(serverAddress, text, isBasic, unauthorized, xmlDocument);
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return xmlDocument;
	}

	private static XmlDocument GetXmlDocument(HttpClient httpClient)
	{
		try
		{
			byte[] result = Task.Run(async () => await httpClient.GetByteArrayAsync(httpClient.BaseAddress)).Result;
			string text = Encoding.ASCII.GetString(result);
			string name = "";
			int num = text.IndexOf("encoding") + 10;
			if (num >= 0)
			{
				int num2 = text.IndexOf("\"", num);
				if (num2 > num)
				{
					name = text.Substring(num, num2 - num);
				}
			}
			Encoding encoding;
			try
			{
				encoding = Encoding.GetEncoding(name);
			}
			catch (Exception)
			{
				encoding = Encoding.UTF8;
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				string xml = ((result[0] != 239) ? encoding.GetString(result) : encoding.GetString(result, 3, result.Length - 3));
				xmlDocument.LoadXml(xml);
				return xmlDocument;
			}
			catch (Exception ex2)
			{
				EnvironmentManager.Instance.Log(error: false, "GetSiteInfoDocument", ex2.Message, null);
				throw;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private Guid BuildSiteInfoViaServerClient(LoginSettings loginSettings)
	{
		using ServerClient serverClient = new ServerClient(loginSettings, GetBinding(isBasicUser: true, loginSettings));
		serverClient.Initialize();
		serverClient.CheckAuthentication();
		ServerInformation serverInformation = serverClient.GetServerInformation();
		SiteInfo siteInfo = new SiteInfo();
		siteInfo.Name = serverInformation.Name;
		siteInfo.Id = new Guid(serverInformation.InstanceId);
		siteInfo.ServerType = "XPCO";
		siteInfo.Description = serverInformation.Description;
		siteInfo.LastModified = serverInformation.LastModified;
		siteInfo.ConnectionState = "";
		siteInfo.ServerVersion = serverInformation.Version.ToString();
		siteInfo.ServiceAccount = serverInformation.ServiceAccount;
		siteInfo.SyncStatus = 0;
		int productCode = 600;
		ISystemInfo systemInfo = ((SDKConfiguration)Configuration.Instance).GetSystemInfo(siteInfo.Id);
		if (systemInfo != null)
		{
			productCode = new LicenseManager(systemInfo.ServerId.UserContext).ProductCode;
		}
		siteInfo.ServerSubType = productCode.ToString() ?? "";
		siteInfo.ProductCode = productCode;
		siteInfo.AddressCollection.Add(new Address
		{
			Type = "Server",
			Url = loginSettings.Uri.ToString()
		});
		siteInfo.Parent = Guid.Empty;
		lock (_lockObject)
		{
			if (_siteInfoDictionary.ContainsKey(siteInfo.Id))
			{
				EnvironmentManager.Instance.Log(error: true, "SiteInfoManager:SiteInfo", "Site already added - new add ignored");
			}
			else
			{
				_siteInfoDictionary.Add(siteInfo.Id, siteInfo);
			}
		}
		_lastRefresh = DateTime.UtcNow;
		return siteInfo.Id;
	}

	private SiteInfo BuildSiteInfoViaManagementServer(ManagementServer mgt)
	{
		SiteInfo siteInfo = new SiteInfo();
		siteInfo.Name = mgt.DisplayName;
		siteInfo.Id = new Guid(mgt.Id);
		siteInfo.ServerType = "XPCO";
		siteInfo.Description = mgt.Description;
		siteInfo.LastModified = mgt.LastModified;
		siteInfo.ConnectionState = "";
		siteInfo.ServerVersion = mgt.Version.ToString();
		siteInfo.ServiceAccount = mgt.ServiceAccount;
		siteInfo.SyncStatus = 0;
		int productCode = 600;
		ISystemInfo systemInfo = ((SDKConfiguration)Configuration.Instance).GetSystemInfo(siteInfo.Id);
		if (systemInfo != null)
		{
			productCode = new LicenseManager(systemInfo.ServerId.UserContext).ProductCode;
		}
		siteInfo.ServerSubType = productCode.ToString() ?? "";
		siteInfo.ProductCode = productCode;
		foreach (SystemAddressChildItem systemAddressChildItem in mgt.SystemAddressFolder.SystemAddressChildItems)
		{
			siteInfo.AddressCollection.Add(new Address
			{
				Type = "Server",
				Url = systemAddressChildItem.Url
			});
		}
		siteInfo.Parent = Guid.Empty;
		lock (_lockObject)
		{
			if (_siteInfoDictionary.ContainsKey(siteInfo.Id))
			{
				EnvironmentManager.Instance.Log(error: true, "SiteInfoManager:SiteInfo", "Site already added - new add ignored");
			}
			else
			{
				_siteInfoDictionary.Add(siteInfo.Id, siteInfo);
			}
		}
		_lastRefresh = DateTime.UtcNow;
		return siteInfo;
	}

	private SiteInfo BuildSiteInfoViaSite(Site site, Guid parent)
	{
		SiteInfo siteInfo = new SiteInfo();
		siteInfo.Name = site.DisplayName;
		siteInfo.Id = new Guid(site.Id);
		siteInfo.ServerType = "XPCO";
		siteInfo.Description = site.Description;
		siteInfo.LastModified = site.LastModified;
		siteInfo.ConnectionState = "";
		siteInfo.ServerVersion = site.Version.ToString();
		siteInfo.ServiceAccount = site.ServiceAccount;
		siteInfo.SyncStatus = 0;
		int productCode = 600;
		ISystemInfo systemInfo = ((SDKConfiguration)Configuration.Instance).GetSystemInfo(siteInfo.Id);
		if (systemInfo != null)
		{
			productCode = new LicenseManager(systemInfo.ServerId.UserContext).ProductCode;
		}
		siteInfo.ServerSubType = productCode.ToString() ?? "";
		siteInfo.ProductCode = productCode;
		foreach (SiteAddressChildItem siteAddressChildItem in site.SiteAddressFolder.SiteAddressChildItems)
		{
			siteInfo.AddressCollection.Add(new Address
			{
				Type = "Server",
				Url = siteAddressChildItem.Url
			});
		}
		siteInfo.Parent = parent;
		lock (_lockObject)
		{
			if (_siteInfoDictionary.ContainsKey(siteInfo.Id))
			{
				EnvironmentManager.Instance.Log(error: true, "SiteInfoManager:SiteInfo", "Site already added - new add ignored");
			}
			else
			{
				_siteInfoDictionary.Add(siteInfo.Id, siteInfo);
			}
		}
		foreach (Site site2 in site.SiteFolder.Sites)
		{
			BuildSiteInfoViaSite(site2, siteInfo.Id);
		}
		_lastRefresh = DateTime.UtcNow;
		return siteInfo;
	}

	public static Binding GetBinding(bool isBasicUser, LoginSettings loginSettings)
	{
		if (loginSettings.SecureOnly || loginSettings.IsOAuthConnection)
		{
			return ManagementServerWcfHelper.GetOAuthBinding(loginSettings.Uri.Scheme == Uri.UriSchemeHttps);
		}
		if (isBasicUser)
		{
			return new BasicHttpBinding
			{
				Security = 
				{
					Mode = BasicHttpSecurityMode.Transport,
					Transport = 
					{
						ClientCredentialType = HttpClientCredentialType.Basic
					}
				},
				MaxBufferPoolSize = 2147483647L,
				MaxReceivedMessageSize = 2147483647L,
				ReaderQuotas = XmlDictionaryReaderQuotas.Max
			};
		}
		return new WSHttpBinding
		{
			Security = 
			{
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Windows
				}
			},
			MaxBufferPoolSize = 2147483647L,
			MaxReceivedMessageSize = 2147483647L,
			ReaderQuotas = XmlDictionaryReaderQuotas.Max
		};
	}
}
