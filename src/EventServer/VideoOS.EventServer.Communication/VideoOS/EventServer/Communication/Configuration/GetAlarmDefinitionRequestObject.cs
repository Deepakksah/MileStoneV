using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class GetAlarmDefinitionRequestObject : RequestObject
{
	private Guid _alarmDefinitionId;

	public Guid AlarmDefinitionId => _alarmDefinitionId;

	public GetAlarmDefinitionRequestObject(int requestId, Guid alarmDefinitionId)
		: base(requestId)
	{
		_alarmDefinitionId = alarmDefinitionId;
	}
}
