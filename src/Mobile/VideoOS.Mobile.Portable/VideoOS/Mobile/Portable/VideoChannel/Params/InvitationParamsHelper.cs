using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class InvitationParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("Slc")]
		Slc = 1u,
		[Description("ServerId")]
		ServerId = 4u,
		[Description("Emails")]
		Emails = 8u,
		[Description("StoreLink")]
		StoreLink = 16u,
		[Description("AppLink")]
		AppLink = 32u,
		[Description("SpecificCulture")]
		SpecificCulture = 64u,
		InvitationParamsInternalMandatory = 8u,
		InvitationParamsMandatory = 61u,
		InvitationParamsInternalPossible = 72u,
		InvitationParamsPossible = 125u,
		All = 125u
	}
}
