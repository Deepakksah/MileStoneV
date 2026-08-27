using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsColor
{
	[JsonProperty("a", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public byte A { get; set; }

	[JsonProperty("r", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public byte R { get; set; }

	[JsonProperty("g", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public byte G { get; set; }

	[JsonProperty("b", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public byte B { get; set; }
}
