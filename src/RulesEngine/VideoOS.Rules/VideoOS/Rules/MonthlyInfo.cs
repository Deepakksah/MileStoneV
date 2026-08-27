using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class MonthlyInfo
{
	public int Day = -1;

	public ScheduleTypes.MonthlyType Type;

	public MonthlyAbsoluteDayInfo AbsoluteDay = new MonthlyAbsoluteDayInfo();

	public MonthlyRelativeDayInfo RelativeDay = new MonthlyRelativeDayInfo();
}
