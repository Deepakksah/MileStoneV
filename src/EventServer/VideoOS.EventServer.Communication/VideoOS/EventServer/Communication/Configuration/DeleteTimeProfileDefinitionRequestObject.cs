using System;

namespace VideoOS.EventServer.Communication.Configuration;

public class DeleteTimeProfileDefinitionRequestObject : RequestObject
{
	private Guid _timeProfileDefinitionId;

	public Guid TimeProfileDefinitionId => _timeProfileDefinitionId;

	public DeleteTimeProfileDefinitionRequestObject(int requestId, Guid timeProfileDefinitionId)
		: base(requestId)
	{
		_timeProfileDefinitionId = timeProfileDefinitionId;
	}
}
