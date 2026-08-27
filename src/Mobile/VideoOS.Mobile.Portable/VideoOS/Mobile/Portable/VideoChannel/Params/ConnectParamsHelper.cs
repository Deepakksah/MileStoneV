using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ConnectParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		[Description("DeviceId")]
		DeviceId = 1u,
		[Description("PublicKey")]
		PublicKey = 2u,
		[Description("EncryptionPadding")]
		EncryptionPadding = 4u,
		[Description("PrimeLength")]
		PrimeLength = 8u,
		[Description("RequireCommandEncryption")]
		RequireCommandEncryption = 16u,
		ConnectPossible = 31u,
		All = 31u
	}
}
