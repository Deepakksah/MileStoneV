using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Common.MessageClient;

public class MessageReceiverArgs : IMessageReceiverArgs
{
	public string Topic { get; set; }

	public object Data { get; set; }
}
