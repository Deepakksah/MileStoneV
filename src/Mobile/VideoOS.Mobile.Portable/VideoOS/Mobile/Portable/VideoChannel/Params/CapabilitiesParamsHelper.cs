namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class CapabilitiesParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		CameraId = 1u,
		ItemId = 2u,
		ItemKind = 4u,
		RequestMandatory = 2u,
		All = 7u
	}
}
