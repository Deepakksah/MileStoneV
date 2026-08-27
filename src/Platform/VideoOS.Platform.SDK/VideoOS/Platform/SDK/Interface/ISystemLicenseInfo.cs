using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ISystemLicenseInfo
{
	bool HasLicense { get; set; }

	bool IsTrialLicense { get; set; }

	bool HasCarePlus { get; set; }

	string SLC { get; set; }

	string ProductSku { get; set; }

	bool Expires { get; set; }

	DateTime ExpiryDate { get; set; }

	string[] Features { get; set; }

	bool IsEssential { get; set; }
}
