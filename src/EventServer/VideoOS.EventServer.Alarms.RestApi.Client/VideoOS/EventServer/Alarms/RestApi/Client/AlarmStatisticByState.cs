using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AlarmStatisticByState
{
	[JsonProperty("level", Required = Required.Always)]
	public int Level { get; set; }

	[JsonProperty("count", Required = Required.Always)]
	public int Count { get; set; }
}
