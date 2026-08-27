using System;
using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsObject
{
	[JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public Guid Id { get; set; }

	[JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Name { get; set; }

	[JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Type { get; set; }

	[JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Description { get; set; }

	[JsonProperty("confidence", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public double Confidence { get; set; }

	[JsonProperty("value", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Value { get; set; }

	[JsonProperty("alarmTrigger", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public bool AlarmTrigger { get; set; }

	[JsonProperty("removed", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public bool Removed { get; set; }

	[JsonProperty("color", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Color { get; set; }

	[JsonProperty("size", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public double Size { get; set; }

	[JsonProperty("sizeUnit", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string SizeUnit { get; set; }

	[JsonProperty("boundingBox", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsBoundingBox BoundingBox { get; set; }

	[JsonProperty("polygon", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsPolygon Polygon { get; set; }

	[JsonProperty("motion", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsMotion Motion { get; set; }

	[JsonProperty("mask", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public byte[] Mask { get; set; }

	[JsonProperty("data", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Data { get; set; }
}
