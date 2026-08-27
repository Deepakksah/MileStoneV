using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class AcknowledgeRequestObject : RequestObject
{
	public Guid Id { get; }

	public AcknowledgeRequestObject(int requestId, Guid id)
		: base(requestId)
	{
		Id = id;
	}
}
