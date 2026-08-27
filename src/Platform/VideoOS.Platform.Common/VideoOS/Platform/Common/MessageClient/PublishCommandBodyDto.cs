using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VideoOS.Platform.Common.MessageClient;

public class PublishCommandBodyDto
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public string Topic { get; set; }

	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public JObject Data { get; set; }
}
