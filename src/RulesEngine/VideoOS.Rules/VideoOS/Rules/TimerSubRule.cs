using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class TimerSubRule : SubRule
{
	private int timerValue;

	public int TimerValue
	{
		get
		{
			return timerValue;
		}
		set
		{
			timerValue = value;
		}
	}

	public override bool CanBeStartRule => false;

	public TimerSubRule()
		: base(RuleType.Timer)
	{
	}
}
