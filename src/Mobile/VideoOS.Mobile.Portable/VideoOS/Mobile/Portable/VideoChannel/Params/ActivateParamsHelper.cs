using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ActivateParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing,
		[Description("ObjectId")]
		ObjectId,
		[Description("TriggerType")]
		TriggerType,
		All
	}

	public enum TriggerType : uint
	{
		[Description("TriggerOutput")]
		TriggerOutput,
		[Description("TriggerEvent")]
		TriggerEvent
	}
}
