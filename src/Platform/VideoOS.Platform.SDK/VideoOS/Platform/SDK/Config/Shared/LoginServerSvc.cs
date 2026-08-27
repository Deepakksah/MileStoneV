using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.Platform.License;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Proxy;
using VideoOS.Platform.SDK.Util;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Config.Shared;

public class LoginServerSvc : LoginServerBase, ILoginServer
{
	private ServerCommandServiceClient _serverCommandService;

	private IConfigurationInfo _corporateSystemInfo;

	private readonly Uri _uri;

	private readonly NetworkCredential _localCredentials;

	private const string CommandServicePath = "ManagementServer/ServerCommandService.svc";

	private const string DomainForBasicAuthentication = "[BASIC]";

	private readonly object _lockObject = new object();

	private readonly string _objectTypeName;

	private Exception _lastException;

	private bool _isOnlineSupported = true;

	public override Exception LastException => _lastException;

	public IConfigurationInfo CorporateSystemInfo => _corporateSystemInfo;

	private bool IsBasicUser => _systemInfo.AuthenticationType == AuthenticationType.Basic;

	public LoginServerSvc(UserContext userContext, ISystemInfo systemInfo)
	{
		ServerType = ServerTypes.CORPORATE;
		UserContext = userContext;
		_systemInfo = systemInfo;
		_uri = CalculateServiceUrl(systemInfo.Uri);
		_objectTypeName = GetType().Name;
		if (!(UserContext.GetLoginSettings(systemInfo.ServerId) ?? throw new MIPException("LoginSettings not found")).IsOAuthIdentity)
		{
			_localCredentials = GetNetworkCredential(systemInfo);
		}
	}

	public virtual Uri CalculateServiceUrl(Uri uri)
	{
		return UriUtil.ConstructLegacyServiceUri(uri, IsBasicUser, "ManagementServer/ServerCommandService.svc");
	}

	public override void LogoutService(Guid instanceGuid, string token)
	{
		if (_serverCommandService != null)
		{
			try
			{
				_serverCommandService.Logout(instanceGuid, LoginInfo.Token);
				CloseConnection();
			}
			catch (Exception)
			{
				CloseConnection();
			}
		}
	}

	public override void TokenExpireTimer_Callback(object state)
	{
		LoginSync();
	}

	public bool GetSystemInfoAndLogin()
	{
		bool flag = false;
		_corporateSystemInfo = null;
		try
		{
			if (!base.UsingLogin)
			{
				LoginInfo = LoginService(base.InstanceGuid, "");
				CreateTokenTimer();
				NewTokenAquired();
			}
			ServerTrace.BeforeCall(_objectTypeName, "GetSystemInfoAndLogin.GetConfiguration", _uri.ToString());
			_corporateSystemInfo = CallWithRetry((Func<ServerCommandServiceClient, IConfigurationInfo>)((ServerCommandServiceClient client) => client.GetConfiguration(LoginInfo.Token)));
			ServerTrace.AfterCall(_objectTypeName, "GetSystemInfoAndLogin.GetConfiguration", _uri.ToString());
			_systemInfo.Properties["ProductName"] = "";
			string text = null;
			string text2 = null;
			try
			{
				ServerTrace.BeforeCall(_objectTypeName, "GetSystemInfoAndLogin.GetProductInfo", _uri.ToString());
				ProductInfo productInfo = CallWithRetry((ServerCommandServiceClient client) => client.GetProductInfo());
				if (productInfo != null)
				{
					_systemInfo.Properties["ProductName"] = productInfo.ProductName;
					if (productInfo.ProductCode != 0)
					{
						text = productInfo.ProductCode.ToString() ?? "";
					}
					if (!string.IsNullOrEmpty(productInfo.SLC))
					{
						text2 = productInfo.SLC;
					}
				}
			}
			catch
			{
			}
			finally
			{
				ServerTrace.AfterCall(_objectTypeName, "GetSystemInfoAndLogin.GetProductInfo", _uri.ToString());
			}
			GetGuidAndName();
			_systemInfo.Name = _corporateSystemInfo.ServerName;
			try
			{
				if (UserContext.Configuration.ServerFQID.ServerId.ServerType == "XP")
				{
					UserContext.Configuration.ServerFQID.ServerId = _systemInfo.ServerId;
				}
				if (text != null && text2 != null)
				{
					_systemInfo.Properties["ServerType"] = text;
					_systemInfo.Properties["SLC"] = text2;
				}
				else
				{
					LicenseManager licenseManager = new LicenseManager(UserContext);
					_systemInfo.Properties["ServerType"] = licenseManager.ProductCode.ToString();
					_systemInfo.Properties["SLC"] = licenseManager.SLC;
				}
			}
			catch
			{
			}
			return true;
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance.Log(error: true, "PerformLogin", message, args);
			return false;
		}
	}

	public override void GetGuidAndName()
	{
		try
		{
			if (_systemInfo.LoginSettings.Guid == Guid.Empty)
			{
				using ServerClient serverClient = new ServerClient(_systemInfo.LoginSettings, GetBinding(_uri));
				serverClient.Initialize();
				serverClient.CheckAuthentication();
				ServerInformation serverInformation = serverClient.GetServerInformation();
				_systemInfo.LoginSettings.Guid = new Guid(serverInformation.InstanceId);
				_systemInfo.LoginSettings.Name = serverInformation.Name;
				_systemInfo.Properties["ProductVersion"] = serverInformation.Version.ToString();
				_systemInfo.Properties["LastModified"] = serverInformation.LastModified.Ticks.ToString(CultureInfo.InvariantCulture);
			}
			if (Configuration.Instance is SDKConfiguration)
			{
				if (_systemInfo.UserContext != null && _systemInfo.UserContext.Configuration != null)
				{
					((SDKConfiguration)_systemInfo.UserContext.Configuration).UpdateServerId(_uri.DnsSafeHost, _systemInfo.LoginSettings.Name, _systemInfo.LoginSettings.Guid, ServerTypes.CORPORATE);
				}
				else
				{
					((SDKConfiguration)Configuration.Instance).UpdateServerId(_uri.DnsSafeHost, _systemInfo.LoginSettings.Name, _systemInfo.LoginSettings.Guid, ServerTypes.CORPORATE);
				}
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance.Log(error: true, "PerformLogin-ServerClient", message, args);
		}
	}

	public ISystemLicenseInfo GetSystemLicenseInfo()
	{
		try
		{
			return CallWithRetry((Func<ServerCommandServiceClient, ISystemLicenseInfo>)((ServerCommandServiceClient client) => client.GetSystemLicenseInfo()));
		}
		catch
		{
			return null;
		}
	}

	private static ServerTypes GetServerType()
	{
		return ServerTypes.CORPORATE;
	}

	public override bool IsServerAvailable(Uri uri)
	{
		ServerTrace.BeforeCall(GetType().Name, "IsServerAvailable", uri.ToString());
		_lastException = null;
		try
		{
			lock (_lockObject)
			{
				if (_serverCommandService == null)
				{
					Connect();
				}
				if (_serverCommandService == null)
				{
					return false;
				}
			}
			bool flag = false;
			try
			{
				if (_isOnlineSupported)
				{
					return CallWithRetry((ServerCommandServiceClient client) => client.IsOnline());
				}
			}
			catch (MessageSecurityException ex)
			{
				WebException ex2 = ex.InnerException as WebException;
				if (ex2?.Response is HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized })
				{
					_lastException = ex2;
					return false;
				}
				_isOnlineSupported = false;
				flag = true;
			}
			catch (Exception lastException)
			{
				_lastException = lastException;
				flag = true;
			}
			ServerType = GetServerType();
			return ServerType != ServerTypes.UNKNOWN && !flag;
		}
		finally
		{
			ServerTrace.AfterCall(GetType().Name, "IsServerAvailable", uri.ToString() + ", onlineSupported=" + _isOnlineSupported);
		}
	}

	private void CallWithRetry(Action<ServerCommandServiceClient> call)
	{
		_ = Task.Run(() => CallWithRetry(delegate(ServerCommandServiceClient s)
		{
			call(s);
			return (string)null;
		})).Result;
	}

	private TResult CallWithRetry<TResult>(Func<ServerCommandServiceClient, TResult> call)
	{
		lock (_lockObject)
		{
			try
			{
				if (_serverCommandService == null || _serverCommandService.State != CommunicationState.Opened)
				{
					Connect();
				}
				return call(_serverCommandService);
			}
			catch (MessageSecurityException ex)
			{
				CloseConnection();
				if ((ex.InnerException as WebException)?.Response is HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized })
				{
					throw;
				}
				if (ex.InnerException is IdpException)
				{
					throw;
				}
				Connect();
				try
				{
					return call(_serverCommandService);
				}
				catch (Exception)
				{
					CloseConnection();
					throw;
				}
			}
			catch (FaultException ex3)
			{
				if (ex3.Code != null && ex3.Code.Name != null)
				{
					if (ex3.Code.Name == "50000" || ex3.Code.Name == "51000")
					{
						throw new NotAuthorizedMIPException(ex3.Message, ex3);
					}
					if (ex3.Code.Name == "51100")
					{
						throw new NotDualAuthorizedMIPException(ex3.Message, ex3);
					}
					if (ex3.Code.Name == "Sender" && ex3.GetType().FullName == "System.ServiceModel.Dispatcher.NetDispatcherFaultException")
					{
						try
						{
							return call(_serverCommandService);
						}
						catch (Exception ex4)
						{
							throw new MIPException("NetDispatcherFaultException retry failed with: " + ex4.Message, ex4);
						}
					}
				}
				throw new MIPException(ex3.Message, ex3);
			}
			catch (CommunicationException ex5)
			{
				CloseConnection();
				if (ex5.InnerException is IdpException)
				{
					throw;
				}
				if ((ex5.InnerException as WebException)?.Response is HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized })
				{
					throw;
				}
				Connect();
				try
				{
					return call(_serverCommandService);
				}
				catch (Exception)
				{
					CloseConnection();
					throw;
				}
			}
			catch (Exception ex7)
			{
				EnvironmentManager.Instance.Log(error: false, _objectTypeName, _uri?.ToString() + " " + ex7.Message);
				CloseConnection();
				Connect();
				try
				{
					return call(_serverCommandService);
				}
				catch (Exception ex8)
				{
					EnvironmentManager.Instance.Log(error: true, _objectTypeName, _uri?.ToString() + " " + ex8.Message);
					throw;
				}
			}
		}
	}

	private void Connect()
	{
		lock (_lockObject)
		{
			if (_serverCommandService == null || _serverCommandService.State != CommunicationState.Opened)
			{
				_serverCommandService = CreateClient(_uri, GetBinding(_uri), _systemInfo.LoginSettings.IdentityTokenCache);
				SetCredentials(_serverCommandService.ClientCredentials, _localCredentials);
				_serverCommandService.Open();
				long num = DateTime.Now.Ticks + 1000000;
				int num2 = 0;
				while (DateTime.Now.Ticks < num && _serverCommandService.State == CommunicationState.Opening)
				{
					num2++;
					Thread.Sleep(5);
				}
				_ = 0;
			}
		}
	}

	public virtual ServerCommandServiceClient CreateClient(Uri uri, Binding binding, IMipTokenCache mipTokenCache)
	{
		if (uri.Scheme == Uri.UriSchemeHttp)
		{
			EnvironmentManager.Instance.Log(error: false, "LoginServerSvc.CreateClient", "Creating an HTTP client using uri: " + uri.ToString());
		}
		SpnEndpointIdentity identity = new SpnEndpointIdentity(SpnFactory.GetSpn(uri));
		ServerCommandServiceClient serverCommandServiceClient = new ServerCommandServiceClient(binding, new EndpointAddress(uri, identity));
		ManagementServerWcfHelper.SetupCertificateValidation(serverCommandServiceClient);
		foreach (OperationDescription operation in serverCommandServiceClient.Endpoint.Contract.Operations)
		{
			IOperationBehavior operationBehavior = operation.Behaviors[typeof(DataContractSerializerOperationBehavior)];
			if (operationBehavior != null && operationBehavior is DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
			}
		}
		return serverCommandServiceClient;
	}

	private NetworkCredential GetNetworkCredential(ISystemInfo systemInfo)
	{
		if (IsBasicUser)
		{
			NetworkCredential credentials = systemInfo.Credentials;
			return new NetworkCredential(credentials.UserName, credentials.Password, "[BASIC]");
		}
		return systemInfo.Credentials;
	}

	public virtual void CloseConnection()
	{
		lock (_lockObject)
		{
			if (_serverCommandService == null)
			{
				return;
			}
			try
			{
				_serverCommandService.Close();
			}
			catch
			{
				try
				{
					_serverCommandService.Abort();
				}
				catch
				{
				}
			}
			_serverCommandService = null;
		}
	}

	public virtual Binding GetBinding(Uri uri)
	{
		Binding binding = ((!IsBasicUser) ? ((Binding)new WSHttpBinding
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
		}) : ((Binding)new BasicHttpBinding
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
		}));
		TimeSpan receiveTimeout = (binding.CloseTimeout = (binding.OpenTimeout = (binding.SendTimeout = ((Environment.Properties.LoginServerSvcTimeoutInSeconds == 0) ? TimeSpan.FromMinutes(10.0) : TimeSpan.FromSeconds(Environment.Properties.LoginServerSvcTimeoutInSeconds)))));
		binding.ReceiveTimeout = receiveTimeout;
		return binding;
	}

	public virtual void SetCredentials(ClientCredentials clientCredentials, NetworkCredential nc)
	{
		if (IsBasicUser)
		{
			string userName = ((!string.IsNullOrEmpty(nc.Domain)) ? (nc.Domain + "\\" + nc.UserName) : nc.UserName);
			clientCredentials.UserName.UserName = userName;
			clientCredentials.UserName.Password = nc.Password;
			clientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
			{
				CertificateValidationMode = X509CertificateValidationMode.None
			};
		}
		else
		{
			clientCredentials.Windows.ClientCredential = nc;
		}
	}

	public void LoginSync()
	{
		if (LoginInProgress)
		{
			return;
		}
		try
		{
			LoginInProgress = true;
			LoginInfo = CallWithRetry((Func<ServerCommandServiceClient, ILoginInfo>)((ServerCommandServiceClient client) => client.Login(base.InstanceGuid, LoginInfo.Token)));
			NewTokenAquired();
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "TokenExpireTimer_Callback", ex.Message);
			NewTokenNotAquired();
		}
		finally
		{
			LoginInProgress = false;
		}
	}

	public override ILoginInfo LoginService(Guid instanceGuid, string currentToken)
	{
		ServerTrace.BeforeCall(_objectTypeName, "LoginService", _uri.ToString());
		try
		{
			return CallWithRetry((ServerCommandServiceClient client) => client.Login(instanceGuid, currentToken));
		}
		finally
		{
			ServerTrace.AfterCall(_objectTypeName, "LoginService", _uri.ToString());
		}
	}

	public virtual Task<IBookmark[]> BookmarkSearchFromBookmarkAsync(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return CallWithRetry((Func<ServerCommandServiceClient, Task<IBookmark[]>>)(async (ServerCommandServiceClient client) => await client.BookmarkSearchFromBookmarkAsync(LoginInfo.Token, fromBookmarkId, new TimeDuration
		{
			MicroSeconds = timeLimit.MicroSeconds
		}, countLimit, ConvertToWcfTypes(deviceTypes, optDeviceIds == null || optDeviceIds.Length == 0), optDeviceIds ?? Array.Empty<Guid>(), optUsers ?? Array.Empty<string>(), optSearchStr ?? "")));
	}

	public virtual Task<IBookmark[]> BookmarkSearchFromBookmarkAsync(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return CallWithRetry((Func<ServerCommandServiceClient, Task<IBookmark[]>>)(async (ServerCommandServiceClient client) => await client.BookmarkSearchFromBookmark2Async(LoginInfo.Token, fromBookmarkId, new TimeDuration
		{
			MicroSeconds = timeLimit.MicroSeconds
		}, countLimit, ConvertToWcfTypes(deviceTypes, optDeviceIds == null || optDeviceIds.Length == 0), optDeviceIds ?? Array.Empty<Guid>(), optUsers ?? Array.Empty<string>(), optSearchStr ?? "", searchHeader, searchDescription, searchReference, searchUser, headerFilter ?? "", descriptionFilter ?? "")));
	}

	public virtual Task<IBookmark[]> BookmarkSearchTimeAsync(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return CallWithRetry((Func<ServerCommandServiceClient, Task<IBookmark[]>>)(async (ServerCommandServiceClient client) => await client.BookmarkSearchTimeAsync(LoginInfo.Token, time, new TimeDuration
		{
			MicroSeconds = timeLimit.MicroSeconds
		}, countLimit, ConvertToWcfTypes(deviceTypes, optDeviceIds == null || optDeviceIds.Length == 0), optDeviceIds ?? Array.Empty<Guid>(), optUsers ?? Array.Empty<string>(), optSearchStr ?? "")));
	}

	public virtual Task<IBookmark[]> BookmarkSearchTimeAsync(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return CallWithRetry((Func<ServerCommandServiceClient, Task<IBookmark[]>>)(async (ServerCommandServiceClient client) => await client.BookmarkSearchTime2Async(LoginInfo.Token, time, new TimeDuration
		{
			MicroSeconds = timeLimit.MicroSeconds
		}, countLimit, ConvertToWcfTypes(deviceTypes, optDeviceIds == null || optDeviceIds.Length == 0), optDeviceIds ?? Array.Empty<Guid>(), optUsers ?? Array.Empty<string>(), optSearchStr ?? "", searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter)));
	}

	public IChange[] QueryChanges(DateTime changesFrom)
	{
		try
		{
			return CallWithRetry((ServerCommandServiceClient client) => client.QueryChanges(LoginInfo.Token, changesFrom));
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, _objectTypeName + ".QueryChanges", ex.Message, null);
		}
		return null;
	}

	public IRecorderInfo QueryRecorderInfo(Guid recorderId)
	{
		try
		{
			return CallWithRetry((ServerCommandServiceClient client) => client.QueryRecorderInfo(LoginInfo.Token, recorderId));
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, _objectTypeName + ".QueryRecorderInfo", ex.Message, null);
		}
		return null;
	}

	public IAlertOccurrence[] GetAlerts(Guid guid, DateTime time, ITimeDuration timeBefore, int maxCountBefore, ITimeDuration timeAfter, int maxCountAfter)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.AlertsGetAroundWithSpan(LoginInfo.Token, new Guid[1] { guid }, time, (TimeDuration)timeBefore, maxCountBefore, (TimeDuration)timeAfter, maxCountAfter));
	}

	public void MatrixMonitorActivateCamera(Guid matrixGuid, Guid cameraGuid)
	{
		CallWithRetry(delegate(ServerCommandServiceClient client)
		{
			client.MatrixMonitorActivateCamera(LoginInfo.Token, matrixGuid, cameraGuid);
		});
	}

	public void EventTrigger(Guid eventGuid)
	{
		CallWithRetry(delegate(ServerCommandServiceClient client)
		{
			client.EventTrigger(LoginInfo.Token, eventGuid);
		});
	}

	public void EventTriggerWithMetadata(Guid eventGuid, IKeyValue[] metadata)
	{
		KeyValue[] wcfKeyValue = new KeyValue[metadata.Length];
		for (int i = 0; i < metadata.Length; i++)
		{
			wcfKeyValue[i] = new KeyValue
			{
				Key = metadata[i].Key,
				Value = metadata[i].Value
			};
		}
		CallWithRetry(delegate(ServerCommandServiceClient client)
		{
			client.EventTriggerWithMetadata(LoginInfo.Token, eventGuid, wcfKeyValue);
		});
	}

	public ICustomSetting GetCustomSettingDataGlobal(Guid platformDefinitionId)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.GetCustomSettingDataGlobal(LoginInfo.Token, platformDefinitionId));
	}

	public ICustomSetting GetCustomSettingDataUser(Guid platformDefinitionId)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.GetCustomSettingDataUser(LoginInfo.Token, platformDefinitionId));
	}

	public override void SetCustomSettingGlobal(Guid id, XmlNode xml, int version, bool global)
	{
		CustomSettingInternalData data = new CustomSettingInternalData();
		data.CustomSettingDataXml = xml.OuterXml;
		data.DataVersion = version;
		VideoOS.Common.Proxy.Server.WCF.Net.SetCustomSettingResultType setCustomSettingResultType = ((!global) ? CallWithRetry((ServerCommandServiceClient client) => client.SetCustomSettingDataUser(LoginInfo.Token, id, data, forceOverride: false)) : CallWithRetry((ServerCommandServiceClient client) => client.SetCustomSettingDataGlobal(LoginInfo.Token, id, data, forceOverride: false)));
		if (setCustomSettingResultType != VideoOS.Common.Proxy.Server.WCF.Net.SetCustomSettingResultType.Success)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string text = _objectTypeName + ".SetCustomSettingData";
			Guid guid = id;
			instance.Log(error: true, text, "Unable to update or save for id=" + guid.ToString());
		}
	}

	public Item GetItemConfiguration(Guid platformDefinitionId, Guid itemKind, Guid itemId)
	{
		string objectTypeName = _objectTypeName;
		Guid guid = platformDefinitionId;
		ServerTrace.BeforeCall(objectTypeName, "GetItemConfiguration", "Id:" + guid.ToString());
		try
		{
			ICustomSetting customSettingDataGlobal = GetCustomSettingDataGlobal(platformDefinitionId);
			if (customSettingDataGlobal == null)
			{
				return null;
			}
			XmlNode customSettingDataXml = customSettingDataGlobal.CustomSettingData.CustomSettingDataXml;
			if (customSettingDataXml != null)
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(customSettingDataXml.OuterXml);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("/PlatformConfiguration");
				if (xmlNode == null)
				{
					return null;
				}
				XmlNode xmlNode2 = xmlNode.SelectSingleNode("Configurations/GUID" + itemKind.ToString() + "[@itemID=\"" + itemId.ToString() + "\"]");
				if (xmlNode2 == null)
				{
					return null;
				}
				Item item = ConfigurationUtil.DeserializeItem(xmlNode2);
				item.FQID.ServerId.UserContext = UserContext;
				return item;
			}
			return null;
		}
		catch (Exception)
		{
			return null;
		}
		finally
		{
			string objectTypeName2 = _objectTypeName;
			guid = platformDefinitionId;
			ServerTrace.AfterCall(objectTypeName2, "GetItemConfiguration", "Id:" + guid.ToString());
		}
	}

	public IRecorderInfo[] GetRecorderInfos(Guid[] recorderIds)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.GetConfigurationRecorders(LoginInfo.Token, recorderIds));
	}

	public IHardwareInfo[] GetHardwareInfos(Guid[] hardwareIds)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.GetConfigurationHardware(LoginInfo.Token, hardwareIds));
	}

	public virtual IBookmarkReference BookmarkGetNewReference(Guid deviceId, bool live)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.BookmarkGetNewReference(LoginInfo.Token, deviceId, live));
	}

	public virtual void BookmarkDelete(Guid bookmarkId)
	{
		CallWithRetry(delegate(ServerCommandServiceClient client)
		{
			client.BookmarkDelete(LoginInfo.Token, bookmarkId);
		});
	}

	public virtual IBookmark BookmarkCreate(Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.BookmarkCreate(LoginInfo.Token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description));
	}

	public virtual IBookmark[] BookmarkSearchTime(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.BookmarkSearchTime(LoginInfo.Token, time, new TimeDuration
		{
			MicroSeconds = timeLimit.MicroSeconds
		}, countLimit, ConvertToWcfTypes(deviceTypes, optDeviceIds == null || optDeviceIds.Length == 0), optDeviceIds ?? Array.Empty<Guid>(), optUsers ?? Array.Empty<string>(), optSearchStr ?? ""));
	}

	public virtual IBookmark[] BookmarkSearchTime(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.BookmarkSearchTime2(LoginInfo.Token, time, new TimeDuration
		{
			MicroSeconds = timeLimit.MicroSeconds
		}, countLimit, ConvertToWcfTypes(deviceTypes, optDeviceIds == null || optDeviceIds.Length == 0), optDeviceIds ?? Array.Empty<Guid>(), optUsers ?? Array.Empty<string>(), optSearchStr ?? "", searchHeader, searchDescription, searchReference, searchUser, headerFilter ?? "", descriptionFilter ?? ""));
	}

	private static MediaDeviceType[] ConvertToWcfTypes(IMediaDeviceType[] originalDeviceTypes, bool provideDefaults)
	{
		if (originalDeviceTypes == null)
		{
			if (provideDefaults)
			{
				return new MediaDeviceType[3]
				{
					MediaDeviceType.Camera,
					MediaDeviceType.Microphone,
					MediaDeviceType.Speaker
				};
			}
			return Array.Empty<MediaDeviceType>();
		}
		return originalDeviceTypes.Select((IMediaDeviceType dt) => GetMediaDeviceType(dt)).ToArray();
	}

	private static MediaDeviceType GetMediaDeviceType(IMediaDeviceType mediaDeviceType)
	{
		return mediaDeviceType switch
		{
			IMediaDeviceType.Camera => MediaDeviceType.Camera, 
			IMediaDeviceType.Microphone => MediaDeviceType.Microphone, 
			IMediaDeviceType.Speaker => MediaDeviceType.Speaker, 
			_ => MediaDeviceType.Camera, 
		};
	}

	public virtual IBookmark BookmarkGet(Guid bookmarkId)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.BookmarkGet(LoginInfo.Token, bookmarkId));
	}

	public virtual IBookmark BookmarkUpdate(Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.BookmarkUpdate(LoginInfo.Token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description));
	}

	public virtual IBookmark[] BookmarkSearchFromBookmark(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.BookmarkSearchFromBookmark(LoginInfo.Token, fromBookmarkId, new TimeDuration
		{
			MicroSeconds = timeLimit.MicroSeconds
		}, countLimit, ConvertToWcfTypes(deviceTypes, optDeviceIds == null || optDeviceIds.Length == 0), optDeviceIds ?? Array.Empty<Guid>(), optUsers ?? Array.Empty<string>(), optSearchStr ?? ""));
	}

	public virtual IBookmark[] BookmarkSearchFromBookmark(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.BookmarkSearchFromBookmark2(LoginInfo.Token, fromBookmarkId, new TimeDuration
		{
			MicroSeconds = timeLimit.MicroSeconds
		}, countLimit, ConvertToWcfTypes(deviceTypes, optDeviceIds == null || optDeviceIds.Length == 0), optDeviceIds ?? Array.Empty<Guid>(), optUsers ?? Array.Empty<string>(), optSearchStr ?? "", searchHeader, searchDescription, searchReference, searchUser, headerFilter ?? "", descriptionFilter ?? ""));
	}

	public IRestrictedMediaResult RestrictedMediaCreate(Guid restrictedMediaId, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.RestrictedMediaCreate(restrictedMediaId, deviceIds, header, description, timeStart, timeEnd));
	}

	public IRestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.RestrictedMediaDelete(restrictedMediaId));
	}

	public IRestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.RestrictedMediaLiveDelete(deviceIds));
	}

	public IRestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.RestrictedMediaLiveModeEnter(deviceIds, timeStart));
	}

	public IRestrictedMediaResult RestrictedMediaLiveModeExit(Guid restrictedMediaId, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.RestrictedMediaLiveModeExit(restrictedMediaId, deviceIds, header, description, timeStart, timeEnd));
	}

	public IRestrictedMediaLive[] RestrictedMediaLiveQueryAll()
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.RestrictedMediaLiveQueryAll());
	}

	public IRestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.RestrictedMediaLiveUpdate(deviceIds, timeStart));
	}

	public IRestrictedMedia[] RestrictedMediaQueryAll()
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.RestrictedMediaQueryAll());
	}

	public IRestrictedMediaResult RestrictedMediaUpdate(Guid restrictedMediaId, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.RestrictedMediaUpdate(restrictedMediaId, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd));
	}

	public IViewGroup GetViewGroupData(Guid groupId)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.GetViewGroupData(LoginInfo.Token, groupId));
	}

	public IViewGroupInfo[] GetViewGroups()
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.GetViewGroups(LoginInfo.Token));
	}

	public IPresetInfo[] GetPresets(Guid deviceId)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.GetPresets(LoginInfo.Token, deviceId));
	}

	public void DeletePreset(Guid id, Guid deviceId)
	{
		CallWithRetry(delegate(ServerCommandServiceClient client)
		{
			client.DeletePreset(LoginInfo.Token, id, deviceId);
		});
	}

	public void UpdatePresetName(Guid id, Guid deviceId, string newName)
	{
		CallWithRetry(delegate(ServerCommandServiceClient client)
		{
			client.UpdatePresetName(LoginInfo.Token, id, deviceId, newName);
		});
	}

	public void UpdatePresetPosition(Guid id, Guid deviceId)
	{
		CallWithRetry(delegate(ServerCommandServiceClient client)
		{
			client.UpdatePresetPosition(LoginInfo.Token, id, deviceId);
		});
	}

	public Guid CreatePreset(Guid deviceId, string name)
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.CreatePreset(LoginInfo.Token, deviceId, name));
	}

	public void OrderPresets(Guid deviceId, Guid[] ids)
	{
		CallWithRetry(delegate(ServerCommandServiceClient client)
		{
			client.OrderPresets(LoginInfo.Token, deviceId, ids);
		});
	}

	public IDeviceDisabledInfo[] GetDisabledDevices()
	{
		return CallWithRetry((ServerCommandServiceClient client) => client.GetDevicesDisabled(LoginInfo.Token));
	}

	public override void RegisterIntegration(string instanceId, Guid integrationId, string integrationName, string version, string manufacturerName)
	{
		try
		{
			_serverCommandService.RegisterIntegration(LoginInfo.Token, instanceId, integrationId, version, integrationName, manufacturerName);
		}
		catch
		{
		}
	}
}
