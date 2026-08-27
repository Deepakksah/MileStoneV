using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Xml;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.Svc;

namespace VideoOS.Platform.Proxy.ConfigApi;

public class ConfigurationClientDecorator : IConfigurationClientDecorator, IDisposable
{
	private ConfigurationServiceClient _configurationServiceClient;

	private LoginSettings _loginSettings;

	private object _lockOpenCall = new object();

	public ConfigurationClientDecorator(LoginSettings loginSettings)
	{
		if (loginSettings == null)
		{
			throw new ArgumentNullException("loginSettings");
		}
		_loginSettings = loginSettings;
	}

	public void UpdateLoginSettingsIfNoToken(LoginSettings loginSettings)
	{
		if (string.IsNullOrEmpty(_loginSettings.Token) && _loginSettings != loginSettings)
		{
			_loginSettings = loginSettings;
		}
	}

	public ConfigurationItem GetItem(string path)
	{
		if (Factory.TraceCommunication)
		{
			Factory.LogBefore("GetItem", path);
		}
		ConfigurationItem configurationItem = CallWithRetry((ConfigurationServiceClient client) => client.GetItem(path));
		if (Factory.TraceCommunication)
		{
			Factory.LogAfter("GetItem", configurationItem);
		}
		return configurationItem;
	}

	public ConfigurationItem[] GetItems(string itemType)
	{
		if (Factory.TraceCommunication)
		{
			Factory.LogBefore("GetItems", itemType);
		}
		ConfigurationItem[] array = CallWithRetry((ConfigurationServiceClient client) => client.GetItems(itemType));
		if (Factory.TraceCommunication)
		{
			Factory.LogAfter("GetItems", array);
		}
		return array;
	}

	public ValidateResult SetItem(ConfigurationItem item)
	{
		if (Factory.TraceCommunication)
		{
			Factory.LogBefore("SetItem", item);
		}
		return CallWithRetry((ConfigurationServiceClient client) => client.SetItem(item));
	}

	public bool HasChildItems(string path)
	{
		return CallWithRetry((ConfigurationServiceClient client) => client.HasChildItems(path));
	}

	public ConfigurationItem[] GetChildItems(string path)
	{
		if (Factory.TraceCommunication)
		{
			Factory.LogBefore("GetChildItems", path);
		}
		ConfigurationItem[] array = CallWithRetry((ConfigurationServiceClient client) => client.GetChildItems(path));
		if (Factory.TraceCommunication)
		{
			Factory.LogAfter("GetChildItems", array);
		}
		return array;
	}

	public ConfigurationItem[] GetChildItemsHierarchy(string path, string[] itemTypes, ItemFilter[] filters)
	{
		if (Factory.TraceCommunication)
		{
			Factory.LogBefore("GetChildItemsHierarchy", path);
		}
		ConfigurationItem[] array = CallWithRetry((ConfigurationServiceClient client) => client.GetChildItemsHierarchy(path, itemTypes, filters));
		if (Factory.TraceCommunication)
		{
			Factory.LogAfter("GetChildItemsHierarchy", array);
		}
		return array;
	}

	public ConfigurationItem[] QueryItems(ItemFilter filter, int maxResults)
	{
		if (Factory.TraceCommunication)
		{
			Factory.LogBefore("QueryItems", "");
		}
		ConfigurationItem[] array = CallWithRetry((ConfigurationServiceClient client) => client.QueryItems(filter, maxResults));
		if (Factory.TraceCommunication)
		{
			Factory.LogAfter("QueryItems", array);
		}
		return array;
	}

	public ConfigurationItem ConstructItem(string itemType, Property[] properties)
	{
		return CallWithRetry((ConfigurationServiceClient client) => client.ConstructItem(itemType, properties));
	}

	public ConfigurationItem ConstructChildItem(string itemType, string parentPath)
	{
		return CallWithRetry((ConfigurationServiceClient client) => client.ConstructChildItem(itemType, parentPath));
	}

	public ValidateResult ValidateItem(ConfigurationItem item)
	{
		return CallWithRetry((ConfigurationServiceClient client) => client.ValidateItem(item));
	}

	public void DeleteItem(string path)
	{
		CallWithRetry(delegate(ConfigurationServiceClient client)
		{
			client.DeleteItem(path);
		});
	}

	public MethodInfo[] GetMethodInfos()
	{
		return CallWithRetry((ConfigurationServiceClient client) => client.GetMethodInfos());
	}

	public MethodInfo GetMethodInfo(string methodId)
	{
		return CallWithRetry((ConfigurationServiceClient client) => client.GetMethodInfo(methodId));
	}

	public ConfigurationItem InvokeMethod(ConfigurationItem item, string method)
	{
		return CallWithRetry((ConfigurationServiceClient client) => client.InvokeMethod(item, method));
	}

	public Dictionary<string, string> GetTranslations(string languageId)
	{
		return CallWithRetry((ConfigurationServiceClient client) => client.GetTranslations(languageId));
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing && _configurationServiceClient != null)
		{
			_configurationServiceClient.Close();
			_configurationServiceClient = null;
		}
	}

	private void CallWithRetry(Action<ConfigurationServiceClient> call)
	{
		try
		{
			if (_configurationServiceClient == null)
			{
				Connect();
			}
			call(_configurationServiceClient);
		}
		catch (FaultException)
		{
			throw;
		}
		catch (CommunicationException ex2)
		{
			lock (_lockOpenCall)
			{
				if (IsUnauthorizedLogin(ex2))
				{
					CloseConnection();
					throw;
				}
				CloseConnection();
				Connect();
				call(_configurationServiceClient);
			}
		}
		catch (ObjectDisposedException)
		{
			lock (_lockOpenCall)
			{
				CloseConnection();
				Connect();
				call(_configurationServiceClient);
			}
		}
	}

	private static bool IsUnauthorizedLogin(CommunicationException ex)
	{
		if (ex?.InnerException?.InnerException is WebException ex2)
		{
			HttpWebResponse obj = ex2.Response as HttpWebResponse;
			if (obj == null)
			{
				return false;
			}
			return obj.StatusCode == HttpStatusCode.Unauthorized;
		}
		return false;
	}

	private TResult CallWithRetry<TResult>(Func<ConfigurationServiceClient, TResult> call)
	{
		try
		{
			if (_configurationServiceClient == null)
			{
				Connect();
			}
			return call(_configurationServiceClient);
		}
		catch (FaultException)
		{
			throw;
		}
		catch (CommunicationException ex2)
		{
			lock (_lockOpenCall)
			{
				if (IsUnauthorizedLogin(ex2))
				{
					CloseConnection();
					throw;
				}
				CloseConnection();
				Connect();
				return call(_configurationServiceClient);
			}
		}
		catch (ObjectDisposedException)
		{
			lock (_lockOpenCall)
			{
				CloseConnection();
				Connect();
				return call(_configurationServiceClient);
			}
		}
	}

	private void Connect()
	{
		lock (_lockOpenCall)
		{
			if (_configurationServiceClient != null)
			{
				return;
			}
			if (_loginSettings.IdentityTokenCache != null)
			{
				Uri uri = new UriBuilder(_loginSettings.Uri.Scheme, _loginSettings.Uri.DnsSafeHost, _loginSettings.Uri.Port, "/ManagementServer/ConfigurationApiServiceOAuth.svc").Uri;
				Binding oAuthBinding = ServiceApiSvcHelper.GetOAuthBinding(uri.Scheme == Uri.UriSchemeHttps);
				EndpointAddress remoteAddress = new EndpointAddress(uri, new SpnEndpointIdentity(SpnFactory.GetSpn(uri)));
				ConfigurationServiceClient configurationServiceClient = new ConfigurationServiceClient(oAuthBinding, remoteAddress);
				configurationServiceClient.Endpoint.EndpointBehaviors.Add(new ServiceUtil.AddTokenBehavior(_loginSettings));
				ServiceApiSvcHelper.ConfigureEndpoint(configurationServiceClient.Endpoint);
				OpenConfigurationServiceClient(configurationServiceClient, uri.ToString());
				return;
			}
			bool isBasicUser = _loginSettings.IsBasicUser;
			string arg = _loginSettings.Uri.Authority;
			string text;
			if (isBasicUser)
			{
				if (_loginSettings.Uri.Port == 80)
				{
					arg = _loginSettings.Uri.Host;
				}
				text = $"https://{arg}/ManagementServer/ConfigurationApiService.svc";
			}
			else
			{
				text = $"http://{arg}/ManagementServer/ConfigurationApiService.svc";
			}
			Uri uri2 = new UriBuilder(text).Uri;
			Binding binding = GetBinding(isBasicUser, isCorporate: true);
			string spn = SpnFactory.GetSpn(uri2);
			EndpointAddress remoteAddress2 = new EndpointAddress(uri2, new SpnEndpointIdentity(spn));
			ConfigurationServiceClient configurationServiceClient2 = new ConfigurationServiceClient(binding, remoteAddress2);
			if (isBasicUser)
			{
				configurationServiceClient2.ClientCredentials.UserName.UserName = "[BASIC]\\" + _loginSettings.NetworkCredential.UserName;
				configurationServiceClient2.ClientCredentials.UserName.Password = _loginSettings.NetworkCredential.Password;
				configurationServiceClient2.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
				{
					CertificateValidationMode = X509CertificateValidationMode.None
				};
			}
			else
			{
				configurationServiceClient2.ClientCredentials.Windows.ClientCredential = _loginSettings.NetworkCredential;
			}
			OpenConfigurationServiceClient(configurationServiceClient2, text);
		}
	}

	private void OpenConfigurationServiceClient(ConfigurationServiceClient configurationServiceClient, string uriString)
	{
		configurationServiceClient.InnerChannel.Open();
		Thread.Sleep(10);
		DateTime dateTime = DateTime.Now.AddSeconds(1.0);
		while (DateTime.Now < dateTime && configurationServiceClient.State != CommunicationState.Opened)
		{
			Thread.Sleep(5);
		}
		if (configurationServiceClient.State != CommunicationState.Opened)
		{
			Factory.Log("Service Constructor", "ConfigurationServiceClientDecorator: Soap client not open after 1 seconds?");
		}
		_configurationServiceClient = configurationServiceClient;
		if (Factory.TraceCommunication)
		{
			Factory.Log("Service Constructor", "Username:" + _loginSettings.Name + ", URL=" + uriString);
		}
	}

	public void CloseConnection()
	{
		lock (_lockOpenCall)
		{
			try
			{
				if (_configurationServiceClient != null)
				{
					_configurationServiceClient.Close();
				}
			}
			catch
			{
				if (_configurationServiceClient != null)
				{
					_configurationServiceClient.Abort();
				}
			}
			_configurationServiceClient = null;
		}
	}

	private static Binding GetBinding(bool IsBasic, bool isCorporate)
	{
		if (!IsBasic)
		{
			WSHttpBinding wSHttpBinding = new WSHttpBinding();
			wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			wSHttpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
			wSHttpBinding.MaxReceivedMessageSize = 2147483647L;
			wSHttpBinding.MaxBufferPoolSize = 2147483647L;
			wSHttpBinding.ReaderQuotas = XmlDictionaryReaderQuotas.Max;
			TimeSpan receiveTimeout = (wSHttpBinding.CloseTimeout = (wSHttpBinding.OpenTimeout = (wSHttpBinding.SendTimeout = TimeSpan.FromMinutes(10.0))));
			wSHttpBinding.ReceiveTimeout = receiveTimeout;
			return wSHttpBinding;
		}
		BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
		basicHttpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
		basicHttpBinding.MaxReceivedMessageSize = 2147483647L;
		basicHttpBinding.MaxBufferSize = int.MaxValue;
		basicHttpBinding.MaxBufferPoolSize = 2147483647L;
		basicHttpBinding.TextEncoding = Encoding.UTF8;
		basicHttpBinding.UseDefaultWebProxy = true;
		basicHttpBinding.AllowCookies = false;
		basicHttpBinding.Namespace = "VideoOS.ConfigurationAPI";
		if (isCorporate)
		{
			basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
			basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
		}
		else
		{
			basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
		}
		TimeSpan receiveTimeout2 = (basicHttpBinding.CloseTimeout = (basicHttpBinding.OpenTimeout = (basicHttpBinding.SendTimeout = TimeSpan.FromMinutes(10.0))));
		basicHttpBinding.ReceiveTimeout = receiveTimeout2;
		return basicHttpBinding;
	}
}
