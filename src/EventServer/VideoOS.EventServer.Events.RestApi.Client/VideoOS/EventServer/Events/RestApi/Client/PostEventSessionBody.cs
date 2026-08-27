using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class PostEventSessionBody
{
	[JsonProperty("filterBy", Required = Required.Always)]
	[Required]
	public ICollection<EventSessionFilter> FilterBy { get; set; } = new Collection<EventSessionFilter>();

	[JsonProperty("orderBy", Required = Required.Always)]
	[Required]
	public ICollection<EventSessionOrderBy> OrderBy { get; set; } = new Collection<EventSessionOrderBy>();
}
