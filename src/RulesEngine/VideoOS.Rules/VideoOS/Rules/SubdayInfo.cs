using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class SubdayInfo
{
	public ScheduleTypes.SubdayType Type;

	public SubdayOnceInfo Once = new SubdayOnceInfo();

	public SubdayRecurringInfo Recurring = new SubdayRecurringInfo();
}
