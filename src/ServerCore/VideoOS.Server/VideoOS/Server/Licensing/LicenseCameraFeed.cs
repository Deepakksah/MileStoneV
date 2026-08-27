using System;
using System.Diagnostics;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Server.Licensing;

[DebuggerDisplay("{Mac}; {OriginalDeviceId}; {Icp}; {RecorderId}; {LicensedValue}")]
public sealed class LicenseCameraFeed
{
	public IHardwareInfo HardwareInfo { get; private set; }

	public IDeviceInfo DeviceInfo { get; private set; }

	public Guid RecorderId { get; private set; }

	public bool Icp { get; private set; }

	public int FeedCount { get; set; }

	public long LicensedValue { get; set; }

	public string UniqueId { get; private set; }

	public LicenseCameraFeed(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, Guid recorderId, bool icp, int feedCount, long licensedValue)
	{
		HardwareInfo = hardwareInfo ?? throw new ArgumentNullException("hardwareInfo");
		DeviceInfo = deviceInfo ?? throw new ArgumentNullException("deviceInfo");
		RecorderId = recorderId;
		Icp = icp;
		UniqueId = deviceInfo.UniqueIdentifier;
		FeedCount = feedCount;
		LicensedValue = licensedValue;
	}
}
