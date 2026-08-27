using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class UpdateMultipleAlarmsRequestObject : RequestObject
{
	public Guid OjectId { get; }

	public int OldState { get; }

	public int NewState { get; }

	public DateTime Time { get; }

	public string AssignedTo { get; }

	public UpdateMultipleAlarmsRequestObject(int requestId, Guid objectId, int oldState, int newState, DateTime time, string assignedTo)
		: base(requestId)
	{
		OjectId = objectId;
		OldState = oldState;
		NewState = newState;
		Time = time;
		AssignedTo = assignedTo;
	}
}
