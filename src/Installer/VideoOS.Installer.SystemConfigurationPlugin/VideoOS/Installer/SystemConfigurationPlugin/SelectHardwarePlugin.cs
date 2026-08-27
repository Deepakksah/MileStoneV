using System.Linq;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class SelectHardwarePlugin : ProgressPlugin
{
	public SelectHardwarePlugin(int serverPort)
		: base(serverPort)
	{
	}

	public string DoSelectHardware(string configurationXml)
	{
		HardwareConfigurations hardwareConfigurations = HardwareConfigurations.GetFromXml(configurationXml);
		HardwareConfigurations hardwareConfigurations2 = new HardwareConfigurations();
		hardwareConfigurations.Where((HardwareConfiguration f) => f.Validated).ToArray().ForEach(delegate(HardwareConfiguration h)
		{
			hardwareConfigurations.Add(h);
		});
		return HardwareConfigurations.GetAsXml(hardwareConfigurations2);
	}
}
