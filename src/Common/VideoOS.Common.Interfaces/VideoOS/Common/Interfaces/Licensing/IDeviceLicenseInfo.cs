using System;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IDeviceLicenseInfo
{
	DateTime LastModified { get; }

	Guid Id { get; }

	string UniqueIdentifier { get; }

	long License { get; }

	DateTime Expires { get; }

	LicenseUpdateAction UpdateType { get; set; }

	bool UpdateExpireDate(DateTime expireDate);
}
