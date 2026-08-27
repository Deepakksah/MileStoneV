using System;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicensedHardwareProvider
{
	void IsHardwareLicensed(string macAddress);

	void IsDeviceLicensed(string mac, Guid deviceId);
}
