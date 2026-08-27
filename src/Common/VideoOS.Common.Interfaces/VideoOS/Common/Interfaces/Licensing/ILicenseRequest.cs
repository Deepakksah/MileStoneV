namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseRequest
{
	ILicenseRequest SnapshotOfCurrentHardwareToLicense { get; }

	int Recorders { get; set; }

	IHardwareDictionary HardwareList { get; }

	string GenerateLicenseRequestAsXml();

	string GenerateSerializedIntegrationRegistrations();

	bool IsActivationRequired();

	void AddHardwareToRequest(IHardwareInfo hardwareInfo);
}
