using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public class SpeakerSecurity : DeviceSecurity<ISpeakerSecurityInfo, SpeakerSecurityInfo>
{
	public bool Speak => _deviceSecurityInfo.Speak;

	public SpeakerSecurity(ISpeakerSecurityInfo speakerSecurity)
		: base(speakerSecurity)
	{
	}
}
