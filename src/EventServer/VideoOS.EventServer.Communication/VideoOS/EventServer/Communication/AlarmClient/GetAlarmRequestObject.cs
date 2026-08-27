using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class GetAlarmRequestObject : RequestObject
{
	public Guid Id { get; }

	public GetAlarmRequestObject(int requestId, Guid id)
		: base(requestId)
	{
		Id = id;
	}
}
