using System;

namespace VideoOS.EventServer.Communication.Map;

public class UpdateDisplayXMLRequestObject : RequestObject
{
	private string _id;

	private string _visualOwnerId;

	private string _displayXml;

	private DateTime _timestamp;

	private bool _force;

	public string Id => _id;

	public string VisualOwnerId => _visualOwnerId;

	public string DisplayXml => _displayXml;

	public DateTime Timestamp => _timestamp;

	public bool Force => _force;

	public UpdateDisplayXMLRequestObject(int requestId, string id, string visualOwnerId, string displayXml, DateTime timestamp, bool force)
		: base(requestId)
	{
		_id = id;
		_visualOwnerId = visualOwnerId;
		_displayXml = displayXml;
		_timestamp = timestamp;
		_force = force;
	}
}
