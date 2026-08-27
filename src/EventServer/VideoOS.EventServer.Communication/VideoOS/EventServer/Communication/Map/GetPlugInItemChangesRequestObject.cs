using System;

namespace VideoOS.EventServer.Communication.Map;

public class GetPlugInItemChangesRequestObject : RequestObject
{
	private DateTime _timestamp;

	private int _timeout;

	public DateTime Timestamp => _timestamp;

	public int Timeout => _timeout;

	public GetPlugInItemChangesRequestObject(int requestId, DateTime timestamp, int timeout)
		: base(requestId)
	{
		_timestamp = timestamp;
		_timeout = timeout;
	}
}
