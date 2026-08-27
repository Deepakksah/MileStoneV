using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PostAlarmBody
{
	[JsonProperty("name", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Name { get; set; }

	[JsonProperty("message", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Message { get; set; }

	[JsonProperty("category", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public Guid? Category { get; set; }

	[JsonProperty("priority", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public Guid? Priority { get; set; }

	[JsonProperty("state", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public Guid? State { get; set; }

	[JsonProperty("assignedTo.displayName", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string AssignedToDisplayName { get; set; }

	[JsonProperty("source", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Source { get; set; }

	[JsonProperty("time", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public DateTime? Time { get; set; }

	[JsonProperty("tag", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Tag { get; set; }

	[JsonProperty("data", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsData Data { get; set; }
}
