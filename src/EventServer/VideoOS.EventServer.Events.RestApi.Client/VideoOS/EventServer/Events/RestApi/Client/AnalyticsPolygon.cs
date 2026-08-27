using System.CodeDom.Compiler;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsPolygon
{
	[JsonProperty("closed", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public bool Closed { get; set; }

	[JsonProperty("color", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsColor Color { get; set; }

	[JsonProperty("fillColor", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsColor FillColor { get; set; }

	[JsonProperty("points", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<AnalyticsPoint> Points { get; set; }
}
