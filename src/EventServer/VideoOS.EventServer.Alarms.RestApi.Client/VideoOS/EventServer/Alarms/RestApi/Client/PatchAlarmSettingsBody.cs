using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PatchAlarmSettingsBody
{
	[JsonProperty("closedAlarmsRetentionPeriodDays", Required = Required.Always)]
	public int ClosedAlarmsRetentionPeriodDays { get; set; }

	[JsonProperty("otherAlarmsRetentionPeriodDays", Required = Required.Always)]
	public int OtherAlarmsRetentionPeriodDays { get; set; }

	[JsonProperty("traceEnabled", Required = Required.Always)]
	public bool TraceEnabled { get; set; }
}
