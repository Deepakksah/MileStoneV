namespace VideoOS.Rules.Interfaces;

public static class ScheduleTypes
{
	public enum FrequencyType
	{
		Undefined,
		Daily,
		Weekly,
		Monthly
	}

	public enum MonthlyType
	{
		Undefined,
		AbsoluteDay,
		RelativeDay
	}

	public enum SubdayType
	{
		Undefined,
		Once,
		Recurring
	}

	public enum MonthlyRelativeOffset
	{
		Undefined,
		First,
		Second,
		Third,
		Fourth,
		Last
	}

	public enum MonthlyRelativeDay
	{
		Undefined,
		Monday,
		Tuesday,
		Wednesday,
		Thursday,
		Friday,
		Saturday,
		Sunday,
		Day,
		WeekDay,
		WeekendDay
	}
}
