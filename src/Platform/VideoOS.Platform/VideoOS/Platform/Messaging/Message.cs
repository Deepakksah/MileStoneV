namespace VideoOS.Platform.Messaging;

public class Message
{
	private string _messageId;

	private FQID _relatedFQID;

	private object _data;

	private string _reason;

	public string MessageId => _messageId;

	public FQID RelatedFQID
	{
		get
		{
			return _relatedFQID;
		}
		set
		{
			_relatedFQID = value;
		}
	}

	public object Data
	{
		get
		{
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	public string Reason
	{
		get
		{
			return _reason;
		}
		set
		{
			_reason = value;
		}
	}

	public FQID ExternalMessageSourceEndPoint { get; set; }

	public Message(string messageId)
	{
		_messageId = messageId;
	}

	public Message(string messageId, FQID relatedFQID)
	{
		_messageId = messageId;
		_relatedFQID = relatedFQID;
	}

	public Message(string messageId, object data)
	{
		_messageId = messageId;
		_data = data;
	}

	public Message(string messageId, FQID relatedFQID, object data)
	{
		_messageId = messageId;
		_relatedFQID = relatedFQID;
		_data = data;
	}

	public Message(string messageId, FQID relatedFQID, object data, string reason)
	{
		_messageId = messageId;
		_relatedFQID = relatedFQID;
		_data = data;
		_reason = reason;
	}
}
