using System.Collections.Generic;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class InputComponent : HardwareComponent
{
	public override string GetComponentName => "Input";

	public InputComponent(IEnumerable<EnabledComponent> inputComponents)
		: base(inputComponents)
	{
		base.ComponentName = Resources.HardwareSelectionInputComponentName;
	}
}
