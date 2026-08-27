using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Maps.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class GetVisualItemResponse
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[Required]
	public VisualItem Data { get; set; } = new VisualItem();
}
