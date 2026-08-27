using Newtonsoft.Json.Linq;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public interface IMessageParser
{
	MessageType GetMessageType(JObject messageData);

	ulong GetCommandId(JObject responseMessage);

	JObject ParseMessage(string receivedMessage);

	bool IsSuccessStatusCode(JObject responseMessage);
}
