using Newtonsoft.Json;

namespace VideoOS.Platform.Common.MessageClient;

public class SubscribeCommandBodyDto
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public string Topic { get; set; }
}
