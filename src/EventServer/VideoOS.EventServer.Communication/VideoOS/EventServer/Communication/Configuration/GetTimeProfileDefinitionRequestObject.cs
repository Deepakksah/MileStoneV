using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class GetTimeProfileDefinitionRequestObject : RequestObject
{
	private Guid _timeProfileDefinitionId;

	public Guid TimeProfileDefinitionId => _timeProfileDefinitionId;

	public GetTimeProfileDefinitionRequestObject(int requestId, Guid timeProfileDefinitionId)
		: base(requestId)
	{
		_timeProfileDefinitionId = timeProfileDefinitionId;
	}
}
