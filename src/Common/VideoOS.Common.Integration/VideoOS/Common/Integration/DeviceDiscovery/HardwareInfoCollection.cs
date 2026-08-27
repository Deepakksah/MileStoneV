using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class HardwareInfoCollection : Collection<HardwareInfo>
{
	public HardwareInfo Add(string id)
	{
		HardwareInfo hardwareInfo = new HardwareInfo(id);
		Add(hardwareInfo);
		return hardwareInfo;
	}

	public HardwareInfo Add(string id, string userName, string password)
	{
		HardwareInfo hardwareInfo = new HardwareInfo(id, userName, password);
		Add(hardwareInfo);
		return hardwareInfo;
	}
}
