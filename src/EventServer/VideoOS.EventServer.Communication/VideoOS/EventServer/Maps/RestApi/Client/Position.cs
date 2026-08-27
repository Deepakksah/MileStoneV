using System.CodeDom.Compiler;
using Newtonsoft.Json;

namespace VideoOS.EventServer.Maps.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public class Position
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public double Latitude { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public double Longitude { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public double Altitude { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public double Angle { get; set; }
}
