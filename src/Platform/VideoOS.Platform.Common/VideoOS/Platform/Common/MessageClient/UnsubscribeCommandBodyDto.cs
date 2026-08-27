using Newtonsoft.Json;

namespace VideoOS.Platform.Common.MessageClient;

public class UnsubscribeCommandBodyDto
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public string Topic { get; set; }
}
