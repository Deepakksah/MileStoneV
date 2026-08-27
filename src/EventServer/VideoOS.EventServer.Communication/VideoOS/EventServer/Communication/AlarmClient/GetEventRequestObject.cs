using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class GetEventRequestObject : RequestObject
{
	public Guid Id { get; }

	public GetEventRequestObject(int requestId, Guid id)
		: base(requestId)
	{
		Id = id;
	}
}
