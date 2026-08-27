using System;
using System.Collections.ObjectModel;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Server.DataComponents;

namespace VideoOS.Server.Licensing;

public interface ITemporaryLicenseStorage : ITemporaryChangesWithoutActivation
{
	Guid Id { get; }

	Guid HomeSiteId { get; }

	string RSA { get; }

	void LoadData(IDataComponentFactory dataComponentFactory);

	bool PersistData(IDataComponentFactory dataComponentFactory);

	void Verify();

	void AddLicense(TemporaryLicense temporaryLicense);

	TemporaryLicense FindLicense(string macAddress);

	void RemoveHardwareLicense(string macAddress);

	void AddDeviceLicense(TemporaryDeviceLicense temporaryDevLicense);

	TemporaryDeviceLicense FindDeviceLicense(string macAddress, string uniqueDeviceId);

	void RemoveDeviceLicense(string macAddress, string uniqueDeviceId);

	void ClearChangesWithoutActivationTokens();

	Collection<string> FindHardwareWithPreviousYearTokens(DateTime activated);

	Collection<IHardwareDeviceItem> FindHardwareDevicesWithPreviousYearTokens(DateTime activated);
}
