using System;

namespace VideoOS.Platform.Messaging;

public class MessageIdAndRelatedKindFilter : MessageFilter
{
	private string _messageId;

	private Guid _kind;

	private bool _startWith;

	public MessageIdAndRelatedKindFilter(string messageId, Guid kind)
	{
		if (!string.IsNullOrEmpty(messageId) && messageId[messageId.Length - 1] == '*')
		{
			_messageId = messageId.Substring(0, messageId.Length - 1);
			_startWith = true;
		}
		else
		{
			_messageId = messageId;
		}
		_kind = kind;
	}

	public override bool Match(Message message, FQID destination, FQID sender)
	{
		if (message == null || message.RelatedFQID == null || message.RelatedFQID.Kind != _kind)
		{
			return false;
		}
		if (_startWith)
		{
			return message.MessageId.StartsWith(_messageId, StringComparison.Ordinal);
		}
		return message.MessageId == _messageId;
	}
}
