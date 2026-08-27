using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Messaging;

[Serializable]
[DataContract(Name = "CommunicationIdFilter", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Messaging.Internal")]
[KnownType(typeof(FQID))]
public class CommunicationIdFilter
{
	[DataMember]
	private string _messageId;

	[DataMember]
	private bool __startWith;

	[DataMember]
	private bool __endsWith;

	[DataMember]
	private string __eventHeaderMessage;

	[DataMember]
	private string __eventHeaderType;

	public string MessageId => _messageId;

	public bool StartWith => __startWith;

	public bool EndsWith => __endsWith;

	public string EventHeaderType => __eventHeaderType;

	public override bool Equals(object obj)
	{
		if (!(obj is CommunicationIdFilter communicationIdFilter))
		{
			return false;
		}
		if (_messageId == communicationIdFilter.MessageId && __startWith == communicationIdFilter.__startWith)
		{
			return __endsWith == communicationIdFilter.__endsWith;
		}
		return false;
	}

	public override int GetHashCode()
	{
		int num = _messageId.GetHashCode();
		if (__eventHeaderType != null)
		{
			num += __eventHeaderType.GetHashCode();
		}
		if (__eventHeaderMessage != null)
		{
			num += __eventHeaderMessage.GetHashCode();
		}
		return num;
	}

	public CommunicationIdFilter(string messageId)
		: this(messageId, null, null)
	{
	}

	public CommunicationIdFilter(string messageId, string eventHeaderType)
		: this(messageId, eventHeaderType, null)
	{
	}

	public CommunicationIdFilter(string messageId, string eventHeaderType, string eventHeaderMessage)
	{
		if (string.IsNullOrEmpty(messageId))
		{
			throw new ArgumentNullException("messageId");
		}
		if (string.IsNullOrEmpty(eventHeaderType))
		{
			if (messageId[0] == '*')
			{
				_messageId = messageId.Substring(1);
				__endsWith = true;
			}
			else if (messageId[messageId.Length - 1] == '*')
			{
				_messageId = messageId.Substring(0, messageId.Length - 1);
				__startWith = true;
			}
			else
			{
				_messageId = messageId;
			}
			return;
		}
		if (messageId != "Server.NewEventsIndication")
		{
			throw new ArgumentException("Providing eventHeaderType is only supported for Server.NewEventsIndication");
		}
		_messageId = messageId;
		__eventHeaderType = eventHeaderType;
		if (!string.IsNullOrEmpty(eventHeaderMessage))
		{
			if (eventHeaderMessage[0] == '*')
			{
				__eventHeaderMessage = eventHeaderMessage.Substring(1);
				__endsWith = true;
			}
			else if (eventHeaderMessage[eventHeaderMessage.Length - 1] == '*')
			{
				__eventHeaderMessage = eventHeaderMessage.Substring(0, eventHeaderMessage.Length - 1);
				__startWith = true;
			}
			else
			{
				__eventHeaderMessage = eventHeaderMessage;
			}
		}
	}

	public bool Match(ExternalMessage message, FQID endPoint, EndPointType endPointType)
	{
		if ((endPoint == null || endPoint.EqualGuids(message.SourceEndPoint)) && (endPointType == EndPointType.Any || endPointType == message.SourceEndPointType))
		{
			if (!string.IsNullOrEmpty(__eventHeaderType))
			{
				if (message.MessageId != "Server.NewEventIndication" && message.MessageId != "Server.NewEventsIndication")
				{
					return false;
				}
				if (message.MessageId == "Server.NewEventIndication")
				{
					BaseEvent baseEvent = message.Data as BaseEvent;
					if (baseEvent.EventHeader.Type == __eventHeaderType)
					{
						return MessageMatch(baseEvent.EventHeader.Message, __eventHeaderMessage);
					}
					return false;
				}
				if (message.MessageId == "Server.NewEventsIndication")
				{
					if (message.Data is IEnumerable<object> enumerable)
					{
						foreach (BaseEvent item in enumerable)
						{
							if (item.EventHeader.Type == __eventHeaderType && MessageMatch(item.EventHeader.Message, __eventHeaderMessage))
							{
								return true;
							}
						}
					}
					return false;
				}
				EnvironmentManager.Instance.Log(error: true, "CommunicationIdfilter", "Received eventS Indication ??? - ignored");
				return false;
			}
			return MessageMatch(message.MessageId, _messageId);
		}
		return false;
	}

	private bool MessageMatch(string text, string matchText)
	{
		if (matchText == null)
		{
			return true;
		}
		if (__startWith)
		{
			return text.StartsWith(matchText, StringComparison.Ordinal);
		}
		if (__endsWith)
		{
			return text.EndsWith(matchText, StringComparison.Ordinal);
		}
		return text == matchText;
	}

	public bool MatchId(string messageId)
	{
		if (__startWith)
		{
			return messageId.StartsWith(_messageId, StringComparison.Ordinal);
		}
		if (__endsWith)
		{
			return messageId.EndsWith(_messageId, StringComparison.Ordinal);
		}
		return messageId == _messageId;
	}

	public override string ToString()
	{
		if (string.IsNullOrEmpty(__eventHeaderType))
		{
			return string.Format(CultureInfo.InvariantCulture, "Filter on: {0}{1}{2}", __endsWith ? "*" : "", _messageId, __startWith ? "*" : "");
		}
		return string.Format(CultureInfo.InvariantCulture, "Filter on: {0}, EventHeaderType:{1}, Messages:{2}{3}{4}", _messageId, __eventHeaderType, __endsWith ? "*" : "", __eventHeaderMessage ?? "*", __startWith ? "*" : "");
	}
}
