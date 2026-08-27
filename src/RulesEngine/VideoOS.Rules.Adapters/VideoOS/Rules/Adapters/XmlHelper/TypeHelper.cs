namespace VideoOS.Rules.Adapters.XmlHelper;

public class TypeHelper
{
	public string RuleType { get; }

	public string EventType { get; }

	public string Sources { get; }

	public string TimerValue { get; }

	public string ScheduleValue { get; }

	public TypeHelper(string ruleType, string eventType, string sources, string timerValue, string scheduleValue)
	{
		RuleType = ruleType;
		EventType = eventType;
		Sources = sources;
		TimerValue = timerValue;
		ScheduleValue = scheduleValue;
	}
}
