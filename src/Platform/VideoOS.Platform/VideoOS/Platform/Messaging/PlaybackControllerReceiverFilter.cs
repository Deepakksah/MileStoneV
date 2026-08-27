using System;
using VideoOS.Platform.Client;

namespace VideoOS.Platform.Messaging;

public class PlaybackControllerReceiverFilter : MessageFilter
{
	private FQID _playbackControllerFQID;

	private string _messageId;

	private bool __startWith;

	public PlaybackControllerReceiverFilter(string messageId, FQID playbackControllerFQID)
	{
		_playbackControllerFQID = playbackControllerFQID;
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

	public PlaybackControllerReceiverFilter(string messageId, WindowInformation windowInformation)
	{
		_playbackControllerFQID = windowInformation.PlaybackControllerFQID;
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
		if (!PlaybackControllerMatch(destination))
		{
			return false;
		}
		if (__startWith)
		{
			return message.MessageId.StartsWith(_messageId, StringComparison.InvariantCulture);
		}
		return message.MessageId == _messageId;
	}

	private bool PlaybackControllerMatch(FQID destination)
	{
		if (_playbackControllerFQID == null && destination == null)
		{
			return true;
		}
		if (_playbackControllerFQID == null || destination == null)
		{
			return false;
		}
		return _playbackControllerFQID.ObjectId == destination.ObjectId;
	}
}
