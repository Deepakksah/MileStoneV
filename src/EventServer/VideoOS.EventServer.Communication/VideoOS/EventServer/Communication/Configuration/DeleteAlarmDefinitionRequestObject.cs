using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class DeleteAlarmDefinitionRequestObject : RequestObject
{
	private Guid _alarmDefinitionId;

	public Guid AlarmDefinitionId => _alarmDefinitionId;

	public DeleteAlarmDefinitionRequestObject(int requestId, Guid alarmDefinitionId)
		: base(requestId)
	{
		_alarmDefinitionId = alarmDefinitionId;
	}
}
