using System;
using System.Collections;
using System.Diagnostics;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.ConditionToken;

[DebuggerDisplay("RecurringTime")]
public class ScheduledTimeReachedFunction : FunctionToken, IEvaluate
{
	private readonly RecurringSchedule _schedule;

	private DateTime _nextScheduledDateTime;

	public ScheduledTimeReachedFunction(string scheduleXml, DateTime initialDateTime, ITimeProvider timeProvider)
	{
		_schedule = new RecurringSchedule();
		_schedule.ReadXmlString(scheduleXml);
		_nextScheduledDateTime = ((initialDateTime == DateTime.MinValue) ? GetNextScheduledDateTime(timeProvider.Now) : GetNextScheduledDateTime(initialDateTime));
	}

	public void Evaluate(TriggerEvent triggerEvent, Stack resultStack)
	{
		BooleanValueToken booleanValueToken = new BooleanValueToken
		{
			Value = false
		};
		if (triggerEvent.Time >= _nextScheduledDateTime.ToUniversalTime())
		{
			booleanValueToken.Value = true;
			_nextScheduledDateTime = GetNextScheduledDateTime(_nextScheduledDateTime);
		}
		resultStack.Push(booleanValueToken);
	}

	private DateTime GetNextScheduledDateTime(DateTime inputTime)
	{
		return _schedule.GetNextScheduleTime(inputTime);
	}
}
