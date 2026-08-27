using Newtonsoft.Json;

namespace VideoOS.Installer.Common.Utilities;

public class AccessTokenResponse
{
	[JsonProperty("access_token")]
	public string AccessToken { get; set; }

	[JsonProperty("expires_in")]
	public long ExpiresIn { get; set; }
}
