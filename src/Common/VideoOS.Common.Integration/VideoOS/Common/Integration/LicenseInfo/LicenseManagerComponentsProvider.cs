using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public class LicenseManagerComponentsProvider : ILicenseManagerComponentsProvider
{
	public IHardwareLicenseCountEvaluator CreateHardwareLicenseCountEvaluator(ILicenseDocument licenseDocument)
	{
		if (licenseDocument == null)
		{
			throw new ArgumentNullException("licenseDocument");
		}
		if (licenseDocument.IsFeatureLicensed("ChannelLicensing"))
		{
			return new HardwareLicenseCountEvaluatorChannel(new LicenseWhitelist(licenseDocument.WhitelistAsXmlNode));
		}
		return null;
	}

	public ILicenseCalculator CreateLicenseCalculator(ILicenseDocument licenseDocument)
	{
		if (licenseDocument == null)
		{
			throw new ArgumentNullException("licenseDocument");
		}
		if (licenseDocument.IsFeatureLicensed("ChannelLicensing"))
		{
			ILicenseCalculator licenseCalculator = LicenseFactory.CreateLicenseCalculator();
			licenseCalculator.Initialize(licenseDocument);
			return licenseCalculator;
		}
		return null;
	}
}
