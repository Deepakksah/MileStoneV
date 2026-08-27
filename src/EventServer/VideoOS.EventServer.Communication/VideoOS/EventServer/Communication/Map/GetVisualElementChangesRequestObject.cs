using System;

namespace VideoOS.EventServer.Communication.Map;

public class GetVisualElementChangesRequestObject : RequestObject
{
	private bool _includeSubElementChanges;

	private bool _ignoreCleanup;

	private string _id;

	private DateTime _timestamp;

	private int _timeout;

	public bool IncludeSubElementChanges => _includeSubElementChanges;

	public bool IgnoreCleanup => _ignoreCleanup;

	public string Id => _id;

	public DateTime Timestamp => _timestamp;

	public int Timeout => _timeout;

	public GetVisualElementChangesRequestObject(int requestId, bool includeSubElementChanges, bool ignoreCleanup, string id, DateTime timestamp, int timeout)
		: base(requestId)
	{
		_includeSubElementChanges = includeSubElementChanges;
		_ignoreCleanup = ignoreCleanup;
		_id = id;
		_timestamp = timestamp;
		_timeout = timeout;
	}
}
