using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsRule
{
	[JsonProperty("id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public Guid Id { get; set; }

	[JsonProperty("name", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Name { get; set; }

	[JsonProperty("type", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Type { get; set; }

	[JsonProperty("polygons", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<AnalyticsPolygon> Polygons { get; set; }
}
