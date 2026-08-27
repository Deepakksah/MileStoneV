using System;

namespace VideoOS.Platform.Messaging;

public class MessageIdFilter : MessageFilter
{
	private string _messageId;

	private bool __startWith;

	public MessageIdFilter(string messageId)
	{
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
		if (__startWith)
		{
			return message.MessageId.StartsWith(_messageId, StringComparison.InvariantCulture);
		}
		return message.MessageId == _messageId;
	}
}
