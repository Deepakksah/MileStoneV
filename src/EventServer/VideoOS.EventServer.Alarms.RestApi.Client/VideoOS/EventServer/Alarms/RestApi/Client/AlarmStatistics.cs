using System.CodeDom.Compiler;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AlarmStatistics
{
	[JsonProperty("byState", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public ICollection<AlarmStatisticByState> ByState { get; set; }
}
