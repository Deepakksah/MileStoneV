using System;
using Newtonsoft.Json;

namespace VideoOS.Platform.Common.MessageClient;

public class HelloCommandBodyDto
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public Guid SessionId { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public int PulsePeriodSeconds { get; set; }
}
