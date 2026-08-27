using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AlarmDisable : SimpleAlarmDisable
{
	[JsonProperty("id", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public Guid Id { get; set; }

	[JsonProperty("enabledEventTypes", Required = Required.Always)]
	[Required]
	public ICollection<Guid> EnabledEventTypes { get; set; } = new Collection<Guid>();
}
