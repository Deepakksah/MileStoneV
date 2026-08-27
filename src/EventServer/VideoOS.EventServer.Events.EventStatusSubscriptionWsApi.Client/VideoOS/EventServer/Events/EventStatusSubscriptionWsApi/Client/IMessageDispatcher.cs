using Newtonsoft.Json.Linq;
using VideoOS.Common.WebSockets;
using VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public interface IMessageDispatcher
{
	void RegisterMessageReceiver(MessageType messageType, IMessageProcessor<JObject> messageProcessor);
}
