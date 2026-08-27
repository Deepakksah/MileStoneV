namespace VideoOS.Platform.Messaging;

public class MessageIdListFilter : MessageFilter
{
	private string[] _messageIdList;

	public MessageIdListFilter(string[] messageIdList)
	{
		_messageIdList = messageIdList;
	}

	public override bool Match(Message message, FQID destination, FQID sender)
	{
		string[] messageIdList = _messageIdList;
		for (int i = 0; i < messageIdList.Length; i++)
		{
			if (messageIdList[i] == message.MessageId)
			{
				return true;
			}
		}
		return false;
	}
}
