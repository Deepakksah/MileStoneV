using System;
using VideoOS.Common.WebSockets;

namespace VideoOS.Platform.Common.MessageClient;

public interface IIncomingCommandProcessor : IMessageProcessor<CommandDto>, IDisposable
{
	event EventHandler<MessageCommandBodyDto> MessageReceived;

	event EventHandler<HelloCommandBodyDto> HelloReceived;

	event EventHandler<AckCommandBodyDto> AckReceived;
}
