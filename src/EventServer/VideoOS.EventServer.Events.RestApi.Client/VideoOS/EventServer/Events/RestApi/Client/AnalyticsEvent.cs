using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AnalyticsEvent : Event
{
	[JsonProperty("data", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public AnalyticsData Data { get; set; }
}
