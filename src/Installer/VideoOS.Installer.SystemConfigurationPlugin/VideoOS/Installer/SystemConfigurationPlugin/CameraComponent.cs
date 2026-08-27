using System.Collections.Generic;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class CameraComponent : HardwareComponent
{
	public override string GetComponentName => "Camera";

	public CameraComponent(IEnumerable<EnabledComponent> cameraComponents)
		: base(cameraComponents)
	{
		base.ComponentName = Resources.HardwareSelectionCameraComponentName;
	}
}
