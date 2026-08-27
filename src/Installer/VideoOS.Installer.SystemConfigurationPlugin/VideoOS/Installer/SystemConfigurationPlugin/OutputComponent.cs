using System.Collections.Generic;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class OutputComponent : HardwareComponent
{
	public override string GetComponentName => "Output";

	public OutputComponent(IEnumerable<EnabledComponent> outputComponents)
		: base(outputComponents)
	{
		base.ComponentName = Resources.HardwareSelectionOutputComponentName;
	}
}
