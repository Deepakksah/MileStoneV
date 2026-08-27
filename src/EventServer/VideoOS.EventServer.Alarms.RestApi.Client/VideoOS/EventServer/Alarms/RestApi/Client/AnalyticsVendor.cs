using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsVendor
{
	[JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Name { get; set; }

	[JsonProperty("customData", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string CustomData { get; set; }
}
