using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK;

public sealed class PtzSecurity
{
	private IPtzSecurityInfo _ptzSecurityInfo;

	public bool ManualControl => _ptzSecurityInfo.ManualControl;

	public bool PresetControl => _ptzSecurityInfo.PresetControl;

	public PtzSecurity(IPtzSecurityInfo ptzSecurityInfo)
	{
		_ptzSecurityInfo = ptzSecurityInfo;
	}
}
