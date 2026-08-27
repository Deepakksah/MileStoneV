using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class AcknowledgeMultipleAlarmsRequestObject : RequestObject
{
	public Guid ObjectId { get; }

	public AcknowledgeMultipleAlarmsRequestObject(int requestId, Guid objectId)
		: base(requestId)
	{
		ObjectId = objectId;
	}
}
