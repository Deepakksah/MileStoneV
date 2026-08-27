using System.Collections.Generic;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class MetadataComponent : HardwareComponent
{
	public override string GetComponentName => "Metadata";

	public MetadataComponent(IEnumerable<EnabledComponent> metadataComponents)
		: base(metadataComponents)
	{
		base.ComponentName = Resources.HardwareSelectionMetadataComponentName;
	}
}
