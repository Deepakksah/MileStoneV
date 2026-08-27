using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.OAuth;

public class Tokens
{
	[JsonProperty("id_token")]
	public string IdToken { get; set; }

	[JsonProperty("access_token")]
	public string AccessToken { get; set; }

	[JsonProperty("refresh_token")]
	public string RefreshToken { get; set; }
}
