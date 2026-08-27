using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PatchAlarmPriorityBody
{
	[JsonProperty("level", Required = Required.Always)]
	public int Level { get; set; }

	[JsonProperty("name", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Name { get; set; }

	[JsonProperty("soundNotificationsOnLoop", Required = Required.Always)]
	public bool SoundNotificationsOnLoop { get; set; }

	[JsonProperty("desktopNotificationsEnabled", Required = Required.Always)]
	public bool DesktopNotificationsEnabled { get; set; }

	[JsonProperty("soundId", Required = Required.AllowNull)]
	public Guid? SoundId { get; set; }
}
