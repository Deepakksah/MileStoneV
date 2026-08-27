using System;
using VideoOS.EventServer.Communication.Map.Client;

namespace VideoOS.EventServer.Communication.Map;

public class UpdateBoundsRequestObject : RequestObject
{
	private string _id;

	private string _visualOwnerId;

	private Bounds _bounds;

	private DateTime _timeStamp;

	private bool _force;

	public string Id => _id;

	public string VisualOwnerId => _visualOwnerId;

	public Bounds Bounds => _bounds;

	public DateTime Timestamp => Timestamp;

	public bool Force => _force;

	public UpdateBoundsRequestObject(int requestId, string id, string visualOwnerId, Bounds bounds, DateTime timestamp, bool force)
		: base(requestId)
	{
		_id = id;
		_visualOwnerId = visualOwnerId;
		_bounds = bounds;
		_timeStamp = timestamp;
		_force = force;
	}
}
