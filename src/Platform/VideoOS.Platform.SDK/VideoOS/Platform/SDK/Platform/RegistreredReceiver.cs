using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.SDK.Platform;

public class RegistreredReceiver
{
	private MessageReceiver _messageReceiver;

	private MessageFilter _messageFilter;

	private bool _unregistered;

	public MessageReceiver MessageReceiver => _messageReceiver;

	public MessageFilter MessageFilter => _messageFilter;

	public bool Unregistered
	{
		get
		{
			return _unregistered;
		}
		set
		{
			_unregistered = value;
		}
	}

	public RegistreredReceiver(MessageReceiver messageReceiver, MessageFilter messageFilter)
	{
		_messageFilter = messageFilter;
		_messageReceiver = messageReceiver;
		_unregistered = false;
	}
}
