using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public class HardwareLicenseCountEvaluatorChannel : IHardwareLicenseCountEvaluator
{
	private readonly ILicenseWhitelist _licenseWhitelist;

	public string Version => _licenseWhitelist.Version;

	public HardwareLicenseCountEvaluatorChannel(ILicenseWhitelist licenseWhitelist)
	{
		_licenseWhitelist = licenseWhitelist ?? throw new ArgumentNullException("licenseWhitelist");
	}

	public bool UpdateLicenseCount(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (hardwareInfo.WhitelistStatus == HardwareWhitelistStatus.NotSet)
		{
			bool flag = _licenseWhitelist.IsHardwareWhitelisted(hardwareInfo);
			hardwareInfo.WhitelistStatus = (flag ? HardwareWhitelistStatus.Whitelisted : HardwareWhitelistStatus.NotWhitelisted);
		}
		return hardwareInfo.WhitelistStatus == HardwareWhitelistStatus.Whitelisted;
	}
}
