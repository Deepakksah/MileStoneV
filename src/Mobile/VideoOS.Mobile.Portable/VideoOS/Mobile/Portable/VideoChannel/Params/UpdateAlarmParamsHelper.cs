using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class UpdateAlarmParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("Id")]
		Id = 1u,
		[Description("Text")]
		Text = 2u,
		[Description("State")]
		State = 4u,
		[Description("Priority")]
		Priority = 8u,
		[Description("Time")]
		Time = 16u,
		[Description("AssignedTo")]
		AssignedTo = 32u,
		Mandatory = 1u,
		All = 63u
	}
}
