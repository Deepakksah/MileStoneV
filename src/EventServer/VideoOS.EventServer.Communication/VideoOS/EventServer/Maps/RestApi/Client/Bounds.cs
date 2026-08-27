using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Maps.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class Bounds
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public Position UpperLeftOrCenterPosition { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public Position LowerRightPosition { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public bool IsRealGps { get; set; }
}
