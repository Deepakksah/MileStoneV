using Newtonsoft.Json;

namespace VideoOS.Mobile.Portable.OAuth;

public class ExternalProvider
{
	[JsonProperty("LoginProviderId")]
	public string LoginProviderId { get; set; }

	[JsonProperty("DisplayName")]
	public string DisplayName { get; set; }
}
