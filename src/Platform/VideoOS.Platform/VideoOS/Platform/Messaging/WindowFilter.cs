using System;
using VideoOS.Platform.Client;

namespace VideoOS.Platform.Messaging;

public class WindowFilter : MessageFilter
{
	private Guid _windowId;

	private string _messageId;

	private bool __startWith;

	public WindowFilter(string messageId, Guid windowId)
	{
		_windowId = windowId;
		if (messageId.Length > 0 && messageId[messageId.Length - 1] == '*')
		{
			_messageId = messageId.Substring(0, messageId.Length - 1);
			__startWith = true;
		}
		else
		{
			_messageId = messageId;
		}
	}

	public WindowFilter(string messageId, WindowInformation windowInformation)
	{
		_windowId = windowInformation.WindowId;
		if (messageId.Length > 0 && messageId[messageId.Length - 1] == '*')
		{
			_messageId = messageId.Substring(0, messageId.Length - 1);
			__startWith = true;
		}
		else
		{
			_messageId = messageId;
		}
	}

	public override bool Match(Message message, FQID destination, FQID sender)
	{
		if (!WindowMatch(sender))
		{
			return false;
		}
		if (__startWith)
		{
			return message.MessageId.StartsWith(_messageId, StringComparison.InvariantCulture);
		}
		return message.MessageId == _messageId;
	}

	private bool WindowMatch(FQID sender)
	{
		if ((_windowId == Guid.Empty || _windowId == Kind.Window) && (sender == null || sender.ObjectId == Kind.Window))
		{
			return true;
		}
		if (_windowId == Guid.Empty || sender == null)
		{
			return false;
		}
		return _windowId == sender.ObjectId;
	}
}
