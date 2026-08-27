using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VideoOS.IdentityServer.Common.Utilities;
using VideoOS.IdentityServer.Common.Utilities.ServerValidation;

namespace VideoOS.IdentityServer.Common;

public class IdpServerValidation : IIdpServerValidation
{
	private class VerifyEndpointSupportsSslThread : AsyncThread<SslEndpointVerificationResult>
	{
		private const string ThreadName = "SSL Verification";

		private readonly ISslVerification _sslVerification;

		private readonly Uri _uriToValidate;

		public VerifyEndpointSupportsSslThread(ISslVerification sslVerification, Uri uriToValidate)
			: base(ThreadPriority.AboveNormal, "SSL Verification")
		{
			_sslVerification = sslVerification;
			_uriToValidate = uriToValidate;
		}

		public override Task<SslEndpointVerificationResult> Run()
		{
			return Run(CancellationToken.None);
		}

		public override Task<SslEndpointVerificationResult> Run(CancellationToken cancellationToken)
		{
			try
			{
				return Run(() => _sslVerification.VerifyEndpointSupportsSsl(_uriToValidate), cancellationToken);
			}
			catch (AggregateException ex)
			{
				ex.Handle((Exception h) => true);
				return Task.FromResult(SslEndpointVerificationResult.UnknownResponse);
			}
		}
	}

	private const string WellKnownConfigPath = "idp/.well-known/openid-configuration";

	private const int DefaultSixtySecondTimeout = 60;

	private const int UseDefaultPortForScheme = -1;

	private readonly IHttpGet _httpGet;

	private readonly int _timeoutInMilliSeconds;

	private readonly IdpDiscoveryType _idpDiscoveryType;

	private ConcurrentDictionary<string, SslEndpointVerificationResult> _sslVerificationResults;

	public ISslVerification SslVerification { get; }

	public bool ContinueOnSslVerificationTimeout { get; set; }

	public IdpServerValidation()
		: this(new HttpGet())
	{
	}

	public IdpServerValidation(IHttpGet httpGet)
		: this(httpGet, 60, IdpDiscoveryType.Both, new SslVerification())
	{
	}

	public IdpServerValidation(IHttpGet httpGet, int timeoutInSeconds)
		: this(httpGet, timeoutInSeconds, IdpDiscoveryType.Both, new SslVerification(timeoutInSeconds))
	{
	}

	public IdpServerValidation(IHttpGet httpGet, IdpDiscoveryType idpDiscoveryType)
		: this(httpGet, 60, idpDiscoveryType, new SslVerification())
	{
	}

	public IdpServerValidation(IHttpGet httpGet, int timeoutInSeconds, IdpDiscoveryType idpDiscoveryType)
		: this(httpGet, timeoutInSeconds, idpDiscoveryType, new SslVerification(timeoutInSeconds))
	{
	}

	public IdpServerValidation(IHttpGet httpGet, int timeoutInSeconds, IdpDiscoveryType idpDiscoveryType, ISslVerification sslVerificationWrapper)
	{
		ContinueOnSslVerificationTimeout = true;
		SslVerification = sslVerificationWrapper;
		_httpGet = httpGet ?? throw new ArgumentNullException("httpGet");
		_timeoutInMilliSeconds = timeoutInSeconds * 1000;
		_idpDiscoveryType = idpDiscoveryType;
		_sslVerificationResults = new ConcurrentDictionary<string, SslEndpointVerificationResult>();
	}

	public Task<IIdpServerInformation> ValidateIdpServerAsync(Uri serverUri)
	{
		return ValidateIdpServerAsync(serverUri, CancellationToken.None);
	}

	public Task<IIdpServerInformation> ValidateIdpServerAsync(Uri serverUri, CancellationToken cancellationToken)
	{
		return ValidateIdpServerWithCancellationAsync(serverUri, cancellationToken);
	}

	public Task<List<IIdpServerInformation>> ValidateIdpServerAsync(Uri[] serverUriList)
	{
		return ValidateIdpServerAsync(serverUriList, CancellationToken.None);
	}

	public async Task<List<IIdpServerInformation>> ValidateIdpServerAsync(Uri[] serverUriList, CancellationToken cancellationToken)
	{
		List<Uri> source = RetrieveUniqueValues(serverUriList);
		List<IIdpServerInformation> result = new List<IIdpServerInformation>();
		await Task.WhenAll(((IEnumerable<Uri>)source).Select((Func<Uri, Task>)async delegate(Uri uri)
		{
			IIdpServerInformation item = await ValidateIdpServerWithCancellationAsync(uri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			lock (result)
			{
				result.Add(item);
			}
		})).ConfigureAwait(continueOnCapturedContext: false);
		return result;
	}

	public async Task ValidateIdpServerAsync(Uri[] serverUriList, Func<IIdpServerInformation, bool> idpServerDelegate)
	{
		if (idpServerDelegate == null)
		{
			throw new ArgumentNullException("idpServerDelegate");
		}
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		try
		{
			_httpGet.CancelToken = cancellationTokenSource.Token;
			try
			{
				await Task.WhenAll(((IEnumerable<Uri>)RetrieveUniqueValues(serverUriList)).Select((Func<Uri, Task>)async delegate(Uri uri)
				{
					IIdpServerInformation arg = await ValidateIdpServerWithCancellationAsync(uri, cancellationTokenSource.Token).ConfigureAwait(continueOnCapturedContext: false);
					cancellationTokenSource.Token.ThrowIfCancellationRequested();
					if (idpServerDelegate(arg))
					{
						cancellationTokenSource.Cancel();
					}
				})).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (OperationCanceledException)
			{
			}
		}
		finally
		{
			if (cancellationTokenSource != null)
			{
				((IDisposable)cancellationTokenSource).Dispose();
			}
		}
		idpServerDelegate(null);
	}

	private async Task<IIdpServerInformation> ValidateIdpServerWithCancellationAsync(Uri serverUri, CancellationToken cancellationToken)
	{
		if (serverUri == null)
		{
			throw new ArgumentNullException("serverUri");
		}
		if (!serverUri.IsAbsoluteUri)
		{
			throw new ArgumentException(FormattableString.Invariant($"Must be absolute Uri"), "serverUri");
		}
		IdpServerInformation result = new IdpServerInformation(_idpDiscoveryType, serverUri);
		if (!serverUri.HasValidScheme())
		{
			serverUri = new Uri(Uri.UriSchemeHttps + "://" + serverUri.ToString());
		}
		await ValidateCertificateAgainstIdpServiceAsync(serverUri, result, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await ValidUnsecureEndpointExistsAgainstIdpServiceAsync(serverUri, result, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		return result;
	}

	private static List<Uri> RetrieveUniqueValues(Uri[] serverUriList)
	{
		if (serverUriList == null)
		{
			throw new ArgumentNullException("serverUriList");
		}
		List<string> list = new List<string>();
		List<Uri> list2 = new List<Uri>();
		foreach (Uri uri in serverUriList)
		{
			if (!(uri == null))
			{
				Uri uri2 = uri;
				if (!uri2.HasValidScheme())
				{
					string text = RemoveScheme(uri2);
					uri2 = new Uri(Uri.UriSchemeHttps + "://" + text);
				}
				string item = RemoveScheme(uri2);
				if (!list.Contains(item))
				{
					list2.Add(uri2);
					list.Add(item);
				}
			}
		}
		return list2;
	}

	private static string RemoveScheme(Uri uri)
	{
		string text = uri.ToString();
		int num = text.IndexOf("://", StringComparison.Ordinal);
		if (num > -1)
		{
			text = text.Substring(num + 3);
		}
		return text;
	}

	private async Task ValidateCertificateAgainstIdpServiceAsync(Uri serverUri, IdpServerInformation idpServerInformation, CancellationToken cancellationToken)
	{
		if (_idpDiscoveryType == IdpDiscoveryType.Unsecure)
		{
			return;
		}
		idpServerInformation.SecureIdpValidated = true;
		UriBuilder secureIdp = new UriBuilder(serverUri)
		{
			Scheme = Uri.UriSchemeHttps
		};
		if (serverUri.IsDefaultPort)
		{
			secureIdp.Port = -1;
		}
		else
		{
			idpServerInformation.ContainsCustomPort = true;
			if (!(await ValidateNonStandardPortAsync(idpServerInformation, secureIdp.Uri, isSecure: true, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)))
			{
				return;
			}
		}
		try
		{
			IHttpGetResult httpGetResult = await _httpGet.GetResponseAsync(new Uri(secureIdp.Uri, "idp/.well-known/openid-configuration"), _timeoutInMilliSeconds).ConfigureAwait(continueOnCapturedContext: false);
			idpServerInformation.IdpConfiguration = JsonConvert.DeserializeObject<IdpOpenConfiguration>(httpGetResult.Response);
			idpServerInformation.ValidCertificate = httpGetResult.IsValidCertificate;
			idpServerInformation.SecureIdpAvailable = httpGetResult != null;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception)
		{
			idpServerInformation.SecureIdpAvailable = false;
		}
		if (idpServerInformation.SecureIdpAvailable)
		{
			idpServerInformation.ValidatedServerUri = secureIdp.Uri;
		}
	}

	private async Task ValidUnsecureEndpointExistsAgainstIdpServiceAsync(Uri serverUri, IdpServerInformation idpServerInformation, CancellationToken cancellationToken)
	{
		if (_idpDiscoveryType == IdpDiscoveryType.Secure)
		{
			return;
		}
		idpServerInformation.UnsecureIdpValidated = true;
		UriBuilder secureIdp = new UriBuilder(serverUri)
		{
			Scheme = Uri.UriSchemeHttp
		};
		if (serverUri.IsDefaultPort)
		{
			secureIdp.Port = -1;
		}
		else
		{
			idpServerInformation.ContainsCustomPort = true;
			if (!(await ValidateNonStandardPortAsync(idpServerInformation, secureIdp.Uri, isSecure: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)))
			{
				return;
			}
		}
		try
		{
			IHttpGetResult httpGetResult = await _httpGet.GetResponseAsync(new Uri(secureIdp.Uri, "idp/.well-known/openid-configuration"), _timeoutInMilliSeconds).ConfigureAwait(continueOnCapturedContext: false);
			_ = httpGetResult?.Response;
			if (idpServerInformation.IdpConfiguration == null && httpGetResult != null && httpGetResult.Response != null)
			{
				idpServerInformation.IdpConfiguration = JsonConvert.DeserializeObject<IdpOpenConfiguration>(httpGetResult.Response);
			}
			idpServerInformation.UnsecureIdpAvailable = httpGetResult != null;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception)
		{
			idpServerInformation.UnsecureIdpAvailable = false;
		}
		if (idpServerInformation.ValidatedServerUri == null && idpServerInformation.UnsecureIdpAvailable)
		{
			idpServerInformation.ValidatedServerUri = secureIdp.Uri;
		}
	}

	private async Task<bool> ValidateNonStandardPortAsync(IdpServerInformation idpServerInformation, Uri uriToValidate, bool isSecure, CancellationToken cancellationToken)
	{
		if (SslVerification == null)
		{
			return true;
		}
		string uriHostPort = $"{uriToValidate.Host}:{uriToValidate.Port}";
		SslEndpointVerificationResult sslEndpointVerificationResult = SslEndpointVerificationResult.NotVerified;
		if (_sslVerificationResults.TryGetValue(uriHostPort, out var value))
		{
			sslEndpointVerificationResult = value;
		}
		if (sslEndpointVerificationResult == SslEndpointVerificationResult.NotVerified)
		{
			sslEndpointVerificationResult = await new VerifyEndpointSupportsSslThread(SslVerification, uriToValidate).Run(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			_sslVerificationResults[uriHostPort] = sslEndpointVerificationResult;
			if (isSecure)
			{
				idpServerInformation.SecureSslVerification = sslEndpointVerificationResult;
			}
			else
			{
				idpServerInformation.UnsecureSslVerification = sslEndpointVerificationResult;
			}
		}
		if (sslEndpointVerificationResult == SslEndpointVerificationResult.TimedOut && ContinueOnSslVerificationTimeout)
		{
			return true;
		}
		if (isSecure && (sslEndpointVerificationResult == SslEndpointVerificationResult.SslSupported || sslEndpointVerificationResult == SslEndpointVerificationResult.SslSupportedInvalidCertificate))
		{
			return true;
		}
		if (!isSecure && sslEndpointVerificationResult == SslEndpointVerificationResult.SslNotSupported)
		{
			return true;
		}
		return false;
	}
}
