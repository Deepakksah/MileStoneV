using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportMachineConfiguration;

public class MachineProperties : IMachineProperties
{
	public MachineProperty[] Properties
	{
		get
		{
			List<MachineProperty> list = new List<MachineProperty>();
			list.AddRange(GetMemoryStatus());
			list.AddRange(GetAdapterProperties());
			return list.ToArray();
		}
	}

	private static IEnumerable<MachineProperty> GetMemoryStatus()
	{
		NativeMethods.MEMORYSTATUSEX mEMORYSTATUSEX = new NativeMethods.MEMORYSTATUSEX();
		NativeMethods.GlobalMemoryStatusEx(mEMORYSTATUSEX);
		List<MachineProperty> list = new List<MachineProperty>
		{
			new MachineProperty("dwMemoryLoad", mEMORYSTATUSEX.dwMemoryLoad.ToString(CultureInfo.InvariantCulture)),
			new MachineProperty("ullAvailExtendedVirtual", mEMORYSTATUSEX.ullAvailExtendedVirtual.ToString(CultureInfo.InvariantCulture)),
			new MachineProperty("ullAvailPageFile", mEMORYSTATUSEX.ullAvailPageFile.ToString(CultureInfo.InvariantCulture)),
			new MachineProperty("ullAvailPhys", mEMORYSTATUSEX.ullAvailPhys.ToString(CultureInfo.InvariantCulture)),
			new MachineProperty("ullAvailVirtual", mEMORYSTATUSEX.ullAvailVirtual.ToString(CultureInfo.InvariantCulture)),
			new MachineProperty("ullTotalPageFile", mEMORYSTATUSEX.ullTotalPageFile.ToString(CultureInfo.InvariantCulture)),
			new MachineProperty("ullTotalPhys", mEMORYSTATUSEX.ullTotalPhys.ToString(CultureInfo.InvariantCulture)),
			new MachineProperty("ullTotalVirtual", mEMORYSTATUSEX.ullTotalVirtual.ToString(CultureInfo.InvariantCulture))
		};
		return list.ToArray();
	}

	private static IEnumerable<MachineProperty> GetAdapterProperties()
	{
		return (from nic in NetworkInterface.GetAllNetworkInterfaces()
			select new MachineProperty(nic.Description, nic.Speed.ToString(CultureInfo.InvariantCulture))).ToArray();
	}
}
