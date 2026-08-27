using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class SimpleAlarmDisable
{
	[JsonProperty("source", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public string Source { get; set; }

	[JsonProperty("disabledEventTypes", Required = Required.Always)]
	[Required]
	public ICollection<Guid> DisabledEventTypes { get; set; } = new Collection<Guid>();

	[JsonProperty("comment", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public string Comment { get; set; }

	[JsonProperty("expirationTime", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public DateTime ExpirationTime { get; set; }
}
