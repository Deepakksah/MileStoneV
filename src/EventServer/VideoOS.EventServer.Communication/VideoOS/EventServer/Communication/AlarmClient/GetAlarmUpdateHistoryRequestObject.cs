using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class GetAlarmUpdateHistoryRequestObject : RequestObject
{
	public Guid Id { get; }

	public GetAlarmUpdateHistoryRequestObject(int requestId, Guid id)
		: base(requestId)
	{
		Id = id;
	}
}
