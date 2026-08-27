namespace VideoOS.EventServer.Communication.Configuration;

public class UpdateTimeProfileDefinitionRequestObject : RequestObject
{
	private TimeProfileDefinition _timeProfileDefinition;

	public TimeProfileDefinition TimeProfileDefinition => _timeProfileDefinition;

	public UpdateTimeProfileDefinitionRequestObject(int requestId, TimeProfileDefinition timeProfileDefinition)
		: base(requestId)
	{
		_timeProfileDefinition = timeProfileDefinition;
	}
}
