using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class MicrophoneSecurity : DeviceSecurity<IMicrophoneSecurityInfo, MicrophoneSecurityInfo>
{
	public MicrophoneSecurity(IMicrophoneSecurityInfo microphoneSecurity)
		: base(microphoneSecurity)
	{
	}
}
