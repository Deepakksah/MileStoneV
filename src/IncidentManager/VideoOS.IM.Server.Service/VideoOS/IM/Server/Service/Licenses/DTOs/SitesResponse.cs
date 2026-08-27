using System.Collections.Generic;
using Newtonsoft.Json;

namespace VideoOS.IM.Server.Service.Licenses.DTOs;

public sealed class SitesResponse
{
	[JsonProperty("array")]
	public List<Site> Sites { get; set; } = new List<Site>();
}
