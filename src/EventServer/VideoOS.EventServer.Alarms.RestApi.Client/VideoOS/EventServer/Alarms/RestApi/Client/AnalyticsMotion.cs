using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsMotion
{
	[JsonProperty("speed", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public double Speed { get; set; }

	[JsonProperty("speedUnit", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string SpeedUnit { get; set; }

	[JsonProperty("path", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsPolygon Path { get; set; }
}
