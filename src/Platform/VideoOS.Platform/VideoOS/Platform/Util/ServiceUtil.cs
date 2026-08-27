using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using VideoOS.Platform.Login;
using VideoOS.Platform.OAuth;
using VideoOS.Platform.Util.Svc;

namespace VideoOS.Platform.Util;

public static class ServiceUtil
{
	public class AddTokenMessageInspector : IClientMessageInspector
	{
		private string _token;

		public AddTokenMessageInspector(string token)
		{
			_token = token;
		}

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			MessageHeader untypedHeader = new MessageHeader<string>(_token).GetUntypedHeader("token", "");
			request.Headers.Add(untypedHeader);
			return null;
		}
	}

	public class BearerAuthorizationHeaderInspector : IClientMessageInspector
	{
		private readonly IMipTokenCache _mipTokenCache;

		private const HttpRequestHeader AuthorizationHeader = HttpRequestHeader.Authorization;

		public BearerAuthorizationHeaderInspector(IMipTokenCache mipTokenCache)
		{
			_mipTokenCache = mipTokenCache;
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request", "Invalid Argument");
			}
			if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out var value))
			{
				HttpRequestMessageProperty httpRequestMessageProperty = (HttpRequestMessageProperty)value;
				if (string.IsNullOrEmpty(httpRequestMessageProperty.Headers[HttpRequestHeader.Authorization]))
				{
					httpRequestMessageProperty.Headers[HttpRequestHeader.Authorization] = FormatToken();
				}
			}
			else
			{
				HttpRequestMessageProperty httpRequestMessageProperty2 = new HttpRequestMessageProperty();
				httpRequestMessageProperty2.Headers.Add(HttpRequestHeader.Authorization, FormatToken());
				request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessageProperty2);
			}
			return null;
		}

		private string FormatToken()
		{
			try
			{
				return FormattableString.Invariant($"Bearer {_mipTokenCache.Token}");
			}
			catch (Exception ex)
			{
				throw new CommunicationException("MIP: Failed to receive token from IDP " + ex.Message, ex);
			}
		}

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
		}
	}

	public class AddTokenBehavior : BehaviorExtensionElement, IEndpointBehavior
	{
		private LoginSettings _loginSettings;

		public override Type BehaviorType => typeof(AddTokenBehavior);

		public AddTokenBehavior(LoginSettings loginSettings)
		{
			_loginSettings = loginSettings;
		}

		public void AddBindingParameters(System.ServiceModel.Description.ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(System.ServiceModel.Description.ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			if (_loginSettings.IdentityTokenCache != null)
			{
				clientRuntime.ClientMessageInspectors.Add(new BearerAuthorizationHeaderInspector(_loginSettings.IdentityTokenCache));
			}
			else
			{
				clientRuntime.ClientMessageInspectors.Add(new AddTokenMessageInspector(_loginSettings.Token));
			}
		}

		public void ApplyDispatchBehavior(System.ServiceModel.Description.ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		public void Validate(System.ServiceModel.Description.ServiceEndpoint endpoint)
		{
		}

		protected override object CreateBehavior()
		{
			return new AddTokenBehavior(_loginSettings);
		}
	}

	private static IServiceRegistration CreateServiceRegistrationProxy(ServerId serverId, NetworkCredential credential)
	{
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		bool flag = false;
		LoginSettings loginSettings = null;
		if (serverId.UserContext != null)
		{
			loginSettings = serverId.UserContext.GetLoginSettings(serverId.Id);
		}
		if (loginSettings == null)
		{
			loginSettings = LoginSettingsCache.GetLoginSettings(serverId);
		}
		if (loginSettings == null)
		{
			EnvironmentManager.Instance.Log(error: false, "ServiceUtil.CreateServiceRegistrationProxy", "Warning: Unable to look up whether provided user is a basic user (therefore assuming it is not) for server: " + serverId.ServerHostname);
		}
		else
		{
			flag = loginSettings.IsBasicUser;
		}
		if (EnvironmentManager.Instance.TraceFunctionCalls)
		{
			EnvironmentManager.Instance.Log(error: false, "CreateServiceRegistrationProxy", "isBasic =" + flag + ", ServerType=" + serverId.ServerType);
		}
		if (serverId.ServerType == "XPCO" || serverId.ServerType == "XP")
		{
			if (EnvironmentManager.Instance.DebugLoggingEnabled && (loginSettings == null || loginSettings.IdentityTokenCache == null))
			{
				EnvironmentManager.Instance.Log(error: false, "CreateServiceRegistrationProxy", "IdentityCache not initialized");
			}
			if (loginSettings?.IdentityTokenCache != null)
			{
				IServiceRegistration serviceRegistration = ServiceRegistrationRestClientDecorator.Create(loginSettings);
				if (serviceRegistration != null)
				{
					return serviceRegistration;
				}
				string text = new UriBuilder(loginSettings.Uri.Scheme, loginSettings.Uri.DnsSafeHost, loginSettings.Uri.Port, "/ManagementServer/ServiceRegistrationServiceOAuth.svc").Uri.ToString();
				if (text.Contains("http://fakeaddress"))
				{
					return null;
				}
				ServiceRegistrationApiSvcOauthDecorator serviceRegistrationApiSvcOauthDecorator = new ServiceRegistrationApiSvcOauthDecorator(loginSettings);
				serviceRegistrationApiSvcOauthDecorator.Url = text;
				try
				{
					ServiceInfo[] services = serviceRegistrationApiSvcOauthDecorator.GetServices();
					if (services.Length != 0)
					{
						if (EnvironmentManager.Instance.TraceFunctionCalls)
						{
							EnvironmentManager.Instance.Log(error: false, "ServiceReg:", text + ", Count=" + services.Length.ToString(CultureInfo.InvariantCulture));
						}
						return serviceRegistrationApiSvcOauthDecorator;
					}
				}
				catch (Exception ex)
				{
					EnvironmentManager.Instance.Log(error: true, "ServiceReg:", text + ", Error: " + ex.Message, null);
				}
				return null;
			}
			if (credential == null && loginSettings != null)
			{
				credential = loginSettings.CredentialCache.GetCredential(serverId.Uri, loginSettings.IsBasicUser ? "Basic" : "Negotiate");
			}
			if (credential != null)
			{
				string text = ((!flag) ? ((serverId.Serverport != 0 && serverId.Serverport != 443) ? new UriBuilder(Uri.UriSchemeHttp, serverId.ServerHostname, serverId.Serverport).Uri.ToString() : new UriBuilder(Uri.UriSchemeHttp, serverId.ServerHostname).Uri.ToString()) : ((serverId.Serverport != 0 && serverId.Serverport != 80) ? new UriBuilder(Uri.UriSchemeHttps, serverId.ServerHostname, serverId.Serverport).Uri.ToString() : new UriBuilder(Uri.UriSchemeHttps, serverId.ServerHostname).Uri.ToString()));
				if (text.Contains("http://fakeaddress"))
				{
					return null;
				}
				string text2 = text.Remove(text.LastIndexOf('/'));
				text2 += "/ManagementServer/ServiceRegistrationService.svc";
				ServiceRegistrationApiSvcDecorator serviceRegistrationApiSvcDecorator = new ServiceRegistrationApiSvcDecorator();
				serviceRegistrationApiSvcDecorator.Credentials = credential;
				serviceRegistrationApiSvcDecorator.Url = text2;
				serviceRegistrationApiSvcDecorator.IsBasicUser = flag;
				try
				{
					ServiceInfo[] services2 = serviceRegistrationApiSvcDecorator.GetServices();
					if (services2.Length != 0)
					{
						if (EnvironmentManager.Instance.TraceFunctionCalls)
						{
							EnvironmentManager.Instance.Log(error: false, "ServiceReg:", text2 + ", Count=" + services2.Length.ToString(CultureInfo.InvariantCulture));
						}
						return serviceRegistrationApiSvcDecorator;
					}
				}
				catch (Exception)
				{
					EnvironmentManager.Instance.Log(error: false, "ServiceReg:", text2, null);
				}
			}
		}
		return null;
	}

	[Obsolete("GetRegisteredServiceUriInfo with network credentials is obsolete")]
	public static List<Configuration.ServiceURIInfo> GetRegisteredServiceUriInfo(ServerId serverId, NetworkCredential credential)
	{
		if (credential != null)
		{
			EnvironmentManager.Instance.Log(error: false, "GetRegisteredServiceUriInfo", "Warning: Network credentials has been provided, method is obsolete. Use the calling method without network credentials.");
		}
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		List<Configuration.ServiceURIInfo> list = new List<Configuration.ServiceURIInfo>();
		IServiceRegistration serviceRegistration = null;
		try
		{
			serviceRegistration = CreateServiceRegistrationProxy(serverId, credential);
			if (serviceRegistration != null)
			{
				ServiceInfo[] services = serviceRegistration.GetServices();
				foreach (ServiceInfo serviceInfo in services)
				{
					string serviceData = "";
					try
					{
						serviceData = serviceRegistration.GetServiceData(serviceInfo.instance);
					}
					catch (Exception)
					{
					}
					List<string> list2 = new List<string>(serviceInfo.uri);
					List<Configuration.RegistrationServiceEndpoint> endpoints = new List<Configuration.RegistrationServiceEndpoint>();
					if (serviceInfo.ServiceEndpoints != null)
					{
						endpoints = serviceInfo.ServiceEndpoints;
						if (list2.Count == 0 && serviceInfo.ServiceEndpoints.Count > 0)
						{
							list2.Add(serviceInfo.ServiceEndpoints[0].Uri);
						}
					}
					list.Add(new Configuration.ServiceURIInfo
					{
						Description = serviceInfo.description,
						Instance = serviceInfo.instance,
						Name = serviceInfo.name,
						Trusted = serviceInfo.trusted,
						Type = serviceInfo.type,
						UriArray = list2,
						ServiceData = serviceData,
						Endpoints = endpoints
					});
				}
				ServiceUtilCache.StoreServiceUriInfoList(serverId, list);
			}
		}
		catch (Exception ex2)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex2.Message;
			object[] args = new Exception[1] { ex2 };
			instance.Log(error: true, "GetRegisteredServiceUriInfo", message, args);
			throw new CommunicationMIPException("GetRegisteredServiceUriInfo: Could not get hold of the services", ex2);
		}
		finally
		{
			if (serviceRegistration != null)
			{
				try
				{
					serviceRegistration.Dispose();
				}
				catch (Exception ex3)
				{
					EnvironmentManager instance2 = EnvironmentManager.Instance;
					string message2 = ex3.Message;
					object[] args = new Exception[1] { ex3 };
					instance2.Log(error: false, "GetRegisteredServiceUriInfo", message2, args);
				}
			}
		}
		return list;
	}

	public static List<Configuration.ServiceURIInfo> GetRegisteredServiceUriInfo(ServerId serverId)
	{
		return GetRegisteredServiceUriInfo(serverId, null);
	}

	[Obsolete("GetRegisteredServiceUriInfo with network credentials is obsolete")]
	public static List<Configuration.ServiceURIInfo> GetRegisteredServiceUriInfo(Guid serviceType, ServerId serverId, NetworkCredential credential)
	{
		if (credential != null)
		{
			EnvironmentManager.Instance.Log(error: false, "GetRegisteredServiceUriInfo", "Warning: Network credentials has been provided, method is obsolete. Use the calling method without network credentials.");
		}
		if (serverId == null)
		{
			throw new ArgumentNullException("serverId");
		}
		List<Configuration.ServiceURIInfo> list = new List<Configuration.ServiceURIInfo>();
		List<Configuration.ServiceURIInfo> list2 = ServiceUtilCache.GetServiceUriInfoList(serverId);
		if (list2 == null)
		{
			list2 = GetRegisteredServiceUriInfo(serverId, credential);
		}
		try
		{
			if (list2 != null)
			{
				list.AddRange(list2.FindAll((Configuration.ServiceURIInfo s) => s.Type == serviceType));
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager instance = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance.Log(error: true, "GetRegisteredServiceUriInfo", message, args);
			throw new CommunicationMIPException("GetRegisteredServiceUriInfo: Could not get hold of the services", ex);
		}
		return list;
	}

	public static List<Configuration.ServiceURIInfo> GetRegisteredServiceUriInfo(Guid serviceType, ServerId serverId)
	{
		return GetRegisteredServiceUriInfo(serviceType, serverId, null);
	}

	[Obsolete("RegisterServiceUri with network credentials is obsolete")]
	public static void RegisterServiceUri(Guid serviceType, ServerId serverId, Guid instance, NetworkCredential credential, Uri uri, string name, string description, string customData)
	{
		if (credential != null)
		{
			EnvironmentManager.Instance.Log(error: false, "RegisterServiceUri", "Warning: Network credentials has been provided, method is obsolete. Use the calling method without network credentials.");
		}
		IServiceRegistration serviceRegistration = null;
		try
		{
			serviceRegistration = CreateServiceRegistrationProxy(serverId, credential);
			if (serviceRegistration != null)
			{
				ArrayOfString uri2 = new ArrayOfString { uri.ToString() };
				serviceRegistration.RegisterService(serviceType, instance, uri2, name, description, customData);
				ServiceUtilCache.Flush(serverId);
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager instance2 = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance2.Log(error: true, "RegisteredServiceUri", message, args);
			throw new CommunicationMIPException("RegisterServiceUri: Could not register the service:" + name, ex);
		}
		finally
		{
			if (serviceRegistration != null)
			{
				try
				{
					serviceRegistration.Dispose();
				}
				catch (Exception ex2)
				{
					EnvironmentManager instance3 = EnvironmentManager.Instance;
					string message2 = ex2.Message;
					object[] args = new Exception[1] { ex2 };
					instance3.Log(error: false, "RegisteredServiceUri", message2, args);
				}
			}
		}
	}

	public static void RegisterServiceUri(Guid serviceType, ServerId serverId, Guid instance, Uri uri, string name, string description, string customData)
	{
		RegisterServiceUri(serviceType, serverId, instance, null, uri, name, description, customData);
	}

	[Obsolete("RegisterServiceUri with network credentials is obsolete")]
	public static void RegisterServiceUri(Guid serviceType, ServerId serverId, Guid instance, NetworkCredential credential, Uri uri, string name, string description, string customData, List<Configuration.RegistrationServiceEndpoint> endpoints)
	{
		if (credential != null)
		{
			EnvironmentManager.Instance.Log(error: false, "RegisterServiceUri", "Warning: Network credentials has been provided, method is obsolete. Use the calling method without network credentials.");
		}
		IServiceRegistration serviceRegistration = null;
		try
		{
			serviceRegistration = CreateServiceRegistrationProxy(serverId, credential);
			if (serviceRegistration == null)
			{
				return;
			}
			List<EndpointDefinition> list = new List<EndpointDefinition>();
			if (endpoints != null)
			{
				foreach (Configuration.RegistrationServiceEndpoint endpoint in endpoints)
				{
					list.Add(new EndpointDefinition(endpoint.Uri, endpoint.Authentication, endpoint.Name, endpoint.Description));
				}
			}
			string asXml = EndpointDefinitions.GetAsXml(list.ToArray());
			ArrayOfString uri2 = new ArrayOfString { uri.ToString() };
			if (list.Count == 0)
			{
				serviceRegistration.RegisterService(serviceType, instance, uri2, name, description, customData);
			}
			else
			{
				try
				{
					serviceRegistration.RegisterServiceWithEndpoints(serviceType, instance, uri2, name, description, customData, asXml);
				}
				catch (Exception)
				{
					serviceRegistration.RegisterService(serviceType, instance, uri2, name, description, customData);
				}
			}
			ServiceUtilCache.Flush(serverId);
		}
		catch (Exception ex2)
		{
			EnvironmentManager instance2 = EnvironmentManager.Instance;
			string message = ex2.Message;
			object[] args = new Exception[1] { ex2 };
			instance2.Log(error: true, "RegisteredServiceUri", message, args);
			throw new CommunicationMIPException("RegisterServiceUri: Could not register the service:" + name, ex2);
		}
		finally
		{
			if (serviceRegistration != null)
			{
				try
				{
					serviceRegistration.Dispose();
				}
				catch (Exception ex3)
				{
					EnvironmentManager instance3 = EnvironmentManager.Instance;
					string message2 = ex3.Message;
					object[] args = new Exception[1] { ex3 };
					instance3.Log(error: false, "RegisteredServiceUri", message2, args);
				}
			}
		}
	}

	public static void RegisterServiceUri(Guid serviceType, ServerId serverId, Guid instance, Uri uri, string name, string description, string customData, List<Configuration.RegistrationServiceEndpoint> endpoints)
	{
		RegisterServiceUri(serviceType, serverId, instance, null, uri, name, description, customData);
	}

	[Obsolete("UnRegisterServiceUri with network credentials is obsolete")]
	public static void UnRegisterServiceUri(ServerId serverId, Guid instance, NetworkCredential credential)
	{
		if (credential != null)
		{
			EnvironmentManager.Instance.Log(error: false, "UnRegisterServiceUri", "Warning: Network credentials has been provided, method is obsolete. Use the calling method without network credentials.");
		}
		IServiceRegistration serviceRegistration = null;
		try
		{
			serviceRegistration = CreateServiceRegistrationProxy(serverId, credential);
			if (serviceRegistration != null)
			{
				serviceRegistration.UnregisterService(instance);
				ServiceUtilCache.Flush(serverId);
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager instance2 = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance2.Log(error: true, "UnRegisterServiceUri", message, args);
			throw new CommunicationMIPException("UnRegisterServiceUri: Could not UnRegister a service", ex);
		}
		finally
		{
			if (serviceRegistration != null)
			{
				try
				{
					serviceRegistration.Dispose();
				}
				catch (Exception ex2)
				{
					EnvironmentManager instance3 = EnvironmentManager.Instance;
					string message2 = ex2.Message;
					object[] args = new Exception[1] { ex2 };
					instance3.Log(error: false, "UnRegisterServiceUri", message2, args);
				}
			}
		}
	}

	public static void UnRegisterServiceUri(ServerId serverId, Guid instance)
	{
		UnRegisterServiceUri(serverId, instance, null);
	}

	[Obsolete("GetServiceData with network credentials is obsolete")]
	public static string GetServiceData(ServerId serverId, Guid instance, NetworkCredential credential)
	{
		if (credential != null)
		{
			EnvironmentManager.Instance.Log(error: false, "GetServiceData", "Warning: Network credentials has been provided, method is obsolete. Use the calling method without network credentials.");
		}
		IServiceRegistration serviceRegistration = null;
		try
		{
			serviceRegistration = CreateServiceRegistrationProxy(serverId, credential);
			if (serviceRegistration != null)
			{
				return serviceRegistration.GetServiceData(instance);
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager instance2 = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance2.Log(error: true, "GetServiceData", message, args);
			throw new CommunicationMIPException("GetServiceData: Could not get service data", ex);
		}
		finally
		{
			if (serviceRegistration != null)
			{
				try
				{
					serviceRegistration.Dispose();
				}
				catch (Exception ex2)
				{
					EnvironmentManager instance3 = EnvironmentManager.Instance;
					string message2 = ex2.Message;
					object[] args = new Exception[1] { ex2 };
					instance3.Log(error: false, "GetServiceData", message2, args);
				}
			}
		}
		return null;
	}

	public static string GetServiceData(ServerId serverId, Guid instance)
	{
		return GetServiceData(serverId, instance, null);
	}

	[Obsolete("SetServiceData with network credentials is obsolete")]
	public static void SetServiceData(ServerId serverId, Guid instance, NetworkCredential credential, string serviceData)
	{
		if (credential != null)
		{
			EnvironmentManager.Instance.Log(error: false, "SetServiceData", "Warning: Network credentials has been provided, method is obsolete. Use the calling method without network credentials.");
		}
		IServiceRegistration serviceRegistration = null;
		try
		{
			serviceRegistration = CreateServiceRegistrationProxy(serverId, credential);
			serviceRegistration?.SetServiceData(instance, serviceData);
		}
		catch (Exception ex)
		{
			EnvironmentManager instance2 = EnvironmentManager.Instance;
			string message = ex.Message;
			object[] args = new Exception[1] { ex };
			instance2.Log(error: true, "SetServiceData", message, args);
			throw new CommunicationMIPException("GetServiceData: Could not set service data", ex);
		}
		finally
		{
			if (serviceRegistration != null)
			{
				try
				{
					serviceRegistration.Dispose();
				}
				catch (Exception ex2)
				{
					EnvironmentManager instance3 = EnvironmentManager.Instance;
					string message2 = ex2.Message;
					object[] args = new Exception[1] { ex2 };
					instance3.Log(error: false, "SetServiceData", message2, args);
				}
			}
		}
	}

	public static void SetServiceData(ServerId serverId, Guid instance, string serviceData)
	{
		SetServiceData(serverId, instance, null, serviceData);
	}

	public static void FlushCache()
	{
		ServiceUtilCache.Flush();
	}

	public static void Close()
	{
	}
}
