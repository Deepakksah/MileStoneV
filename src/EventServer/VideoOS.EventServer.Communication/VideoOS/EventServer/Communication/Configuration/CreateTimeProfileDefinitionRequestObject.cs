namespace VideoOS.EventServer.Communication.Configuration;

public class CreateTimeProfileDefinitionRequestObject : RequestObject
{
	private TimeProfileDefinition _timeProfileDefinition;

	public TimeProfileDefinition TimeProfileDefinition => _timeProfileDefinition;

	public CreateTimeProfileDefinitionRequestObject(int requestId, TimeProfileDefinition timeProfileDefinition)
		: base(requestId)
	{
		_timeProfileDefinition = timeProfileDefinition;
	}
}
