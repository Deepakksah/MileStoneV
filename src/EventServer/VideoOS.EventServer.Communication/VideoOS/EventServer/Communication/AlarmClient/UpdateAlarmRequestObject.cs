using System;

namespace VideoOS.EventServer.Communication.AlarmClient;

public class UpdateAlarmRequestObject : RequestObject
{
	public Guid Id { get; }

	public string Text { get; }

	public int State { get; }

	public int Priority { get; }

	public DateTime Time { get; }

	public string AssignedTo { get; }

	public UpdateAlarmRequestObject(int requestId, Guid id, string text, int state, int priority, DateTime time, string assignedTo)
		: base(requestId)
	{
		Id = id;
		Text = text;
		State = state;
		Priority = priority;
		Time = time;
		AssignedTo = assignedTo;
	}
}
