using Newtonsoft.Json.Linq;

namespace VideoOS.Platform.Common.MessageClient;

public class NewMessageArgs
{
	public string Topic { get; set; }

	public JObject Data { get; set; }
}
