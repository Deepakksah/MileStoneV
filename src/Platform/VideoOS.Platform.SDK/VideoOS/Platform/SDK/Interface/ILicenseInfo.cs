using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ILicenseInfo
{
	Guid LicenseId { get; set; }

	string Data { get; set; }
}
