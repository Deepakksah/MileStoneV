using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class DeleteDisableAlarmItemRequestObject : RequestObject
{
	private Guid _sourceId;

	private Guid _disableAlarmItemId;

	public Guid SourceId => _sourceId;

	public Guid DisableAlarmItemId => _disableAlarmItemId;

	public DeleteDisableAlarmItemRequestObject(int requestId, Guid sourceId, Guid disableAlarmItemId)
		: base(requestId)
	{
		_sourceId = sourceId;
		_disableAlarmItemId = disableAlarmItemId;
	}
}
