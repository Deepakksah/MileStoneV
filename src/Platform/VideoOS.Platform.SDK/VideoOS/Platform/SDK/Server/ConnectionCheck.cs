#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Model.Responses;
using VideoOS.Platform.Login;
using VideoOS.Platform.SDK.Platform.Exceptions;
using VideoOS.Platform.SDK.Util;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.SDK.Server;

public class ConnectionCheck
{
	private static readonly string CommandServicePath = "ManagementServer/ServerCommandService.svc";

	public static readonly int DefaultTimeout = 5;

	private readonly IDiscoveryClientFactory _discoveryClientFactory = new DiscoveryClientFactory();

	public static int Timeout
	{
		get
		{
			int result = DefaultTimeout;
			if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentOptions.TryGetValue("ConnectionCheckTimeout", out var value))
			{
				int.TryParse(value, out result);
			}
			return result;
		}
	}

	public ConnectionCheck()
	{
	}

	public ConnectionCheck(IDiscoveryClientFactory discoveryClientFactory)
	{
		_discoveryClientFactory = discoveryClientFactory;
	}

	public static bool IsLegacyServerOnline(string address, out bool https, CredentialCache credentialCache, out bool unauthorized)
	{
		bool flag = false;
		https = false;
		long ticks = DateTime.Now.Ticks;
		ServerCommandServiceClient serverCommandServiceClient = null;
		bool flag2 = false;
		unauthorized = false;
		try
		{
			Uri uri = new UriBuilder(UriUtil.EnsureHttpPrefix(address, Uri.UriSchemeHttp)).Uri;
			string text = "";
			Uri uri2 = (uri.IsDefaultPort ? new UriBuilder("http", uri.Host)
			{
				Path = CommandServicePath
			}.Uri : new UriBuilder("http", uri.Host, uri.Port)
			{
				Path = CommandServicePath
			}.Uri);
			Uri uri3 = (uri.IsDefaultPort ? new UriBuilder("https", uri.Host)
			{
				Path = CommandServicePath
			}.Uri : new UriBuilder("https", uri.Host, uri.Port)
			{
				Path = CommandServicePath
			}.Uri);
			NetworkCredential credential = credentialCache.GetCredential(uri2, "Negotiate");
			if (credential == null)
			{
				ServerTrace.Info("ConnectionCheck:IsLegacyServerOnline", $"Credential not found for AD user on URL: {uri2}");
				credential = credentialCache.GetCredential(uri2, "Basic");
				if (credential == null)
				{
					ServerTrace.Info("ConnectionCheck:IsLegacyServerOnline", $"Credential not found for BASIC user on URL: {uri2}");
					credential = credentialCache.GetCredential(uri, "Basic");
					if (credential == null && uri.IsDefaultPort)
					{
						ServerTrace.Info("ConnectionCheck:IsLegacyServerOnline", $"Credential not found for BASIC user on URL: {uri}");
						Uri uri4 = new UriBuilder("http", uri.Host, 443)
						{
							Path = CommandServicePath
						}.Uri;
						credential = credentialCache.GetCredential(uri4, "Basic");
					}
				}
				if (credential != null)
				{
					flag2 = true;
				}
			}
			if (credential != null)
			{
				string text2 = (flag2 ? "BASIC" : "AD");
				Uri uri5 = (flag2 ? uri3 : uri2);
				ServerTrace.Info("ConnectionCheck:IsLegacyServerOnline", "Credential found for " + text2 + " user");
				serverCommandServiceClient = new ServerCommandServiceClient(GetBinding(flag2), new EndpointAddress(uri5, new SpnEndpointIdentity(flag2 ? "host/localhost" : SpnFactory.GetSpn(uri5))));
				ManagementServerWcfHelper.SetupCertificateValidation(serverCommandServiceClient);
				SetCredentials(serverCommandServiceClient.ClientCredentials, credential, flag2);
				serverCommandServiceClient.Open();
				Thread.Sleep(100);
				text = serverCommandServiceClient.GetServerVersion();
				flag = true;
				string[] array = text.Split('.');
				int result = 5;
				int result2 = 0;
				int.TryParse(array[0], out result);
				if (array.Length > 1)
				{
					int.TryParse(array[1], out result2);
				}
				if (result >= 6)
				{
					https = true;
				}
				LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(uri.Host, uri.Port);
				if (loginSettings != null)
				{
					loginSettings.ServerProductInfo.MajorVersion = result.ToString();
					loginSettings.ServerProductInfo.MinorVersion = result2.ToString();
					loginSettings.ServerProductInfo.ProductVersion = text;
				}
			}
			else
			{
				ServerTrace.Info("ConnectionCheck:IsLegacyServerOnline", "No credentials found for " + address);
			}
		}
		catch (WebException ex)
		{
			flag = false;
			if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response is HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized })
			{
				flag = true;
				unauthorized = true;
			}
		}
		catch (MessageSecurityException ex2)
		{
			https = flag2;
			flag = true;
			if (ex2.InnerException is WebException { Status: WebExceptionStatus.ProtocolError, Response: HttpWebResponse { StatusCode: HttpStatusCode.Unauthorized } })
			{
				unauthorized = true;
			}
		}
		catch (Exception)
		{
			flag = false;
		}
		finally
		{
			long num = DateTime.Now.Ticks - ticks;
			Trace.WriteLine("ConnectionCheck for:" + address + ", Took:" + num / 10000 + "ms");
			if (serverCommandServiceClient != null)
			{
				try
				{
					serverCommandServiceClient.Close();
				}
				catch (Exception)
				{
				}
			}
		}
		if (!flag)
		{
			EnvironmentManager.Instance.Log(error: false, "ConnectionCheck:IsLegacyServerOnline", "Could not establish connection to server: " + address);
		}
		return flag;
	}

	private static Binding GetBinding(bool isBasicUser)
	{
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
				ReaderQuotas = XmlDictionaryReaderQuotas.Max,
				OpenTimeout = TimeSpan.FromSeconds(Timeout),
				CloseTimeout = TimeSpan.FromSeconds(Timeout),
				SendTimeout = TimeSpan.FromSeconds(Timeout)
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
			ReaderQuotas = XmlDictionaryReaderQuotas.Max,
			OpenTimeout = TimeSpan.FromSeconds(Timeout),
			CloseTimeout = TimeSpan.FromSeconds(Timeout),
			SendTimeout = TimeSpan.FromSeconds(Timeout)
		};
	}

	private static void SetCredentials(ClientCredentials clientCredentials, NetworkCredential nc, bool isBasicUser)
	{
		if (isBasicUser)
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

	public async Task<bool> IsOAuthServerOnline(string address)
	{
		Uri idpUri = new Uri(new Uri(address), "idp");
		IDiscoveryClient discoveryClient = _discoveryClientFactory.Create(new List<Uri> { idpUri });
		using CancellationTokenSource source = new CancellationTokenSource(Timeout * 1000);
		DiscoveryResponse discoveryResponse;
		try
		{
			discoveryResponse = await discoveryClient.GetAsync(source.Token);
		}
		catch (OperationCanceledException)
		{
			source.Dispose();
			EnvironmentManager.Instance.Log(error: false, "ConnectionCheck:IsOAuthServerOnline", $"Connection check for {address} timed out after {Timeout} seconds. " + "Timeout can be changed by setting ConnectionCheckTimeout on EnvironmentOptions.");
			return false;
		}
		if (discoveryResponse.IsError)
		{
			if (discoveryResponse.ErrorResponses != null)
			{
				string arg = string.Join("\n", discoveryResponse.ErrorResponses.Select((DiscoveryResponseBase r) => r.Error));
				EnvironmentManager.Instance.Log(error: false, "ConnectionCheck:IsOAuthServerOnline", $"Error(s): {arg}");
				if (Environment.KeepLoginServerUriSchemeAllEnvironments && discoveryResponse.ErrorResponses.Any((DiscoveryResponseBase r) => r.StatusCode == HttpStatusCode.Forbidden))
				{
					throw new ForbiddenMIPException(discoveryResponse.ErrorResponses.First((DiscoveryResponseBase r) => r.StatusCode == HttpStatusCode.Forbidden).Error);
				}
			}
			return false;
		}
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(idpUri.Host, idpUri.Port);
		if (loginSettings != null)
		{
			loginSettings.ServerProductInfo.MajorVersion = discoveryResponse.ServerMajorVersion.ToString();
			loginSettings.ServerProductInfo.MinorVersion = discoveryResponse.ServerMinorVersion.ToString();
			loginSettings.ServerProductInfo.ProductVersion = discoveryResponse.ServerVersion;
		}
		return discoveryResponse.ServerMajorVersion >= 21;
	}
}
