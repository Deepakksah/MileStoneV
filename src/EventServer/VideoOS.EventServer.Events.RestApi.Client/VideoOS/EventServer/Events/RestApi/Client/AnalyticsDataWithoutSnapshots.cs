using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsDataWithoutSnapshots
{
	[JsonProperty("description", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Description { get; set; }

	[JsonProperty("startTime", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public DateTimeOffset? StartTime { get; set; }

	[JsonProperty("endTime", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public DateTimeOffset? EndTime { get; set; }

	[JsonProperty("location", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Location { get; set; }

	[JsonProperty("count", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public int Count { get; set; }

	[JsonProperty("rules", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<AnalyticsRule> Rules { get; set; }

	[JsonProperty("objects", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<AnalyticsObject> Objects { get; set; }

	[JsonProperty("references", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<string> References { get; set; }

	[JsonProperty("vendor", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsVendor Vendor { get; set; }
}
