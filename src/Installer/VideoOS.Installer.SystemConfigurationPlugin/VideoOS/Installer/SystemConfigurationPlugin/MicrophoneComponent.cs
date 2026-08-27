using System.Collections.Generic;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class MicrophoneComponent : HardwareComponent
{
	public override string GetComponentName => "Microphone";

	public MicrophoneComponent(IEnumerable<EnabledComponent> microphoneComponents)
		: base(microphoneComponents)
	{
		base.ComponentName = Resources.HardwareSelectionMicrophoneComponentName;
	}
}
