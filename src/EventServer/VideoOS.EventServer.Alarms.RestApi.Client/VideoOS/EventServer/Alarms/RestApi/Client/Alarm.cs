using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class Alarm
{
	[JsonProperty("id", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public Guid Id { get; set; }

	[JsonProperty("localId", Required = Required.AllowNull)]
	public int? LocalId { get; set; }

	[JsonProperty("source", Required = Required.AllowNull)]
	public string Source { get; set; }

	[JsonProperty("time", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public DateTime Time { get; set; }

	[JsonProperty("lastUpdatedTime", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public DateTime LastUpdatedTime { get; set; }

	[JsonProperty("assignedTo", Required = Required.AllowNull)]
	public AlarmAssignation AssignedTo { get; set; }

	[JsonProperty("name", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Name { get; set; }

	[JsonProperty("message", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Message { get; set; }

	[JsonProperty("category", Required = Required.AllowNull)]
	public AlarmCategory Category { get; set; }

	[JsonProperty("priority", Required = Required.AllowNull)]
	public AlarmPriority Priority { get; set; }

	[JsonProperty("state", Required = Required.Always)]
	[Required]
	public AlarmState State { get; set; } = new AlarmState();

	[JsonProperty("data", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsDataWithoutSnapshots Data { get; set; }

	[JsonProperty("tag", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Tag { get; set; }

	[JsonProperty("legacyType", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string LegacyType { get; set; }

	[JsonProperty("cameraId", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
	public Guid CameraId { get; set; }
}
