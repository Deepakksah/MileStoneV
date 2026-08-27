using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class AlarmUpdates
{
	[JsonProperty("time", Required = Required.Always)]
	[Required(AllowEmptyStrings = true)]
	public DateTime Time { get; set; }

	[JsonProperty("modifiedBy", Required = Required.AllowNull)]
	public string ModifiedBy { get; set; }

	[JsonProperty("modifiedFields", Required = Required.Always)]
	[Required]
	public ICollection<AlarmUpdatedField> ModifiedFields { get; set; } = new Collection<AlarmUpdatedField>();

	[JsonProperty("comment", Required = Required.AllowNull)]
	public string Comment { get; set; }
}
