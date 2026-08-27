using Newtonsoft.Json;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class LicenseResponseContainer
{
	[JsonProperty("licenseFile")]
	public string LicenseFile { get; set; }

	[JsonProperty("status")]
	public string Status { get; set; }

	[JsonProperty("softwareLicenseCode")]
	public string SoftwareLicenseCode { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public string ErrorMessage { get; set; }
}
