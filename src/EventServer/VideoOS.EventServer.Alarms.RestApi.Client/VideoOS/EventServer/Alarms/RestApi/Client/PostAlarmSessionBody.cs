using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PostAlarmSessionBody
{
	[JsonProperty("filterBy", Required = Required.Always)]
	[Required]
	public ICollection<AlarmSessionFilter> FilterBy { get; set; } = new Collection<AlarmSessionFilter>();

	[JsonProperty("orderBy", Required = Required.Always)]
	[Required]
	public ICollection<AlarmSessionOrderBy> OrderBy { get; set; } = new Collection<AlarmSessionOrderBy>();
}
