using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PatchAlarmBody
{
	[JsonProperty("comment", Required = Required.AllowNull)]
	public string Comment { get; set; }

	[JsonProperty("priority", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public Guid Priority { get; set; }

	[JsonProperty("state", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public Guid State { get; set; }

	[JsonProperty("assignedTo.displayName", Required = Required.AllowNull)]
	public string AssignedToDisplayName { get; set; }

	[JsonProperty("reasonForClosing", Required = Required.AllowNull)]
	public string ReasonForClosing { get; set; }
}
