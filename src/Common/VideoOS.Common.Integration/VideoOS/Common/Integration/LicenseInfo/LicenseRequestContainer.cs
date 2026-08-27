using System;
using System.Text;
using Newtonsoft.Json;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class LicenseRequestContainer
{
	[JsonProperty("licenseRequest")]
	public string LicenseRequest { get; set; }

	[JsonProperty("integrationData")]
	public string IntegrationData { get; set; }

	public LicenseRequestContainer()
	{
	}

	public LicenseRequestContainer(string licenseRequest)
	{
		if (string.IsNullOrEmpty(licenseRequest))
		{
			throw new ArgumentNullException("licenseRequest");
		}
		LicenseRequest = Convert.ToBase64String(Encoding.UTF8.GetBytes(licenseRequest));
	}

	public LicenseRequestContainer(string licenseRequest, string integrationData)
		: this(licenseRequest)
	{
		IntegrationData = integrationData;
	}
}
