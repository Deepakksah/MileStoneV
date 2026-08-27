using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public class RecorderInfo : IRecorderInfo
{
	private readonly List<IHardwareInfo> _hardwareInfo;

	public Guid Id { get; private set; }

	public bool Enabled { get; private set; }

	public string DevicePackInfo { get; private set; }

	public IReadOnlyList<IHardwareInfo> Hardware => _hardwareInfo.AsReadOnly();

	public RecorderInfo(Guid id, bool enabled, string devicePackInfo)
	{
		if (id.Equals(Guid.Empty))
		{
			throw new ArgumentException("Guid can not be empty", "id");
		}
		Id = id;
		Enabled = enabled;
		DevicePackInfo = devicePackInfo ?? string.Empty;
		_hardwareInfo = new List<IHardwareInfo>();
	}

	public void AddHardware(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		_hardwareInfo.Add(hardwareInfo);
	}

	public void ClearExistingLicenseInformation()
	{
		_hardwareInfo.ForEach(delegate(IHardwareInfo hi)
		{
			ResetHardware(hi);
		});
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void ResetHardware(IHardwareInfo hardwareInfo)
	{
		hardwareInfo.LicenseState = HardwareLicenseState.Unknown;
		hardwareInfo.Devices.ForEach(delegate(IDeviceInfo d)
		{
			d.LicenseState = DeviceInfoLicenseState.Unknown;
		});
	}
}
