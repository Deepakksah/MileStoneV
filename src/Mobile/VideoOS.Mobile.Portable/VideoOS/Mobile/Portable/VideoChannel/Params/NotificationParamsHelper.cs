using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class NotificationParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("DeviceId")]
		DeviceId = 1u,
		[Description("Cloud")]
		Cloud = 2u,
		[Description("X")]
		DeviceToken = 4u,
		[Description("loc-key")]
		NotificationMessage = 8u,
		[Description("S")]
		ValidationSLC = 16u,
		[Description("ServerId")]
		ServerId = 32u,
		[Description("AlarmId")]
		AlarmId = 64u,
		[Description("PX")]
		OptionalParamX = 128u,
		[Description("PY")]
		OptionalParamY = 256u,
		[Description("PZ")]
		OptionalParamZ = 512u,
		[Description("DeviceName")]
		DeviceName = 1024u,
		[Description("Settings")]
		Settings = 2048u,
		Minimum = 1u,
		Mandatory = 1025u,
		MandatoryOnS = 2049u,
		All = 3969u,
		AllRegisterToOnS = 2983u
	}

	public enum CloudType
	{
		[Description("Apple")]
		Apple,
		[Description("Google")]
		Google,
		[Description("MS")]
		MS,
		[Description("Google2")]
		Google2
	}

	public enum NotificationResult
	{
		[Description("SentOK")]
		SentOK,
		[Description("Disconnected")]
		Disconnected,
		[Description("Unregistered")]
		Unregistered,
		[Description("InvalidSLC")]
		InvalidSLC,
		[Description("OnlineServiceCommFailure")]
		OnlineServiceCommFailure,
		[Description("UnknownError")]
		UnknownError
	}
}
