using System;
using Newtonsoft.Json;

namespace VideoOS.Platform.Common.MessageClient;

public class AckCommandBodyDto
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public Guid AckId { get; set; }
}
