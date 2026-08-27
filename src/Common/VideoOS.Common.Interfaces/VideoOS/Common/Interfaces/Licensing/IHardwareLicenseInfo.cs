using System;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IHardwareLicenseInfo
{
	DateTime LastModified { get; }

	Guid Id { get; }

	string UniqueIdentifier { get; }

	long License { get; }

	DateTime ExpireDate { get; }

	LicenseUpdateAction UpdateType { get; set; }

	bool UpdateExpireDate(DateTime expireDate);
}
