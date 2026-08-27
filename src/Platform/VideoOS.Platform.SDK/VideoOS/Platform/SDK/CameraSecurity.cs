using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK;

public class CameraSecurity : DeviceSecurity<ICameraSecurityInfo, CameraSecurityInfo>
{
	private bool _events = true;

	private bool _outputs = true;

	public bool ExportAvi => _deviceSecurityInfo.ExportAvi;

	public bool ExportJpeg => _deviceSecurityInfo.ExportJpeg;

	public bool SmartSearch => _deviceSecurityInfo.SmartSearch;

	public PtzSecurity PtzSecurity => new PtzSecurity(_deviceSecurityInfo.PtzSecurity);

	public bool Events => _events;

	public bool Outputs => _outputs;

	public CameraSecurity(ICameraSecurityInfo cameraSecurityInfo)
		: base(cameraSecurityInfo)
	{
		_events = true;
	}
}
