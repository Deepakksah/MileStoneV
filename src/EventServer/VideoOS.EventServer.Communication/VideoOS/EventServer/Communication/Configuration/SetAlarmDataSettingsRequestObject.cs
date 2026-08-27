namespace VideoOS.EventServer.Communication.Configuration;

public class SetAlarmDataSettingsRequestObject : RequestObject
{
	private AlarmDataSettings _settings;

	public AlarmDataSettings Settings => _settings;

	public SetAlarmDataSettingsRequestObject(int requestId, AlarmDataSettings settings)
		: base(requestId)
	{
		_settings = settings;
	}
}
