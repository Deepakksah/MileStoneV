using System;

namespace VideoOS.EventServer.Communication.Map;

public class GetTypeChangedRequestObject : RequestObject
{
	private Guid[] _types;

	private DateTime _timestamp;

	private int _timeout;

	public Guid[] Types => _types;

	public DateTime Timestamp => _timestamp;

	public int Timeout => _timeout;

	public GetTypeChangedRequestObject(int requestId, Guid[] types, DateTime timestamp, int timeout)
		: base(requestId)
	{
		_types = types;
		_timestamp = timestamp;
		_timeout = timeout;
	}
}
