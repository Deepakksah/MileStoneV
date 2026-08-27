namespace VideoOS.Common.Interfaces.Licensing;

public interface ILicenseManagerComponentsProvider
{
	IHardwareLicenseCountEvaluator CreateHardwareLicenseCountEvaluator(ILicenseDocument licenseDocument);

	ILicenseCalculator CreateLicenseCalculator(ILicenseDocument licenseDocument);
}
