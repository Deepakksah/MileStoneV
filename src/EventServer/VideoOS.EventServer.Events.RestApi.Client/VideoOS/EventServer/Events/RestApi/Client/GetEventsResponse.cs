using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class GetEventsResponse : PagedResponse
{
	[JsonProperty("array", Required = Required.Always)]
	[Required]
	public ICollection<Event> Array { get; set; } = new Collection<Event>();

	[JsonProperty("eventCount", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
	public int? EventCount { get; set; }
}
