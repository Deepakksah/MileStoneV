namespace VideoOS.EventServer.Communication.Configuration;

public class CreateAlarmDefinitionRequestObject : RequestObject
{
	private AlarmDefinition _alarmDefinition;

	public AlarmDefinition AlarmDefinition => _alarmDefinition;

	public CreateAlarmDefinitionRequestObject(int requestId, AlarmDefinition alarmDefinition)
		: base(requestId)
	{
		_alarmDefinition = alarmDefinition;
	}
}
