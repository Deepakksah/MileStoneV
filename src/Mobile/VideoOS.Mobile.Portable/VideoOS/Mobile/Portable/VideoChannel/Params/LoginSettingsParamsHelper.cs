using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class LoginSettingsParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("LoginType")]
		LoginType = 1u,
		[Description("Username")]
		UserName = 2u,
		[Description("Password")]
		Password = 4u,
		[Description("IdpClientId")]
		IdpClientId = 8u,
		[Description("IdentityToken")]
		IdentityToken = 16u,
		[Description("AccessToken")]
		AccessToken = 32u,
		[Description("RefreshToken")]
		RefreshToken = 64u,
		[Description("ClientType")]
		ClientType = 128u,
		UserLoginPossible = 7u,
		UserLoginMandatory = 6u,
		TokenLoginPossible = 121u,
		TokenLoginMandatory = 121u
	}
}
