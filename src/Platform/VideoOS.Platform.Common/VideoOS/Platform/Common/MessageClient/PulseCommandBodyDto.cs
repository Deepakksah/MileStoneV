using Newtonsoft.Json;

namespace VideoOS.Platform.Common.MessageClient;

public class PulseCommandBodyDto
{
	[JsonProperty(/*Could not decode attribute arguments.*/)]
	public int Sequence { get; set; }
}
