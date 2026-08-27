using System;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.IdentityServer.Common.Exceptions;

namespace VideoOS.IdentityServer.Common;

public class IdpDiscoveryService : IIdpDiscoveryService
{
	private static readonly Lazy<IdpDiscoveryService> _instance = new Lazy<IdpDiscoveryService>(() => new IdpDiscoveryService());

	public static IdpDiscoveryService Instance => _instance.Value;

	public async Task<IdpInfo> ValidateIdpServer(Uri address, int timeout, CancellationToken cancellationToken)
	{
		IHttpClient httpClient = new HttpClientFactory().Create();
		httpClient.Timeout = TimeSpan.FromSeconds(timeout);
		return await ValidateIdpServer(httpClient, address, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private static async Task<IdpInfo> ValidateIdpServer(IHttpClient httpClient, Uri address, CancellationToken cancellationToken)
	{
		Uri wellKnownAddress = GetWellKnownAddress(address);
		try
		{
			IHttpResponseMessage httpResponseMessage = await httpClient.GetAsync(wellKnownAddress, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				switch ((int)httpResponseMessage.StatusCode)
				{
				case 401:
					throw new UnauthorizedException((int)httpResponseMessage.StatusCode, null);
				case 403:
					throw new ForbiddenException(null);
				case 404:
					throw new NotFoundException(null);
				case 500:
					throw new InternalServerErrorException(null);
				default:
					throw new NotFoundException(null);
				}
			}
			IdpOpenConfiguration idpConfiguration = JsonConvert.DeserializeObject<IdpOpenConfiguration>(await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
			return new IdpInfo
			{
				IdpConfiguration = idpConfiguration,
				Address = address
			};
		}
		catch (Exception ex) when (ex.GetType().Equals(typeof(TaskCanceledException)))
		{
			throw new TimeoutException();
		}
		catch (Exception ex2) when (((Func<bool>)delegate
		{
			// Could not convert BlockContainer to single expression
			Exception innerException = ex2.InnerException;
			return innerException != null && innerException.InnerException?.GetType().Equals(typeof(AuthenticationException)) == true;
		}).Invoke())
		{
			throw ex2.InnerException.InnerException;
		}
		catch (Exception ex3) when (((Func<bool>)delegate
		{
			// Could not convert BlockContainer to single expression
			Exception innerException2 = ex3.InnerException;
			return innerException2 != null && innerException2.InnerException?.GetType().Equals(typeof(SocketException)) == true;
		}).Invoke())
		{
			throw new NotFoundException(null);
		}
	}

	public async Task<IdpInfo> ValidateFasterIdpServerOnBothSchemas(Uri address, int timeout, CancellationToken cancellationToken, int extraTimeoutForHttps = 0, int defaultPortHttps = 443)
	{
		(Uri, Uri) tuple = MapToAddresses(address, defaultPortHttps);
		Uri HttpAddress = tuple.Item1;
		Uri HttpsAddress = tuple.Item2;
		IdpInfo httpIdpInfo = null;
		IdpInfo httpsIdpInfo = null;
		IHttpClient httpClient = new HttpClientFactory().Create();
		httpClient.Timeout = TimeSpan.FromSeconds(timeout);
		CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		try
		{
			Task[] tasks = new Task[2]
			{
				Task.Run(async delegate
				{
					_ = 1;
					try
					{
						httpIdpInfo = await ValidateIdpServer(httpClient, HttpAddress, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
						if (!cts.IsCancellationRequested && extraTimeoutForHttps > 0)
						{
							await Task.Delay(extraTimeoutForHttps, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
							cts.Cancel();
						}
					}
					catch
					{
					}
				}, cts.Token),
				Task.Run(async delegate
				{
					try
					{
						httpsIdpInfo = await ValidateIdpServer(httpClient, HttpsAddress, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
						if (!cts.IsCancellationRequested)
						{
							cts.Cancel();
						}
					}
					catch
					{
					}
				}, cts.Token)
			};
			try
			{
				await Task.WhenAll(tasks).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch
			{
			}
			return (httpsIdpInfo != null) ? httpsIdpInfo : httpIdpInfo;
		}
		finally
		{
			if (cts != null)
			{
				((IDisposable)cts).Dispose();
			}
		}
	}

	public async Task<(IdpInfo IdpInfo, Exception HttpError, Exception HttpsError)> ValidateFasterIdpServerOnBothSchemasWithErrors(Uri address, int timeout, CancellationToken cancellationToken, int extraTimeoutForHttps = 0, int defaultPortHttps = 443)
	{
		(Uri, Uri) tuple = MapToAddresses(address, defaultPortHttps);
		Uri HttpAddress = tuple.Item1;
		Uri HttpsAddress = tuple.Item2;
		IdpInfo httpIdpInfo = null;
		IdpInfo httpsIdpInfo = null;
		Exception httpException = null;
		Exception httpsException = null;
		IHttpClient httpClient = new HttpClientFactory().Create();
		httpClient.Timeout = TimeSpan.FromSeconds(timeout);
		CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		try
		{
			Task[] tasks = new Task[2]
			{
				Task.Run(async delegate
				{
					_ = 1;
					try
					{
						httpIdpInfo = await ValidateIdpServer(httpClient, HttpAddress, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
						if (!cts.IsCancellationRequested && extraTimeoutForHttps > 0)
						{
							await Task.Delay(extraTimeoutForHttps, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
							cts.Cancel();
						}
					}
					catch (Exception ex)
					{
						httpException = ex;
					}
				}, cts.Token),
				Task.Run(async delegate
				{
					try
					{
						httpsIdpInfo = await ValidateIdpServer(httpClient, HttpsAddress, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
						if (!cts.IsCancellationRequested)
						{
							cts.Cancel();
						}
					}
					catch (Exception ex)
					{
						httpsException = ex;
					}
				}, cts.Token)
			};
			try
			{
				await Task.WhenAll(tasks).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch
			{
			}
			return (IdpInfo: httpsIdpInfo ?? httpIdpInfo, HttpError: httpException, HttpsError: httpsException);
		}
		finally
		{
			if (cts != null)
			{
				((IDisposable)cts).Dispose();
			}
		}
	}

	private static (Uri HttpAddress, Uri HttpsAddress) MapToAddresses(Uri address, int defaultHttpsPort)
	{
		UriBuilder uriBuilder = new UriBuilder(address);
		bool flag = address.Port == 80 || address.Port == 443;
		uriBuilder.Scheme = Uri.UriSchemeHttp;
		uriBuilder.Port = (flag ? 80 : address.Port);
		Uri uri = uriBuilder.Uri;
		uriBuilder.Scheme = Uri.UriSchemeHttps;
		uriBuilder.Port = (flag ? 443 : address.Port);
		Uri uri2 = uriBuilder.Uri;
		return (HttpAddress: uri, HttpsAddress: uri2);
	}

	private static Uri GetWellKnownAddress(Uri address)
	{
		string text = address.ToString();
		text = text.TrimEnd('/');
		if (!text.EndsWith("idp", StringComparison.OrdinalIgnoreCase))
		{
			text += "/idp";
		}
		text += "/.well-known/openid-configuration";
		return new Uri(text);
	}
}
