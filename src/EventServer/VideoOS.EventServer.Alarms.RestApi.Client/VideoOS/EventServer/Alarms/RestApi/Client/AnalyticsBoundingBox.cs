using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsBoundingBox
{
	[JsonProperty("top", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public double Top { get; set; }

	[JsonProperty("left", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public double Left { get; set; }

	[JsonProperty("bottom", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public double Bottom { get; set; }

	[JsonProperty("right", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public double Right { get; set; }

	[JsonProperty("color", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsColor Color { get; set; }
}
