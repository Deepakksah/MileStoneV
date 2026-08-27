namespace VideoOS.EventServer.Communication.Configuration;

public class UpdateAlarmDefinitionRequestObject : RequestObject
{
	private AlarmDefinition _alarmDefinition;

	public AlarmDefinition AlarmDefinition => _alarmDefinition;

	public UpdateAlarmDefinitionRequestObject(int requestId, AlarmDefinition alarmDefinition)
		: base(requestId)
	{
		_alarmDefinition = alarmDefinition;
	}
}
