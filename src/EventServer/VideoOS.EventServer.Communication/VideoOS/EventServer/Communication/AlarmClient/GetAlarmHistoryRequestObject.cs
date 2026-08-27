using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class GetAlarmHistoryRequestObject : RequestObject
{
	public Guid Id { get; }

	public GetAlarmHistoryRequestObject(int requestId, Guid id)
		: base(requestId)
	{
		Id = id;
	}
}
