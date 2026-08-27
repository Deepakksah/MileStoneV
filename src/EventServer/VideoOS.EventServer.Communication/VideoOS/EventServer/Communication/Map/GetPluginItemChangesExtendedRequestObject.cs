using System;

namespace VideoOS.EventServer.Communication.Map;

public class GetPluginItemChangesExtendedRequestObject : RequestObject
{
	private DateTime _timestamp;

	private int _timeout;

	private Guid[] _kinds;

	private string _language;

	public DateTime Timestamp => _timestamp;

	public int Timeout => _timeout;

	public Guid[] Kinds => _kinds;

	public string Language => _language;

	public GetPluginItemChangesExtendedRequestObject(int requestId, DateTime timestamp, int timeout, Guid[] kinds, string language)
		: base(requestId)
	{
		_timestamp = timestamp;
		_timeout = timeout;
		_kinds = kinds;
		_language = language;
	}
}
