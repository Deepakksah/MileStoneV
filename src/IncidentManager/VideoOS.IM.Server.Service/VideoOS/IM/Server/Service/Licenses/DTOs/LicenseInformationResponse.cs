using System.Collections.Generic;
using Newtonsoft.Json;

namespace VideoOS.IM.Server.Service.Licenses.DTOs;

public sealed class LicenseInformationResponse
{
	[JsonProperty("array")]
	public List<LicenseInstalledProduct> LicenseInstalledProducts { get; set; } = new List<LicenseInstalledProduct>();
}
