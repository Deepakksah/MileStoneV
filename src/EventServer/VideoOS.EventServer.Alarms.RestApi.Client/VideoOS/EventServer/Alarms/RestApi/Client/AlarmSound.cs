using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AlarmSound
{
	[JsonProperty("id", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public Guid Id { get; set; }

	[JsonProperty("name", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Name { get; set; }

	[JsonProperty("sound", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Sound { get; set; }
}
