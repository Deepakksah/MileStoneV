using System.Collections.Generic;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class SpeakerComponent : HardwareComponent
{
	public override string GetComponentName => "Speaker";

	public SpeakerComponent(IEnumerable<EnabledComponent> speakerComponents)
		: base(speakerComponents)
	{
		base.ComponentName = Resources.HardwareSelectionSpeakerComponentName;
	}
}
