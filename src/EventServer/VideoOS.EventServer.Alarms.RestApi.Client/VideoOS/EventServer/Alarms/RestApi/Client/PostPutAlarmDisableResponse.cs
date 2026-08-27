using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PostPutAlarmDisableResponse
{
	[JsonProperty("data", Required = Required.Always)]
	public AlarmDisable Data { get; set; }
}
