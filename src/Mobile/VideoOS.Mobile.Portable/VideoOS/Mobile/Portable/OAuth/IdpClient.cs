using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.OAuth;

public class IdpClient
{
	private static string Scope = "openid profile managementserver offline_access email";

	private const string IdpSignInUrlPath = "response_type=code&nonce={0}&state={1}&code_challenge={2}&code_challenge_method=S256&client_id={3}&scope={4}&redirect_uri={5}&acr_values=idp%3A{6}&culture={7}&prompt=login";

	private string _clientId;

	private string _returnUrl;

	private string _state;

	private string _codeVerifier;

	private bool _stopRequired;

	private Tokens _tokens;

	private OpenIdConfiguration _openIdConfiguration;

	public IdpClient(OpenIdConfiguration openidConfiguration, string clientId, ICallbackClilent callbackClient)
	{
		_clientId = clientId;
		_returnUrl = callbackClient.RedirectUrl.ToString();
		callbackClient.SubscribeForCode(CodeSubscriber);
		_openIdConfiguration = openidConfiguration;
	}

	private void CodeSubscriber(HttpListenerContext context)
	{
		try
		{
			string text = context.Request.QueryString["code"];
			if (context.Request.QueryString["state"] == _state && !string.IsNullOrWhiteSpace(text))
			{
				string text2 = ExchangeCodeForTokenRespone(text, _codeVerifier, _returnUrl, Scope, _clientId);
				_tokens = JsonConvert.DeserializeObject<Tokens>(text2);
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "Failed to exchange code for token", ex.Message);
		}
	}

	public Tokens Login(string providerName)
	{
		_tokens = null;
		LaunchLoginPage(providerName);
		while (_tokens == null && !_stopRequired)
		{
			Task.Delay(500).Wait();
		}
		return _tokens;
	}

	private void LaunchLoginPage(string providerName)
	{
		_state = RandomGeneratorUtility.GetBase64UrlEncodedRandom(32);
		_codeVerifier = RandomGeneratorUtility.GetBase64UrlEncodedRandom(32);
		string text = Uri.EscapeDataString(_clientId);
		string text2 = Guid.NewGuid().ToString("N");
		string text3 = new Sha256HasherImplementer().ComputeHash(_codeVerifier);
		string text4 = Uri.EscapeDataString(Scope);
		string text5 = HttpUtility.UrlEncode(_returnUrl);
		string name = Thread.CurrentThread.CurrentUICulture.Name;
		UriBuilder uriBuilder = new UriBuilder(_openIdConfiguration.AuthorizationEndpoint);
		uriBuilder.Query = string.Format(CultureInfo.InvariantCulture, "response_type=code&nonce={0}&state={1}&code_challenge={2}&code_challenge_method=S256&client_id={3}&scope={4}&redirect_uri={5}&acr_values=idp%3A{6}&culture={7}&prompt=login", text2, _state, text3, text, text4, text5, providerName, name);
		Process.Start(new ProcessStartInfo(uriBuilder.Uri.ToString())
		{
			UseShellExecute = true
		});
	}

	public void Cancel()
	{
		_stopRequired = true;
	}

	public string ExchangeCodeForTokenRespone(string code, string codeVerifier, string redirectUri, string scope, string clientId)
	{
		Dictionary<string, string> source = new Dictionary<string, string>
		{
			{ "grant_type", "authorization_code" },
			{ "code", code },
			{ "code_verifier", codeVerifier },
			{ "redirect_uri", redirectUri },
			{ "scope", scope },
			{ "client_id", clientId }
		};
		return HttpHelper.PerformHTTPPost(_openIdConfiguration.TokenEndpoint, source.ToList());
	}
}
