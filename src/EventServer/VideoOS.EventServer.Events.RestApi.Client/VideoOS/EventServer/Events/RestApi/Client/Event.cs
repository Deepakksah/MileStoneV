using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VideoOS.EventServer.Events.RestApi.Client;

[JsonConverter(typeof(JsonInheritanceConverter), new object[] { "datatype" })]
[JsonInheritance("analytics", typeof(AnalyticsEvent))]
[JsonInheritance("none", typeof(NoDataEvent))]
[JsonInheritance("access", typeof(AccessControlEvent))]
[JsonInheritance("free-form", typeof(FreeFormEvent))]
[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class Event
{
	[JsonProperty("specversion", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	[JsonConverter(typeof(StringEnumConverter))]
	public SpecVersion Specversion { get; set; }

	[JsonProperty("type", Required = Required.AllowNull)]
	public Guid? Type { get; set; }

	[JsonProperty("source", Required = Required.AllowNull)]
	public string Source { get; set; }

	[JsonProperty("time", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public DateTimeOffset Time { get; set; }

	[JsonProperty("id", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public Guid Id { get; set; }

	[JsonProperty("localId", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public int? LocalId { get; set; }

	[JsonProperty("tag", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Tag { get; set; }
}
