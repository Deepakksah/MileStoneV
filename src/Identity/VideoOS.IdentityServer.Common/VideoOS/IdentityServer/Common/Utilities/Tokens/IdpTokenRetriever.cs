using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using VideoOS.IdentityServer.Common.Exceptions;

namespace VideoOS.IdentityServer.Common.Utilities.Tokens;

public sealed class IdpTokenRetriever : IIdpTokenRetriever, IDisposable
{
	private class HttpListenerChecker
	{
		private HttpListener _httpListener;

		public bool ShouldCheckCancel { get; set; }

		public HttpListenerChecker(HttpListener httpListener)
		{
			_httpListener = httpListener;
		}

		public async Task CheckCancel(CancellationToken cancellationToken)
		{
			ShouldCheckCancel = true;
			while (ShouldCheckCancel)
			{
				await Task.Delay(100, CancellationToken.None).ConfigureAwait(continueOnCapturedContext: false);
				if (cancellationToken.IsCancellationRequested)
				{
					_httpListener.Stop();
					((IDisposable)_httpListener).Dispose();
					break;
				}
			}
		}
	}

	private const string IdpSignInUrlPath = "connect/authorize?response_type=code&nonce={0}&state={1}&code_challenge={2}&code_challenge_method=S256&client_id={3}&scope={4}&redirect_uri={5}&acr_values=idp%3A{6}&culture={7}&prompt=login";

	private const string LoginLandingPagePath = "Identity/Account/LoginSuccess";

	private const string LowerCaseIdpRegexString = "/idp/";

	private readonly string _idpUrl;

	private readonly HashSet<HttpListener> _listeners;

	private readonly string _scopeRequestedEscaped;

	public string[] ScopesRequested { get; }

	public IdpTokenRetriever(IIdentityServerClientSettings settings, string[] supportedScopes, string[] requestedScopes)
	{
		if (settings == null)
		{
			throw new ArgumentNullException("settings");
		}
		if (supportedScopes == null)
		{
			throw new ArgumentNullException("supportedScopes");
		}
		if (requestedScopes == null)
		{
			throw new ArgumentNullException("requestedScopes");
		}
		ScopesRequested = requestedScopes.Where((string rs) => supportedScopes.Contains(rs)).ToArray();
		_scopeRequestedEscaped = Uri.EscapeDataString(string.Join(" ", ScopesRequested));
		string text = settings.BaseServerAddress.AbsoluteUri.ToString();
		if (text.Last() != '/')
		{
			text += "/";
		}
		_idpUrl = Regex.Replace(text, "/idp/", "/idp/", RegexOptions.IgnoreCase);
		_listeners = new HashSet<HttpListener>();
	}

	public Task<TokenResponse> RetrieveTokensAsync(IdpTokenClient idpTokenClient, string externalProvider, string clientId)
	{
		return RetrieveTokensAsync(idpTokenClient, externalProvider, clientId, CancellationToken.None);
	}

	public async Task<TokenResponse> RetrieveTokensAsync(IdpTokenClient idpTokenClient, string externalProvider, string clientId, CancellationToken cancellationToken)
	{
		if (idpTokenClient == null)
		{
			throw new ArgumentNullException("idpTokenClient");
		}
		if (string.IsNullOrEmpty(externalProvider))
		{
			throw new ArgumentNullException("externalProvider");
		}
		if (string.IsNullOrEmpty(clientId))
		{
			throw new ArgumentNullException("clientId");
		}
		clientId = Uri.EscapeDataString(clientId);
		string nonce = Guid.NewGuid().ToString("N");
		string text = GenerateRandom32ByteDataBase64Url();
		string codeVerifier = GenerateRandom32ByteDataBase64Url();
		string text2 = GenerateBase64UrlEncodeNoPadding(Sha256Ascii(codeVerifier));
		string redirectUri = CreateHttpListenerEndpoint();
		string text3 = HttpUtility.UrlEncode(redirectUri);
		string text4 = GrabCulture();
		string idpSignInUrl = _idpUrl + string.Format(CultureInfo.InvariantCulture, "connect/authorize?response_type=code&nonce={0}&state={1}&code_challenge={2}&code_challenge_method=S256&client_id={3}&scope={4}&redirect_uri={5}&acr_values=idp%3A{6}&culture={7}&prompt=login", nonce, text, text2, clientId, _scopeRequestedEscaped, text3, externalProvider, text4);
		string onSuccessRedirectUri = new UriBuilder(idpTokenClient.BaseAddress)
		{
			Path = Path.Combine(idpTokenClient.BaseAddress.AbsolutePath, "Identity/Account/LoginSuccess")
		}.Uri.ToString();
		return await ExchangeCodeForTokensAsync(idpTokenClient, await RetrieveCodeAsync(redirectUri, idpSignInUrl, onSuccessRedirectUri, text, cancellationToken).ConfigureAwait(continueOnCapturedContext: false), codeVerifier, redirectUri, nonce, clientId, string.Join(" ", ScopesRequested), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task<string> RetrieveCodeAsync(string redirectUri, string idpSignInUrl, string onSuccessRedirectUri, string state, CancellationToken cancellationToken)
	{
		HttpListener httpListener = null;
		try
		{
			HttpListener httpListener2;
			httpListener = (httpListener2 = new HttpListener());
			using (httpListener2)
			{
				AddListener(httpListener);
				HttpListenerChecker checker = new HttpListenerChecker(httpListener);
				Task checkCancelTask = checker.CheckCancel(cancellationToken);
				cancellationToken.ThrowIfCancellationRequested();
				httpListener.Prefixes.Add(redirectUri);
				httpListener.Start();
				Process.Start(new ProcessStartInfo
				{
					FileName = idpSignInUrl,
					UseShellExecute = true,
					Verb = "open"
				});
				HttpListenerContext httpListenerContext = await httpListener.GetContextAsync().ConfigureAwait(continueOnCapturedContext: false);
				string code = httpListenerContext.Request.QueryString["code"];
				string text = httpListenerContext.Request.QueryString["state"];
				if (string.IsNullOrEmpty(code))
				{
					throw new IdpNullCodeException();
				}
				if (text != state)
				{
					throw new IdpStateMismatchException();
				}
				ProvideBrowserResponse(httpListenerContext, onSuccessRedirectUri);
				await Task.Delay(100, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				httpListener.Stop();
				checker.ShouldCheckCancel = false;
				await checkCancelTask.ConfigureAwait(continueOnCapturedContext: false);
				return code;
			}
		}
		catch (ObjectDisposedException)
		{
			cancellationToken.ThrowIfCancellationRequested();
			throw;
		}
		finally
		{
			RemoveListener(httpListener);
		}
	}

	private static async Task<TokenResponse> ExchangeCodeForTokensAsync(IdpTokenClient idpTokenClient, string code, string codeVerifier, string redirectUri, string expectedNonce, string clientId, string scope, CancellationToken cancellationToken)
	{
		if (string.IsNullOrEmpty(code))
		{
			return null;
		}
		TokenResponse tokenResponse = await idpTokenClient.ExchangeCodeForTokensAsync(code, codeVerifier, redirectUri, scope, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (string.IsNullOrEmpty(tokenResponse.Refresh_Token) || string.IsNullOrEmpty(tokenResponse.Access_Token) || string.IsNullOrEmpty(tokenResponse.Id_Token))
		{
			throw new IdpNullTokenException();
		}
		if (new JwtSecurityTokenHandler().ReadJwtToken(tokenResponse.Id_Token).Claims.FirstOrDefault((Claim c) => c.Type == "nonce").Value != expectedNonce)
		{
			throw new IdpNonceMismatchException();
		}
		return tokenResponse;
	}

	private static void ProvideBrowserResponse(HttpListenerContext context, string onSuccessRedirectUri)
	{
		HttpListenerResponse response = context.Response;
		response.Redirect(onSuccessRedirectUri);
		response.Close();
	}

	private static string CreateHttpListenerEndpoint()
	{
		int randomUnusedPort = GetRandomUnusedPort();
		return $"http://127.0.0.1:{randomUnusedPort}/";
	}

	private static int GetRandomUnusedPort()
	{
		TcpListener tcpListener = new TcpListener(IPAddress.Any, 0);
		tcpListener.Start();
		int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
		tcpListener.Stop();
		return port;
	}

	private static string GenerateBase64UrlEncodeNoPadding(byte[] buffer)
	{
		return Convert.ToBase64String(buffer).Replace("+", "-").Replace("/", "_")
			.Replace("=", "");
	}

	private static string GenerateRandom32ByteDataBase64Url()
	{
		byte[] array;
		using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
		{
			array = new byte[32];
			randomNumberGenerator.GetBytes(array);
		}
		return GenerateBase64UrlEncodeNoPadding(array);
	}

	private static string GrabCulture()
	{
		return Thread.CurrentThread.CurrentUICulture.Name;
	}

	private static byte[] Sha256Ascii(string text)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(text);
		using SHA256 sHA = SHA256.Create();
		return sHA.ComputeHash(bytes);
	}

	public void Dispose()
	{
		lock (_listeners)
		{
			foreach (HttpListener listener in _listeners)
			{
				listener.Abort();
			}
		}
	}

	private void AddListener(HttpListener httpListener)
	{
		lock (_listeners)
		{
			_listeners.Add(httpListener);
		}
	}

	private void RemoveListener(HttpListener httpListener)
	{
		lock (_listeners)
		{
			_listeners.Remove(httpListener);
		}
	}
}
