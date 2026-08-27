namespace VideoOS.Common.Interfaces.Licensing;

public interface ICustomLicenseType
{
	CustomLicensingType CustomLicenseIsUsed(IHardwareInfo hardwareInfo);
}
