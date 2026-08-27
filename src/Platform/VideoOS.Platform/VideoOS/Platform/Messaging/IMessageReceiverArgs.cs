namespace VideoOS.Platform.Messaging;

public interface IMessageReceiverArgs
{
	string Topic { get; }

	object Data { get; }
}
