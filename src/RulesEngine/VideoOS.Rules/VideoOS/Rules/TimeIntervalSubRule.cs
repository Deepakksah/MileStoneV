using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class TimeIntervalSubRule : SubRule
{
	private bool isIntervalStarted;

	public bool IsIntervalStarted
	{
		get
		{
			return isIntervalStarted;
		}
		set
		{
			isIntervalStarted = value;
		}
	}

	public TimeIntervalSubRule()
		: base(RuleType.TimeInterval)
	{
	}
}
