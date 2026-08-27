using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Management.Server;

public interface ILicenseComponent : IDisposable
{
	DateTime QueryLicenseModifiedTime();

	ILicenseDocument QueryLicenseDocument();

	ILicenseInfo QueryLicenseItemById(Guid id);

	void SafelyAddOrUpdateLicenseItem(Guid id, DateTime previousLastModified, byte[] data);
}
