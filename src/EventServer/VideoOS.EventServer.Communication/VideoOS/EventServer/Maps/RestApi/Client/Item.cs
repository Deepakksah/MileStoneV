using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Maps.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class Item
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[Required(AllowEmptyStrings = true)]
	public Guid Type { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[Required(AllowEmptyStrings = true)]
	public Guid Id { get; set; }
}
