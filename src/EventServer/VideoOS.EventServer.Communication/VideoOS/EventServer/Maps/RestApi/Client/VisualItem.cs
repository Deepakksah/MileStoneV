using System;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Maps.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class VisualItem
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[Required(AllowEmptyStrings = true)]
	public string Name { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public string DisplayXml { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[Required]
	public Bounds Bounds { get; set; } = new Bounds();

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[Required(AllowEmptyStrings = true)]
	public DateTimeOffset TimeStamp { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	[Required]
	public Relations Relations { get; set; } = new Relations();
}
