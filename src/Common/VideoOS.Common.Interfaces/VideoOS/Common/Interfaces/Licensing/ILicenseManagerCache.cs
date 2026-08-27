using System;

namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseManagerCache
{
	bool IsRunning { get; }

	void ClearAllCaches();

	void AddLicenseSnapshot(ILicenseSnapshot licenseSnapshot);

	ILicenseSnapshot RetrieveLicenseSnapshot();

	void AddRecordingServerLicense(Guid recordingServerId, byte[] license);

	bool RetrieveRecordingServerLicense(Guid recordingServerId, out byte[] license);
}
