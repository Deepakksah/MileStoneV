using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class GetAlarmParamsHelper
{
	public enum ParamMask
	{
		Nothing = 0,
		[Description("Count")]
		Count = 1,
		[Description("Operator")]
		Operator = 2,
		[Description("Timestamp")]
		Timestamp = 4,
		[Description("AlarmId")]
		AlarmId = 8,
		[Description("MyAlarms")]
		MyAlarms = 16,
		[Description("StatesNE")]
		StatesNE = 32,
		[Description("PrioritiesNE")]
		PrioritiesNE = 64,
		[Description("CategoriesNE")]
		CategoriesNE = 128,
		None = 0,
		ByAlarmId = 8,
		ByTimestamp = 7,
		ByStatesNE = 33,
		ByPrioritiesNE = 65,
		ByCategoriesNE = 129,
		All = 255
	}

	public enum OperatorType : uint
	{
		[Description("GreaterThan")]
		GreaterThan,
		[Description("LessThan")]
		LessThan
	}
}
