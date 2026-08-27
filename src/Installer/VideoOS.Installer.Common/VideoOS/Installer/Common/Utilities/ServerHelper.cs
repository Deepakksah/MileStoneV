using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using Newtonsoft.Json;
using ServerCommandService;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.Installer.Common.Properties;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.Utilities;

public class ServerHelper
{
	public class ServerException : Exception
	{
		public ServerException(SecurityNegotiationException ex)
			: base(Resources.ErrorMessage_ServerAuthorizationFailed, ex)
		{
		}

		public ServerException(FaultException ex)
			: base(Resources.ErrorMessage_ServerAuthorizationFailed, ex)
		{
		}

		public ServerException(AuthenticationException ex)
			: base(Resources.ErrorMessage_CertificateNotTrusted, ex)
		{
		}

		public ServerException(HttpRequestException ex)
			: base(Resources.ErrorMessage_CouldNotGetToken, (Exception)(object)ex)
		{
		}

		public ServerException(string message)
			: base(message)
		{
		}
	}

	private readonly IWebRequestFactory _webRequestFactory;

	private readonly IHttpClientFactory _httpClientFactory;

	private bool _certificateError;

	public ServerHelper()
		: this(new WebRequestFactory(), new HttpClientFactory())
	{
	}

	public ServerHelper(IWebRequestFactory webRequestFactory, IHttpClientFactory httpClientFactory)
	{
		_webRequestFactory = webRequestFactory;
		_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException("httpClientFactory");
	}

	public static bool TryLoginToCorporate(Uri managementServerAddress, Uri identityServerAddress)
	{
		return TryLoginToCorporate(managementServerAddress, identityServerAddress, (NetworkCredential)CredentialCache.DefaultCredentials);
	}

	public static ICredentials ResolveCredentials(string userName, string password)
	{
		if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
		{
			return (NetworkCredential)CredentialCache.DefaultCredentials;
		}
		string[] array = userName.Split('\\');
		return new NetworkCredential(array[1], password, array[0]);
	}

	public static bool TryLoginToCorporate(Uri managementServerAddress, Uri identityServerAddress, ICredentials credentials)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_0096: Expected O, but got Unknown
		if (managementServerAddress == null)
		{
			throw new ArgumentNullException("managementServerAddress");
		}
		if (identityServerAddress == null)
		{
			throw new ArgumentNullException("identityServerAddress");
		}
		LogManager.Write("Try login to " + managementServerAddress.ToString());
		if (!identityServerAddress.AbsoluteUri.EndsWith("/", StringComparison.OrdinalIgnoreCase))
		{
			identityServerAddress = new Uri(identityServerAddress.AbsoluteUri + "/");
		}
		LogManager.Write("Getting token from " + identityServerAddress.ToString());
		AccessTokenResponse result;
		try
		{
			result = IdentityServerClient.GetTokenAsync(identityServerAddress, credentials).Result;
		}
		catch (HttpRequestException ex)
		{
			LogManager.Write((Exception)ex);
			throw new ServerException(ex);
		}
		if (result != null)
		{
			LogManager.Write("Got access token...");
			using ServerCommandServiceClient serverCommandServiceClient = ServerCommandServiceClientHelper.CreateClient(managementServerAddress, result.AccessToken);
			try
			{
				serverCommandServiceClient.Login(Guid.NewGuid(), result.AccessToken);
				LogManager.Write("Login success");
				return true;
			}
			catch (SecurityNegotiationException ex2)
			{
				LogManager.Write(ex2);
				throw new ServerException(ex2);
			}
			catch (FaultException ex3) when (ex3.Message.ToUpperInvariant().Contains("VMO61008"))
			{
				LogManager.Write(ex3);
				throw new ServerException(ex3);
			}
			catch (EndpointNotFoundException)
			{
				return false;
			}
			catch (CommunicationException ex5)
			{
				LogManager.Write(ex5);
				if (ex5.InnerException != null && ex5.InnerException is VideoOS.Common.EnvironmentServices.Web.WebException && ex5.InnerException.InnerException != null && ex5.InnerException.InnerException is AuthenticationException)
				{
					throw new ServerException((AuthenticationException)ex5.InnerException.InnerException);
				}
				throw;
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
				throw;
			}
		}
		LogManager.Write("Access token is null, login failed");
		return false;
	}

	public bool TryGetManagementServerInfo(Uri managementServerAddress, out ManagementServerInfo managementServerInfo)
	{
		managementServerInfo = null;
		string text = null;
		_certificateError = false;
		if (managementServerAddress == null)
		{
			throw new ArgumentNullException("managementServerAddress");
		}
		using (IHttpClient httpClient = _httpClientFactory.Create((Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>)CustomValidator))
		{
			Uri uri = new UriBuilder(managementServerAddress)
			{
				Path = "/managementserver/.Well-known/uris"
			}.Uri;
			try
			{
				using IHttpResponseMessage httpResponseMessage = httpClient.GetAsync(uri).Result;
				if (httpResponseMessage.IsSuccessStatusCode)
				{
					if (_certificateError)
					{
						throw new ServerException(new AuthenticationException("Certificate is not trusted"));
					}
					text = httpResponseMessage.Content.ReadAsStringAsync().Result;
				}
				else
				{
					LogManager.Write(FormattableString.Invariant($"Getting Management Server Info failed, response was Status code = {httpResponseMessage.StatusCode}"), TraceEventType.Warning);
				}
			}
			catch (AggregateException)
			{
			}
			catch (ServerException)
			{
				throw;
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
				return false;
			}
		}
		if (text == null)
		{
			if (VerifyLegacyConnection(managementServerAddress))
			{
				Uri httpsAddress;
				Uri httpAddress;
				if (string.Compare(managementServerAddress.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) == 0)
				{
					httpsAddress = managementServerAddress;
					httpAddress = new UriBuilder(Uri.UriSchemeHttp, managementServerAddress.Host).Uri;
				}
				else
				{
					httpAddress = managementServerAddress;
					httpsAddress = new UriBuilder(Uri.UriSchemeHttps, managementServerAddress.Host).Uri;
				}
				Uri uri2 = new UriBuilder(managementServerAddress)
				{
					Path = "IDP"
				}.Uri;
				managementServerInfo = new ManagementServerInfo(uri2, httpAddress, httpsAddress);
				return true;
			}
			return false;
		}
		try
		{
			UrisData urisData = JsonConvert.DeserializeObject<UrisData>(text);
			managementServerInfo = new ManagementServerInfo(new Uri(urisData.IdentityProvider), new Uri(urisData.UnsecureManagementServer), new Uri(urisData.SecureManagementServer));
			return true;
		}
		catch (Exception exception2)
		{
			LogManager.Write(exception2);
			return false;
		}
	}

	private bool CustomValidator(HttpRequestMessage requestMessage, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslErrors)
	{
		_certificateError = sslErrors != SslPolicyErrors.None;
		return true;
	}

	private bool VerifyLegacyConnection(Uri newManagementServerAddress)
	{
		UriBuilder uriBuilder = new UriBuilder(newManagementServerAddress)
		{
			Path = "/ManagementServer/ServiceRegistrationService.svc"
		};
		IWebResponse webResponse = null;
		try
		{
			IWebRequest webRequest = _webRequestFactory.Create(uriBuilder.Uri);
			webRequest.UseDefaultCredentials = true;
			using (webResponse = webRequest.GetResponse())
			{
				return CheckResponse(webResponse);
			}
		}
		catch (VideoOS.Common.EnvironmentServices.Web.WebException ex)
		{
			webResponse = ex.Response;
		}
		return CheckResponse(webResponse);
	}

	private static bool CheckResponse(IWebResponse response)
	{
		if (response is IHttpWebResponse httpWebResponse)
		{
			if (httpWebResponse.StatusCode == HttpStatusCode.OK || httpWebResponse.StatusCode == HttpStatusCode.Forbidden || httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
			{
				return true;
			}
			LogManager.Write(FormattableString.Invariant($"Response from Management Server indicates it is not present, response was Status code = {httpWebResponse.StatusCode}"), TraceEventType.Information);
		}
		return false;
	}
}
