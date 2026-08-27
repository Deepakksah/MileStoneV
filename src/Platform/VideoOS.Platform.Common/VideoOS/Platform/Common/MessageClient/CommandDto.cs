using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VideoOS.Platform.Common.MessageClient;

public class CommandDto
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public Guid Id { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public string Type { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public JObject Body { get; set; }
}
