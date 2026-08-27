using System;

namespace VideoOS.EventServer.Communication.Status;

public class GetVisualizationChangesRequestObject : RequestObject
{
	private DateTime _timestamp;

	private int _timeout;

	public DateTime Timestamp => _timestamp;

	public int Timeout => _timeout;

	public GetVisualizationChangesRequestObject(int requestId, DateTime timestamp, int timeout)
		: base(requestId)
	{
		_timestamp = timestamp;
		_timeout = timeout;
	}
}
