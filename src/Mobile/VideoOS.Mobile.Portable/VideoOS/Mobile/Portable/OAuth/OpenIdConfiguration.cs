using System;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.OAuth;

public class OpenIdConfiguration
{
	private const string WellKnownExtProviderPath = ".well-known/openid-configuration";

	[JsonProperty("authorization_endpoint")]
	public string AuthorizationEndpoint { get; set; }

	[JsonProperty("token_endpoint")]
	public string TokenEndpoint { get; set; }

	[JsonProperty("external_providers_uri")]
	public string ExternalProvidersEndpoint { get; set; }

	public static OpenIdConfiguration GetOpenIdConfiguration(Uri oauthEndpoint)
	{
		OpenIdConfiguration result = null;
		try
		{
			result = JsonConvert.DeserializeObject<OpenIdConfiguration>(HttpHelper.PerformHTTPGet(oauthEndpoint.AddPath(".well-known/openid-configuration").ToString()));
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "GetOpenIdConfiguration", ex.Message);
		}
		return result;
	}
}
