using System;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IRecorderLicenseInfo
{
	DateTime LastModified { get; }

	Guid Id { get; }

	LicenseUpdateAction UpdateType { get; set; }
}
