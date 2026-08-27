using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Maps.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class GetMapsResponse : PagedResponse
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[Required]
	public ICollection<Map> Array { get; set; } = new Collection<Map>();
}
