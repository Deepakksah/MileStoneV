using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class GetAlarmsSessionResponseData
{
	[JsonProperty("deletedFromSession", Required = Required.Always)]
	[Required]
	public ICollection<Guid> DeletedFromSession { get; set; } = new Collection<Guid>();

	[JsonProperty("addedToSession", Required = Required.Always)]
	[Required]
	public ICollection<Alarm> AddedToSession { get; set; } = new Collection<Alarm>();

	[JsonProperty("updatedInSession", Required = Required.Always)]
	[Required]
	public ICollection<Alarm> UpdatedInSession { get; set; } = new Collection<Alarm>();
}
