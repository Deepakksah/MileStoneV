using System;
using Newtonsoft.Json;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class LicenseActivationContainer
{
	[JsonProperty("licenserequestfile")]
	public string LicenseRequestFile { get; set; }

	[JsonProperty("origin")]
	public string Origin { get; set; }

	public LicenseActivationContainer()
	{
	}

	public LicenseActivationContainer(string licenseRequestFile)
	{
		if (string.IsNullOrEmpty(licenseRequestFile))
		{
			throw new ArgumentNullException("licenseRequestFile");
		}
		LicenseRequestFile = licenseRequestFile;
		Origin = "Vms";
	}
}
