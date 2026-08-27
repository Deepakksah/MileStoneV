using Newtonsoft.Json;

namespace VideoOS.IdentityServer.Common;

public class TokenResponse
{
	[JsonProperty("access_token")]
	public string Access_Token { get; set; }

	[JsonProperty("refresh_token")]
	public string Refresh_Token { get; set; }

	[JsonProperty("id_token")]
	public string Id_Token { get; set; }

	[JsonProperty("expires_in")]
	public long Expires_In { get; set; }

	[JsonProperty("token_type")]
	public string Token_Type { get; set; }

	public string Scope { get; set; }
}
