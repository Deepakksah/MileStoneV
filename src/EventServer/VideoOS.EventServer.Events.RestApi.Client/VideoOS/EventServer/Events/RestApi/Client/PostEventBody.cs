using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[JsonConverter(typeof(JsonInheritanceConverter), new object[] { "datatype" })]
[JsonInheritance("analytics", typeof(PostAnalyticsEventBody))]
[JsonInheritance("free-form", typeof(PostFreeFormEventBody))]
[JsonInheritance("none", typeof(PostNoDataEventBody))]
[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PostEventBody
{
	[JsonProperty("type", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public Guid Type { get; set; }

	[JsonProperty("source", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Source { get; set; }

	[JsonProperty("time", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public DateTimeOffset? Time { get; set; }

	[JsonProperty("tag", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Tag { get; set; }
}
