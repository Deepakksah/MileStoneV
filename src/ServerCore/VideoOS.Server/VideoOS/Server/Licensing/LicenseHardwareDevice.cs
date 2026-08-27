using System;
using System.Diagnostics;

namespace VideoOS.Server.Licensing;

[DebuggerDisplay("{Mac}; {RecorderId}; {LicensedValue}")]
public sealed class LicenseHardwareDevice
{
	public bool IsFreeLicense { get; private set; }

	public string Mac { get; private set; }

	public bool Icp { get; private set; }

	public Guid RecorderId { get; private set; }

	public long LicensedValue { get; set; }

	public LicenseHardwareDevice(string mac, bool icp, Guid recorderId, bool isFreeLicense)
	{
		Mac = mac;
		Icp = icp;
		RecorderId = recorderId;
		IsFreeLicense = isFreeLicense;
		LicensedValue = 0L;
	}
}
