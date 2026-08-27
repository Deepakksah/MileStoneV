using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PaginationLinks
{
	[JsonProperty("prev", Required = Required.AllowNull)]
	public string Prev { get; set; }

	[JsonProperty("next", Required = Required.AllowNull)]
	public string Next { get; set; }
}
