using System;

namespace VideoOS.EventServer.Communication.Map;

public class UpdateNameRequestObject : RequestObject
{
	private string _id;

	private string _visualOwnerId;

	private string _name;

	private DateTime _timeStamp;

	private bool _force;

	public string Id => _id;

	public string VisualOwnerId => _visualOwnerId;

	public string Name => _name;

	public DateTime TimeStamp => _timeStamp;

	public bool Force => _force;

	public UpdateNameRequestObject(int requestId, string id, string visualOwnerId, string name, DateTime timeStamp, bool force)
		: base(requestId)
	{
		_id = id;
		_visualOwnerId = visualOwnerId;
		_name = name;
		_timeStamp = timeStamp;
		_force = force;
	}
}
