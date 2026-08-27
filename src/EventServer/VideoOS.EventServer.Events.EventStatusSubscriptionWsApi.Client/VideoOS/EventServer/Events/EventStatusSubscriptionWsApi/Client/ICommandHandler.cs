using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VideoOS.Common.WebSockets;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public interface ICommandHandler
{
	void SetOutgoingMessageProcessor(IMessageProcessor<string> outgoingMessageProcessor);

	Task<JObject> SendCommand(ICommand command, CancellationToken cancellationToken);
}
